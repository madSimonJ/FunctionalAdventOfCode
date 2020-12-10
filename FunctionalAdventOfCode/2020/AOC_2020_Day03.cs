using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Common;

namespace FunctionalAdventOfCode._2020
{
    public static class AOC_2020_Day03_Answer
    {
        public static int CountTrees(string input)
        {
            var slope = input.Split(Environment.NewLine).ToArray();
            var verticalLength = slope.Length;
            var horizontalLength = slope[0].Length;

            var treeLookup = slope  .SelectMany((x, i) => x.Select((y, j) => (X: j, Y: i, Value: y)))
                                    .Where(x => x.Value == '#')
                                    .ToHashSet();
            var route = Enumerable.Range(0, verticalLength)
                        .Select((x, i) => (X: i * 3, Y: i, translatedX: (i * 3) % horizontalLength));
            var trees = route.Select(x => treeLookup.Contains((x.translatedX, x.Y, Value: '#')));
            return trees.Count(x => x);
        }

        public static int CountTreesForSlopeX(string input, int deltaX, int deltaY)
        {
            var slope = input.Split(Environment.NewLine).ToArray();
            var verticalLength = slope.Length;
            var horizontalLength = slope[0].Length;

            var treeLookup = slope.SelectMany((x, i) => x.Select((y, j) => (X: j, Y: i, Value: y)))
                                    .Where(x => x.Value == '#')
                                    .ToHashSet();
            var route = Enumerable.Range(0, verticalLength)
                        .Select((x, i) => (X: i * deltaX, Y: i * deltaY, translatedX: (i * deltaX) % horizontalLength));
            var trees = route.Select(x => treeLookup.Contains((x.translatedX, x.Y, Value: '#')));
            return trees.Count(x => x);
        }

    }

    public class AOC_2020_Day03
    {
        [Fact]
        public void test_tree_count()
        {
            var input = $"..##.......{Environment.NewLine}#...#...#..{Environment.NewLine}.#....#..#.{Environment.NewLine}..#.#...#.#{Environment.NewLine}.#...##..#.{Environment.NewLine}..#.##.....{Environment.NewLine}.#.#.#....#{Environment.NewLine}.#........#{Environment.NewLine}#.##...#...{Environment.NewLine}#...##....#{Environment.NewLine}.#..#...#.#";
            var answer = AOC_2020_Day03_Answer.CountTrees(input);
            answer.Should().Be(7);
        }

        [Fact]
        public void AOC_2020_Day03a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day03.txt");
            var answer = AOC_2020_Day03_Answer.CountTrees(input);
            answer.Should().Be(198);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(3, 1, 7)]
        [InlineData(5, 1, 3)]
        [InlineData(7, 1, 4)]
        [InlineData(1, 2, 2)]
        public void test_tree_count_variable_slope(int deltaX, int deltaY, int expectedResult)
        {
            var input = $"..##.......{Environment.NewLine}#...#...#..{Environment.NewLine}.#....#..#.{Environment.NewLine}..#.#...#.#{Environment.NewLine}.#...##..#.{Environment.NewLine}..#.##.....{Environment.NewLine}.#.#.#....#{Environment.NewLine}.#........#{Environment.NewLine}#.##...#...{Environment.NewLine}#...##....#{Environment.NewLine}.#..#...#.#";
            var answer = AOC_2020_Day03_Answer.CountTreesForSlopeX(input, deltaX, deltaY);
            answer.Should().Be(expectedResult);
        }

        [Fact]
        public void AOC_2020_Day03b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day03.txt");
            var answers = new[]
            {
                AOC_2020_Day03_Answer.CountTreesForSlopeX(input, 1, 1),
                AOC_2020_Day03_Answer.CountTreesForSlopeX(input, 3, 1),
                AOC_2020_Day03_Answer.CountTreesForSlopeX(input, 5, 1),
                AOC_2020_Day03_Answer.CountTreesForSlopeX(input, 7, 1),
                AOC_2020_Day03_Answer.CountTreesForSlopeX(input, 1, 2)
            };
            var answer = answers.Aggregate(1D, (acc, x) => acc * x);
            answer.Should().Be(5140884672);
        }
    }
}
