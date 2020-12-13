using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{
    public static class AOC_2020_Day10_Answer
    {
        public static (int OneJolt, int ThreeJolts) GetJoltDifferenceCount(IEnumerable<int> input) =>
            input.ToArray().OrderBy(x => x)
                .SelectAdjacentWithDefault((prev, curr) => curr - prev)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count())
                .Map(x => (
                    x[1],
                    x[3] + 1
                ));

        public static double GetNumberOfPossibilities(IEnumerable<int> input)
        {
            var inputArray = input.Concat(new[] { 0, input.Max() + 3 }).OrderBy(x => x).ToArray();
            var numberOfOptions = inputArray.Select(x =>
                inputArray.Where(y => y.InRange(x + 1, x + 3))
            );
            var skippableNumbers = numberOfOptions.SelectMany(x => x.Take(x.Count() - 1)).Distinct().ToArray();

            var answer = skippableNumbers.Aggregate(1D, (acc, x) =>
                skippableNumbers.Contains(x+1) &&
                skippableNumbers.Contains(x+2)
                        ? acc * 1.75
                        : acc * 2
            );
            return answer;
        }
    }

    public class AOC_2020_Day10
    {
        [Fact]
        public void get_counts_of_jolt_differences()
        {
            var input = new[] {
                16,
                10,
                15,
                5,
                1,
                11,
                7,
                19,
                6,
                12,
                4
            };

            var answer = AOC_2020_Day10_Answer.GetJoltDifferenceCount(input);
            answer.Should().Be((7, 5));
        }

        [Fact]
        public void get_number_of_possibilities()
        {
            var input = new[] {
                16,
                10,
                15,
                5,
                1,
                11,
                7,
                19,
                6,
                12,
                4
            };

            var answer = AOC_2020_Day10_Answer.GetNumberOfPossibilities(input);
            answer.Should().Be(8);
        }

        [Fact]
        public void get_counts_of_jolt_differences_2()
        {
            var input = new[] {
                    28,
                    33,
                    18,
                    42,
                    31,
                    14,
                    46,
                    20,
                    48,
                    47,
                    24,
                    23,
                    49,
                    45,
                    19,
                    38,
                    39,
                    11,
                    1,
                    32,
                    25,
                    35,
                    8,
                    17,
                    7,
                    9,
                    4,
                    2,
                    34,
                    10,
                    3
            };

            var answer = AOC_2020_Day10_Answer.GetJoltDifferenceCount(input);
            answer.Should().Be((22, 10));
        }

        [Fact]
        public void get_number_of_possibilities_2()
        {
            var input = new[] {
                    28,
                    33,
                    18,
                    42,
                    31,
                    14,
                    46,
                    20,
                    48,
                    47,
                    24,
                    23,
                    49,
                    45,
                    19,
                    38,
                    39,
                    11,
                    1,
                    32,
                    25,
                    35,
                    8,
                    17,
                    7,
                    9,
                    4,
                    2,
                    34,
                    10,
                    3
            };

            var answer = AOC_2020_Day10_Answer.GetNumberOfPossibilities(input);
            answer.Should().Be(19208);
        }

        [Fact]
        public void AOC_2020_Day10a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day10.txt");

            var parsedInput = input.Split("\r\n").Select(x => x.ToInt());
            var answer = AOC_2020_Day10_Answer.GetJoltDifferenceCount(parsedInput);
            answer.Should().Be((65, 32));

            var finalAnswer = answer.OneJolt * answer.ThreeJolts;
            finalAnswer.Should().Be(2080);
        }

        [Fact]
        public void AOC_2020_Day10b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day10.txt");

            var parsedInput = input.Split("\r\n").Select(x => x.ToInt());
            var answer = AOC_2020_Day10_Answer.GetNumberOfPossibilities(parsedInput);
            answer.Should().Be(6908379398144);
        }
    }

}
