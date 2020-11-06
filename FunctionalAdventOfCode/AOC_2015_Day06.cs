using Common;
using FluentAssertions;
using FunctionalAdventOfCode.Common;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Sdk;

namespace FunctionalAdventOfCode
{
    public enum InstructionType
    {
        On,
        Off,
        Toggle
    }

    public class Instruction
    {
        public InstructionType Type { get; set; }
        public Coordinate Start { get; set; }
        public Coordinate End { get; set; }
    }

    public static class AOC_2015_Day06_Answer
    {
        public static Instruction ParseInstruction(string input) =>
            input.Split(" ").ToArray().Map(x => 
                new Instruction
                {
                    Type = (x[0], x[1]) switch
                    {
                        var s when s.Item2 == "on" => InstructionType.On,
                        var s when s.Item1 == "toggle" => InstructionType.Toggle,
                        var s when s.Item2 == "off" => InstructionType.Off
                    },
                    Start = (x[0] == "toggle" ? x[1] : x[2]).Split(",").ToArray().Map(
                            y => new Coordinate
                            {
                                X = y[0].ToInt(),
                                Y = y[1].ToInt()
                            }
                        ),
                    End = (x[0] == "toggle" ? x[3] : x[4]).Split(",").ToArray().Map(
                            y => new Coordinate
                            {
                                X = y[0].ToInt(),
                                Y = y[1].ToInt()
                            }
                        )
                }
            );



        internal static int CalculateLitLights(Instruction[] instructions) =>
                            Enumerable.Range(0, 1000)
                                .SelectMany(x => Enumerable.Range(0, 1000).Select(y => (X: x, Y: y)))
                                .Select(x => instructions.Where(y => x.X.InRange(y.Start.X, y.End.X) && x.Y.InRange(y.Start.Y, y.End.Y)))
                                .Select(x => x.Aggregate(false, (acc, curr) =>
                                        curr.Type switch
                                        {
                                            var y when y == InstructionType.Off => false,
                                            var y when y == InstructionType.On => true,
                                            var y when y == InstructionType.Toggle => !acc
                                        }
                                )).Count(x => x);
        internal static int CalculateLitLights2(Instruction[] instructions) =>
                         Enumerable.Range(0, 1000)
                             .SelectMany(x => Enumerable.Range(0, 1000).Select(y => (X: x, Y: y)))
                             .Select(x => instructions.Where(y => x.X.InRange(y.Start.X, y.End.X) && x.Y.InRange(y.Start.Y, y.End.Y)))
                             .Select(x => x.Aggregate(0, (acc, curr) =>
                                     curr.Type switch
                                     {
                                         var y when y == InstructionType.On => acc + 1,
                                         var y when y == InstructionType.Off && acc > 0 => acc - 1,
                                         var y when y == InstructionType.Toggle => acc + 2,
                                         _ => acc
                                     }
                             )).Sum(x => x);



    }

    public class AOC_2015_Day06
    {
        [Fact]
        public void parse_instruction_On()
        {
            var input = "turn on 0,0 through 999,999";
            var answer = AOC_2015_Day06_Answer.ParseInstruction(input);
            answer.Should().BeEquivalentTo(
                    new Instruction
                    {
                        Type = InstructionType.On,
                        Start = new Coordinate
                        {
                            X = 0,
                            Y = 0
                        },
                        End = new Coordinate
                        {
                            X = 999,
                            Y = 999
                        }
                    }
                ); ;
        }

        [Fact]
        public void parse_instruction_Off()
        {
            var input = "turn off 499,499 through 500,500";
            var answer = AOC_2015_Day06_Answer.ParseInstruction(input);
            answer.Should().BeEquivalentTo(
                    new Instruction
                    {
                        Type = InstructionType.Off,
                        Start = new Coordinate
                        {
                            X = 499,
                            Y = 499
                        },
                        End = new Coordinate
                        {
                            X = 500,
                            Y = 500
                        }
                    }
                ); ;
        }

        [Fact]
        public void parse_instruction_toggle()
        {
            var input = "toggle 0,0 through 999,0";
            var answer = AOC_2015_Day06_Answer.ParseInstruction(input);
            answer.Should().BeEquivalentTo(
                    new Instruction
                    {
                        Type = InstructionType.Toggle,
                        Start = new Coordinate
                        {
                            X = 0,
                            Y = 0
                        },
                        End = new Coordinate
                        {
                            X = 999,
                            Y = 0
                        }
                    }
                ); ;
        }

        [Fact]
        public void turn_on_all_lights()
        {
            var input = "turn on 0,0 through 999,999";
            var parsedInput = AOC_2015_Day06_Answer.ParseInstruction(input);
            var answer = AOC_2015_Day06_Answer.CalculateLitLights(new[] { parsedInput });
            answer.Should().Be(1000000);
        }

        [Fact]
        public void turn_on_first_line_of_lights()
        {
            var input = "toggle 0,0 through 999,0";
            var parsedInput = AOC_2015_Day06_Answer.ParseInstruction(input);
            var answer = AOC_2015_Day06_Answer.CalculateLitLights(new[] { parsedInput });
            answer.Should().Be(1000);
        }

        [Fact]
        public void increase_brightness_by_one()
        {
            var input = "turn on 0,0 through 0,0";
            var parsedInput = AOC_2015_Day06_Answer.ParseInstruction(input);
            var answer = AOC_2015_Day06_Answer.CalculateLitLights2(new[] { parsedInput });
            answer.Should().Be(1);
        }

        [Fact]
        public void turn_on_first_line_of_lights_then_turn_off_middle_200()
        {
            var instruction1 = AOC_2015_Day06_Answer.ParseInstruction("toggle 0,0 through 999,0");
            var instruction2 = AOC_2015_Day06_Answer.ParseInstruction("toggle 400,0 through 599,0");
            var answer = AOC_2015_Day06_Answer.CalculateLitLights(new[] { instruction1, instruction2 });
            answer.Should().Be(800);
        }

        [Fact]
        public void AOC_2015_Day06a()
        {
            var input = File.ReadAllText(".//Content//Day06.txt")
                            .Split(Environment.NewLine)
                            .Select(x => AOC_2015_Day06_Answer.ParseInstruction(x))
                            .ToArray();
            var answer = AOC_2015_Day06_Answer.CalculateLitLights(input);
            answer.Should().Be(377891);
        }

        [Fact]
        public void AOC_2015_Day06b()
        {
            var input = File.ReadAllText(".//Content//Day06.txt")
                            .Split(Environment.NewLine)
                            .Select(x => AOC_2015_Day06_Answer.ParseInstruction(x))
                            .ToArray();
            var answer = AOC_2015_Day06_Answer.CalculateLitLights2(input);
            answer.Should().Be(14110788);
        }
    }
}
