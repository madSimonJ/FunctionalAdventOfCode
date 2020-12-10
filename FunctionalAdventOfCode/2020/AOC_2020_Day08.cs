using Common;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{
    public class Instruction
    {
        public string Command { get; set; }
        public string Parameter { get; set; }
    }

    public static class AOC_2020_Day08_Answer
    {
        public static IEnumerable<Instruction> ParseInstructions(string input) =>
            input.Split("\r\n")
                .Select(x => x.Split(" "))
                .Select(x => new Instruction
                {
                    Command = x[0],
                    Parameter = x[1]
                });

        public static int ExecuteToInfiniteLoop(IEnumerable<Instruction> input)
        {
            var inputArray = input.ToArray();
            var answer = Execute(new HashSet<int>());
            return answer;

            int Execute(HashSet<int> executedSoFar, int currentInstruction = 0, int accumulator = 0)
            {
                var curr = inputArray[currentInstruction];

                return executedSoFar.Contains(currentInstruction)
                    ? accumulator
                    : Execute(
                            executedSoFar.Append(currentInstruction).ToHashSet(),
                            curr.Command switch
                            {
                                "jmp" => currentInstruction + ((curr.Parameter[0] == '-' ? -1 : 1) * curr.Parameter[1..].ToInt()),
                                _ => currentInstruction + 1
                            },
                            curr.Command switch
                            {
                                "acc" => accumulator + ((curr.Parameter[0] == '-' ? -1 : 1) * curr.Parameter[1..].ToInt()),
                                _ => accumulator
                            }
                        );
            }
        }

        public static int ExecuteWithAdjustments(IEnumerable<Instruction> input)
        {
            var inputArray = input.ToArray();
            var alteredArrays = input.Select((x, i) =>
                x.Command switch
                {
                    "nop" => inputArray.Select((y, j) => i == j ? new Instruction { Command = "jmp", Parameter = x.Parameter } : y).ToArray(),
                    "jmp" => inputArray.Select((y, j) => i == j ? new Instruction { Command = "nop", Parameter = x.Parameter } : y).ToArray(),
                    _ => null
                }
            ).Where(x => x != null).ToArray();
            
            var answer = alteredArrays.Select((x, i) => Execute(i, new HashSet<int>())).First(x => x != 0);
            return answer;

            int Execute(int altArray, HashSet<int> executedSoFar, int currentInstruction = 0, int accumulator = 0)
            {
                var curr = alteredArrays[altArray][currentInstruction];

                return executedSoFar.Contains(currentInstruction) || (currentInstruction == (alteredArrays[altArray].Length - 1))
                    ? currentInstruction == (alteredArrays[altArray].Length-1) ? accumulator + (curr.Command == "acc" ? ((curr.Parameter[0] == '-' ? -1 : 1) * curr.Parameter[1..].ToInt()) : 0) : 0
                    : Execute(altArray,
                            executedSoFar.Append(currentInstruction).ToHashSet(),
                            curr.Command switch
                            {
                                "jmp" => currentInstruction + ((curr.Parameter[0] == '-' ? -1 : 1) * curr.Parameter[1..].ToInt()),
                                _ => currentInstruction + 1
                            },
                            curr.Command switch
                            {
                                "acc" => accumulator + ((curr.Parameter[0] == '-' ? -1 : 1) * curr.Parameter[1..].ToInt()),
                                _ => accumulator
                            }
                        );
            }
        }
    }

    public class AOC_2020_Day08
    {
        [Fact]
        public void parse_instructions()
        {
            var input = @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";

            var answer = AOC_2020_Day08_Answer.ParseInstructions(input);
            answer.Should().BeEquivalentTo(
                    new Instruction
                    {
                        Command = "nop",
                        Parameter = "+0"
                    },
                    new Instruction
                    {
                        Command = "acc",
                        Parameter = "+1"
                    },
                    new Instruction
                    {
                        Command = "jmp",
                        Parameter = "+4"
                    },
                    new Instruction
                    {
                        Command = "acc",
                        Parameter = "+3"
                    },
                    new Instruction
                    {
                        Command = "jmp",
                        Parameter = "-3"
                    },
                    new Instruction
                    {
                        Command = "acc",
                        Parameter = "-99"
                    },
                    new Instruction
                    {
                        Command = "acc",
                        Parameter = "+1"
                    },
                    new Instruction
                    {
                        Command = "jmp",
                        Parameter = "-4"
                    },
                    new Instruction
                    {
                        Command = "acc",
                        Parameter = "+6"
                    }
                );

        }

        [Fact]
        public void find_first_repeated_instruction()
        {
            var input = @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";

            var parsedInstructions = AOC_2020_Day08_Answer.ParseInstructions(input);
            var answer = AOC_2020_Day08_Answer.ExecuteToInfiniteLoop(parsedInstructions);
            answer.Should().Be(5);
        }

        [Fact]
        public void AOC_2020_Day08a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day08.txt");

            var parsedInstructions = AOC_2020_Day08_Answer.ParseInstructions(input);
            var answer = AOC_2020_Day08_Answer.ExecuteToInfiniteLoop(parsedInstructions);
            answer.Should().Be(1501);
        }

        [Fact]
        public void find_fix_instruction()
        {
            var input = @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";

            var parsedInstructions = AOC_2020_Day08_Answer.ParseInstructions(input);
            var answer = AOC_2020_Day08_Answer.ExecuteWithAdjustments(parsedInstructions);
            answer.Should().Be(8);
        }

        [Fact]
        public void AOC_2020_Day08b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day08.txt");

            var parsedInstructions = AOC_2020_Day08_Answer.ParseInstructions(input);
            var answer = AOC_2020_Day08_Answer.ExecuteWithAdjustments(parsedInstructions);
            answer.Should().Be(509);
        }
    }
}
