using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{
    public class PasswordRule
    {
        public int MinNum { get; set; }
        public int MaxNum { get; set; }
        public char CharToCheck{ get; set; }
        public string Password { get; set; }
    }

    public static class AOC_2020_Day02_Answer
    {
        public static IEnumerable<PasswordRule> Parse(IEnumerable<string> input) =>
            input.Select(x => x.Split(" ").ToArray())
                .Select(x => new PasswordRule
                {
                    MinNum = x[0].Split("-")[0].ToInt(),
                    MaxNum = x[0].Split("-")[1].ToInt(),
                    CharToCheck = x[1][0],
                    Password = x[2]
                });

        public static int CountRulesThatPass(IEnumerable<PasswordRule> input) =>
            input.Count(x => x.Password.Count(y => y == x.CharToCheck).InRange(x.MinNum, x.MaxNum));

        public static int CountRulesThatPassv2(IEnumerable<PasswordRule> input) =>
            input.Count(x => (  x.Password.GetCharOrDefault(x.MinNum-1) == x.CharToCheck ^ 
                                x.Password.GetCharOrDefault(x.MaxNum-1) == x.CharToCheck
                    ));
    }

    public class AOC_2020_Day02
    {
        [Fact]
        public void parse_input_line()
        {
            var input = new[]
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };

            var output = AOC_2020_Day02_Answer.Parse(input);
            output.Should().BeEquivalentTo(
                new PasswordRule
                {
                    MinNum = 1,
                    MaxNum = 3,
                    CharToCheck = 'a',
                    Password = "abcde"
                },
                new PasswordRule
                {
                    MinNum = 1,
                    MaxNum = 3,
                    CharToCheck = 'b',
                    Password = "cdefg"
                },
                new PasswordRule
                {
                    MinNum = 2,
                    MaxNum = 9,
                    CharToCheck = 'c',
                    Password = "ccccccccc"
                }
            );
        }

        [Fact]
        public void count_rules_that_pass()
        {
            var input = new[]
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };

            var output = AOC_2020_Day02_Answer.Parse(input);
            var answer = AOC_2020_Day02_Answer.CountRulesThatPass(output);
            answer.Should().Be(2);
        }

        [Fact]
        public void AOC_2020_Day02a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day02.txt")
                            .Split(Environment.NewLine);
            var output = AOC_2020_Day02_Answer.Parse(input);
            var answer = AOC_2020_Day02_Answer.CountRulesThatPass(output);
            answer.Should().Be(643);
        }

        [Fact]
        public void count_rules_that_pass_v2()
        {
            var input = new[]
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };

            var output = AOC_2020_Day02_Answer.Parse(input);
            var answer = AOC_2020_Day02_Answer.CountRulesThatPassv2(output);
            answer.Should().Be(1);
        }

        [Fact]
        public void AOC_2020_Day02b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day02.txt")
                            .Split(Environment.NewLine);
            var output = AOC_2020_Day02_Answer.Parse(input);
            var answer = AOC_2020_Day02_Answer.CountRulesThatPassv2(output);
            answer.Should().Be(388);
        }
    }
}
