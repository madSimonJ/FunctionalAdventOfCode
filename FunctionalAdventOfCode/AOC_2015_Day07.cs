using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode
{
    public abstract class Command 
    {
        public string OutputWire { get; set; }
    }

    public class CompletedCommand : Command
    {
        public ushort Value { get; set; }
    }

    public class CommandAssigningValue : Command
    {
        public string ValueToAssign { get; set; }
    }

    public class CommandWithAndOperaiton : Command
    {
        public string LeftWire { get; set; }
        public string RightWire { get; set; }
    }

    public class CommandWithOrOperaiton : Command
    {
        public string LeftWire { get; set; }
        public string RightWire { get; set; }
    }

    public class CommandWithAnLshiftOperation : Command
    {
        public string WireToShift { get; set; }
        public ushort AmountToShiftBy { get; set; }
    }

    public class CommandWithAnRshiftOperation : Command
    {
        public ushort AmountToShiftBy { get; set; }
        public string WireToShift { get; set; }
    }

    public class CommandWithANotOperation : Command
    {
        public string WireToNot { get; set; }
    }


    public static class AOC_2015_Day07_Answer
    {
        public static Command ParseCommand(string input) =>
            input.Split(" ").ToArray() switch
            {
                var x when x[1] == "AND" => new CommandWithAndOperaiton
                {
                    LeftWire = x[0],
                    RightWire = x[2],
                    OutputWire = x[4]
                },
                var x when x[1] == "OR" => new CommandWithOrOperaiton
                {
                    LeftWire = x[0],
                    RightWire = x[2],
                    OutputWire = x[4]
                },
                var x when x[1] == "LSHIFT" => new CommandWithAnLshiftOperation
                {
                    AmountToShiftBy = x[2].ToUShort(),
                    OutputWire = x[4],
                    WireToShift = x[0]
                },
                var x when x[1] == "RSHIFT" => new CommandWithAnRshiftOperation
                {
                    AmountToShiftBy = x[2].ToUShort(),
                    OutputWire = x[4],
                    WireToShift = x[0]
                },
                var x when x[0] == "NOT" => new CommandWithANotOperation
                {
                    OutputWire = x[3],
                    WireToNot = x[1]
                },
                var x when x.Count() == 3 => new CommandAssigningValue
                {
                    ValueToAssign = x[0],
                    OutputWire = x[2]
                },
                _ => throw new Exception("boom")
            };

        private static KeyValuePair<string, ushort> RunCommand(Command c, IDictionary<string, ushort> wires) =>
            c switch
            {
                CommandAssigningValue cav => new KeyValuePair<string, ushort>(cav.OutputWire, cav.ValueToAssign.ToUShort()),
                CommandWithAndOperaiton cwao => new KeyValuePair<string, ushort>(cwao.OutputWire, (ushort)( (cwao.LeftWire == "1" ? 1 : wires[cwao.LeftWire])    & wires[cwao.RightWire])),
                CommandWithOrOperaiton cwoo => new KeyValuePair<string, ushort>(cwoo.OutputWire, (ushort)(wires[cwoo.LeftWire] | wires[cwoo.RightWire])),
                CommandWithAnLshiftOperation cwalo => new KeyValuePair<string, ushort>(cwalo.OutputWire, (ushort)(wires[cwalo.WireToShift] << cwalo.AmountToShiftBy)),
                CommandWithAnRshiftOperation cwaro => new KeyValuePair<string, ushort>(cwaro.OutputWire, (ushort)(wires[cwaro.WireToShift] >> cwaro.AmountToShiftBy)),
                CommandWithANotOperation cwano => new KeyValuePair<string, ushort>(cwano.OutputWire, (ushort)~wires[cwano.WireToNot]),
                _ => throw new Exception("boom")
            };

        public static IEnumerable<(string wire, ushort value)> Execute(Command[] commands) =>
            commands.Aggregate(new Dictionary<string, ushort>(),
                    (acc, curr) => acc.Append(RunCommand(curr, acc))
                                        .ToDictionary(x => x.Key, x => x.Value))
                .Select(x => (x.Key, x.Value));



        private static IEnumerable<Command> UpdateCommands(IEnumerable<Command> oldCommands)
        {
            var commandsDictionary = oldCommands.Where(x => x is CompletedCommand).ToDictionary(x => x.OutputWire, x => x as CompletedCommand);
            var returnvalue = oldCommands.Select(x =>
                x switch
                {
                    CommandAssigningValue cav when cav.ValueToAssign.IsNumeric() => new CompletedCommand { OutputWire = cav.OutputWire, Value = cav.ValueToAssign.ToUShort()},
                    CommandAssigningValue cav when commandsDictionary.ContainsKey(cav.ValueToAssign) => new CompletedCommand { OutputWire = cav.OutputWire, Value = commandsDictionary[cav.ValueToAssign].Value },
                    CommandWithAndOperaiton cwao when cwao.LeftWire == "1" && commandsDictionary.ContainsKey(cwao.RightWire) => new CompletedCommand { OutputWire = cwao.OutputWire, Value = (ushort)(1 & commandsDictionary[cwao.RightWire].Value) },
                    CommandWithAndOperaiton cwao when commandsDictionary.ContainsKey(cwao.LeftWire) && commandsDictionary.ContainsKey(cwao.RightWire) => new CompletedCommand { OutputWire = cwao.OutputWire, Value = (ushort)(commandsDictionary[cwao.LeftWire].Value & commandsDictionary[cwao.RightWire].Value) },
                    CommandWithOrOperaiton cwoo when commandsDictionary.ContainsKey(cwoo.LeftWire) && commandsDictionary.ContainsKey(cwoo.RightWire) => new CompletedCommand { OutputWire = cwoo.OutputWire, Value = (ushort)(commandsDictionary[cwoo.LeftWire].Value | commandsDictionary[cwoo.RightWire].Value) },
                    CommandWithAnLshiftOperation cwalo when commandsDictionary.ContainsKey(cwalo.WireToShift) => new CompletedCommand { OutputWire = cwalo.OutputWire, Value = (ushort)(commandsDictionary[cwalo.WireToShift].Value << cwalo.AmountToShiftBy) },
                    CommandWithAnRshiftOperation cwaro when commandsDictionary.ContainsKey(cwaro.WireToShift) => new CompletedCommand { OutputWire = cwaro.OutputWire, Value = (ushort)(commandsDictionary[cwaro.WireToShift].Value >> cwaro.AmountToShiftBy) },
                    CommandWithANotOperation cwano when commandsDictionary.ContainsKey(cwano.WireToNot) => new CompletedCommand { OutputWire = cwano.OutputWire, Value = (ushort)(~commandsDictionary[cwano.WireToNot].Value) },
                    Command c => c
                });
            return returnvalue;
        }

        public static ushort ExecuteCommands(IEnumerable<Command> commands, string commandToExecute) =>
            commands.All(x => x is CompletedCommand)
                ? (commands.First(x => x.OutputWire == commandToExecute) as CompletedCommand).Value
                : ExecuteCommands(
                        UpdateCommands(commands),
                        commandToExecute
                    );
    }

    public class AOC_2015_Day07
    {
        [Fact]
        public void parse_a_command_with_a_value()
        {
            var input = "123 -> x";
            var answer = AOC_2015_Day07_Answer.ParseCommand(input);
            answer.Should().BeEquivalentTo(
                    new CommandAssigningValue
                    {
                        OutputWire = "x",
                        ValueToAssign = "123"
                    }
                );
        }

        [Fact]
        public void parse_a_command_with_an_AND_operation()
        {
            var input = "x AND y -> z";
            var answer = AOC_2015_Day07_Answer.ParseCommand(input);
            answer.Should().BeEquivalentTo(
                    new CommandWithAndOperaiton
                    {
                        OutputWire = "z",
                        LeftWire = "x",
                        RightWire = "y"
                    }
                );
        }

        [Fact]
        public void parse_a_command_with_an_OR_operation()
        {
            var input = "x OR y -> z";
            var answer = AOC_2015_Day07_Answer.ParseCommand(input);
            answer.Should().BeEquivalentTo(
                    new CommandWithOrOperaiton
                    {
                        OutputWire = "z",
                        LeftWire = "x",
                        RightWire = "y"
                    }
                );
        }

        [Fact]
        public void parse_a_command_with_an_LSHIFT_operation()
        {
            var input = "p LSHIFT 2 -> q";
            var answer = AOC_2015_Day07_Answer.ParseCommand(input);
            answer.Should().BeEquivalentTo(
                    new CommandWithAnLshiftOperation
                    {
                        OutputWire = "q",
                        AmountToShiftBy = 2,
                        WireToShift = "p"
                    }
                );
        }

        [Fact]
        public void parse_a_command_with_an_RSHIFT_operation()
        {
            var input = "p RSHIFT 2 -> q";
            var answer = AOC_2015_Day07_Answer.ParseCommand(input);
            answer.Should().BeEquivalentTo(
                    new CommandWithAnRshiftOperation
                    {
                        OutputWire = "q",
                        AmountToShiftBy = 2,
                        WireToShift = "p"
                    }
                );
        }

        [Fact]
        public void parse_a_command_with_a_NOT_operation()
        {
            var input = "NOT e -> f";
            var answer = AOC_2015_Day07_Answer.ParseCommand(input);
            answer.Should().BeEquivalentTo(
                    new CommandWithANotOperation
                    {
                        OutputWire = "f",
                        WireToNot = "e"
                    }
                );
        }

        [Fact]
        public void assign_a_value_command()
        {
            var input = "123 -> x";
            var command = AOC_2015_Day07_Answer.ParseCommand(input);
            var answer = AOC_2015_Day07_Answer.Execute(new[] { command });
            answer.Should().BeEquivalentTo(new[] { ("x", 123) });
        }

        [Fact]
        public void run_an_AND_Operation()
        {
            var input = $"123 -> x{Environment.NewLine}456 -> y{Environment.NewLine}x AND y -> d"
                        .Split(Environment.NewLine)
                        .Select(x => AOC_2015_Day07_Answer.ParseCommand(x))
                        .ToArray();
            var answer = AOC_2015_Day07_Answer.Execute(input);
            answer.Should().BeEquivalentTo(new[]
            {
                ("x", 123),
                ("y", 456),
                ("d", 72),
            });
        }

        [Fact]
        public void run_an_OR_Operation()
        {
            var input = $"123 -> x{Environment.NewLine}456 -> y{Environment.NewLine}x OR y -> e"
                        .Split(Environment.NewLine)
                        .Select(x => AOC_2015_Day07_Answer.ParseCommand(x))
                        .ToArray();
            var answer = AOC_2015_Day07_Answer.Execute(input);
            answer.Should().BeEquivalentTo(new[]
            {
                ("x", 123),
                ("y", 456),
                ("e", 507),
            });
        }

        [Fact]
        public void run_an_LSHIFT_Operation()
        {
            var input = $"123 -> x{Environment.NewLine}456 -> y{Environment.NewLine}x LSHIFT 2 -> f"
                        .Split(Environment.NewLine)
                        .Select(x => AOC_2015_Day07_Answer.ParseCommand(x))
                        .ToArray();
            var answer = AOC_2015_Day07_Answer.Execute(input);
            answer.Should().BeEquivalentTo(new[]
            {
                ("x", 123),
                ("y", 456),
                ("f", 492),
            });
        }

        [Fact]
        public void run_an_RSHIFT_Operation()
        {
            var input = $"123 -> x{Environment.NewLine}456 -> y{Environment.NewLine}y RSHIFT 2 -> g"
                        .Split(Environment.NewLine)
                        .Select(x => AOC_2015_Day07_Answer.ParseCommand(x))
                        .ToArray();
            var answer = AOC_2015_Day07_Answer.Execute(input);
            answer.Should().BeEquivalentTo(new[]
            {
                ("x", 123),
                ("y", 456),
                ("g", 114),
            });
        }

        [Fact]
        public void run_a_NOT_Operation()
        {
            var input = $"123 -> x{Environment.NewLine}456 -> y{Environment.NewLine}NOT x -> h{Environment.NewLine}NOT y -> i"
                        .Split(Environment.NewLine)
                        .Select(x => AOC_2015_Day07_Answer.ParseCommand(x))
                        .ToArray();
            var answer = AOC_2015_Day07_Answer.Execute(input);
            answer.Should().BeEquivalentTo(new[]
            {
                ("x", 123),
                ("y", 456),
                ("h", 65412),
                ("i", 65079)
            });
        }

        [Fact]
        public void AOC_2015_Day07a()
        {
            var input = File.ReadAllText(".//Content//Day07.txt")
                            .Split(Environment.NewLine)
                            .Select(x => AOC_2015_Day07_Answer.ParseCommand(x))
                            .ToArray();
            var wireA = AOC_2015_Day07_Answer.ExecuteCommands(input, "a");

            var secondInput = input.AsEnumerable().Replace(x => x.OutputWire == "b", new CommandAssigningValue { OutputWire = "b", ValueToAssign = wireA.ToString() });
            var answer = AOC_2015_Day07_Answer.ExecuteCommands(secondInput, "a");

            answer.Should().Be(40149);

        }

    }
}
