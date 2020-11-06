using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace FunctionalAdventOfCode
{
    public static class AOC_2015_Day05_Answer
    {
        private static readonly IEnumerable<string> NaughtyStrings =
            new[] { "ab", "cd", "pq", "xy" };

        public static bool CalculateIsNice(string input) =>
            input.Validate(
                    x => x.CountOf('a', 'e', 'i', 'o', 'u') >= 3,
                    x => x.AnyAdjacent((prev, curr) => prev == curr ),
                    x => NaughtyStrings.All(y => x.IndexOf(y) == -1)
            );

        public static bool CalculateContainsSamePairOfLettersTwice(string input) =>
            input.AnyAdjacent((prev, curr, i) =>
            {
            var restofString = input.Substring(0, i).IndexOf(new string(new[] { prev, curr })) >=0 ||
                                input.Substring(i + 2).IndexOf(new string(new[] { prev, curr })) >=0;

                return restofString;
            });

        public static bool CalculateContainsSameLetterSeparatedByASingleLetter(string input) =>
            input.AnyAdjacentTwo((p2, p1, curr) => p2 == curr);

        public static bool CalculateIsNiceNewRules(string input) =>
            input.Validate(
                    x => CalculateContainsSamePairOfLettersTwice(x),
                    x => CalculateContainsSameLetterSeparatedByASingleLetter(x)
                );
    }

    public class AOC_2015_Day05
    {
        [Theory]
        [InlineData("ugknbfddgicrmopn", true)]
        [InlineData("aaa", true)]
        [InlineData("jchzalrnumimnmhp", false)]
        [InlineData("haegwjzuvuyypxyu", false)]
        [InlineData("dvszwmarrgswjxmb", false)]
        public void calculate_is_nice(string input, bool expectedAnswer)
        {
            var output = AOC_2015_Day05_Answer.CalculateIsNice(input);
            output.Should().Be(expectedAnswer);
        }


        [Fact]
        public void AOC_2015_Day05a()
        {
            var input = File.ReadAllText(".//Content//Day05.txt")
                .Split(Environment.NewLine);
            var output = input.Count(x => AOC_2015_Day05_Answer.CalculateIsNice(x));
            output.Should().Be(238);
        }

        [Theory]
        [InlineData("qjhvhtzxzqqjkmpb", true)]
        [InlineData("xxyxx", true)]
        [InlineData("uurcxstgmygtbstg", false)]
        [InlineData("ieodomkazucvgmuy", false)]
        [InlineData("zztdcqzqddaazdjp", false)]
        public void calculate_is_nice_new_rules(string input, bool expectedAnswer)
        {
            var output = AOC_2015_Day05_Answer.CalculateIsNiceNewRules(input);
            output.Should().Be(expectedAnswer);
        }

        [Theory]
        [InlineData("xyxy", true)]
        [InlineData("aabcdefgaa", true)]
        [InlineData("bcdefgaaaa", true)]
        [InlineData("aaa", false)]
        public void calculate_contains_same_pair_of_letters_twice(string input, bool expectedAnswer)
        {
            var output = AOC_2015_Day05_Answer.CalculateContainsSamePairOfLettersTwice(input);
            output.Should().Be(expectedAnswer);
        }

        [Theory]
        [InlineData("xyx", true)]
        [InlineData("abcdefeghi", true)]
        [InlineData("aaa", true)]
        [InlineData("aab", false)]
        [InlineData("abcdefgihi", true)]
        [InlineData("abacdefgihj", true)]

        public void calculate_contains_the_same_letter_separated_by_a_single_letter(string input, bool expectedAnswer)
        {
            var output = AOC_2015_Day05_Answer.CalculateContainsSameLetterSeparatedByASingleLetter(input);
            output.Should().Be(expectedAnswer);
        }

        [Fact]
        public void AOC_2015_Day05b()
        {
            var input = File.ReadAllText(".//Content//Day05.txt")
                .Split(Environment.NewLine).ToArray();

            bool HasPair(string str) =>
                Regex.IsMatch(str, @"(\w{2}).*\1");

            bool HasDuplicate(string str) =>
                Regex.IsMatch(str, @"(\w).\1");

            bool IsNiceString(string str) =>
                HasDuplicate(str)
                && HasPair(str);

            var niceStrings = input.Where(IsNiceString);


            var myNiceStrings = input.Where(x => AOC_2015_Day05_Answer.CalculateIsNiceNewRules(x));
            var difference = myNiceStrings.Except(niceStrings);

            var output = myNiceStrings.Count();
            output.Should().Be(69);
        }
    }

}
