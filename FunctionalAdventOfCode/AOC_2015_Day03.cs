using Common;
using FluentAssertions;
using FunctionalAdventOfCode.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode
{

    public static class AOC_2015_Day03_Answer
    {
        private static Coordinate MoveNorth(Coordinate before) =>
            new Coordinate
            {
                X = before.X,
                Y = before.Y + 1
            };

        private static Coordinate MoveSouth(Coordinate before) =>
            new Coordinate
            {
                X = before.X,
                Y = before.Y - 1
            };

        private static Coordinate MoveEast(Coordinate before) =>
            new Coordinate
            {
                X = before.X + 1,
                Y = before.Y
            };

        private static Coordinate MoveWest(Coordinate before) =>
            new Coordinate
            {
                X = before.X - 1,
                Y = before.Y
            };

        private static IEnumerable<Coordinate> ToTrail(IEnumerable<char> moves)
        {
            var prev = new Coordinate
            {
                X = 0,
                Y = 0
            };

            yield return prev;

            foreach (var m in moves)
            {
                var newLocation = m switch
                {
                    _ when m == '^' => MoveNorth(prev),
                    _ when m == 'v' => MoveSouth(prev),
                    _ when m == '>' => MoveEast(prev),
                    _ when m == '<' => MoveWest(prev),
                    _ => prev
                };

                yield return newLocation;
                prev = newLocation;
            }
        }

        public static int CalculatePresentsDelivered(string input) =>
            ToTrail(input)
                .Select(x => $"{x.X},{x.Y}")
                .Distinct()
                .Count();

        public static int CalculatePresentsDeliveredWithRoboSanta(string input) =>
            input.Select((x, i) => (x, i))
                .GroupBy(x => x.i % 2 == 0)
                .Select(x => x.Select(y => y.x)).ToArray()
                .Map(x => ToTrail(x[0]).Concat(ToTrail(x[1])))
                .Select(x => $"{x.X},{x.Y}")
                .Distinct()
                .Count();
    }
        public class AOC_2015_Day03
    {
        [Theory]
        [InlineData(">", 2)]
        [InlineData("^>v<", 4)]
        [InlineData("^v^v^v^v^v", 2)]
        public void string_to_count_of_presents_delivered(string input, int expectedAnswer)
        {
            var output = AOC_2015_Day03_Answer.CalculatePresentsDelivered(input);
            output.Should().Be(expectedAnswer);
        }


        [Fact]
        public void AOC_2015_Day03a()
        {
            var input = File.ReadAllText(".//Content//Day03.txt");
            var output = AOC_2015_Day03_Answer.CalculatePresentsDelivered(input);
            output.Should().Be(2592);
        }

        [Theory]
        [InlineData("^v", 3)]
        [InlineData("^>v<", 3)]
        [InlineData("^v^v^v^v^v", 11)]
        public void string_to_count_of_presents_delivered_with_robosanta(string input, int expectedAnswer)
        {
            var output = AOC_2015_Day03_Answer.CalculatePresentsDeliveredWithRoboSanta(input);
            output.Should().Be(expectedAnswer);
        }

        [Fact]
        public void AOC_2015_Day03b()
        {
            var input = File.ReadAllText(".//Content//Day03.txt");
            var output = AOC_2015_Day03_Answer.CalculatePresentsDeliveredWithRoboSanta(input);
            output.Should().Be(2360);
        }
    }
}
