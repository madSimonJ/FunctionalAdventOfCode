using Common;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace FunctionalAdventOfCode
{
    public static class AOC_2015_Day08Answer
    {
        private static string ReplaceHexCharacters(this string input)
        {
            return Regex.Replace(input, @"\\x[0-9a-fA-F][0-9a-fA-F]", x =>
            {
                return x.Value[2..].ToUInt().ToChar().ToString();
            });
        }

        public static int CalculateCharsInString(string input) =>
            input[1..^1]
                .Replace(@"\""",@"""")
                .Replace(@"\\", @"\")
                .ReplaceHexCharacters()
                .Length;

        public static int CalculateAnswer(string input) =>
            input.Split(Environment.NewLine)
                .Sum(x => x.Length - CalculateCharsInString(x));

        public static string ReencodeString(string input) =>
            input
                .Replace(@"\", @"\\")
                .Replace(@"""", @"\""")
                .Map(x => $"\"{x}\"");

        internal static object CalculateSecondAnswer(string input) =>
            input.Split(Environment.NewLine)
                .Sum(x => ReencodeString(x).Length - x.Length);
    }

    public class AOC_2015_Day08
    {
        [Theory]
        [InlineData(@"""""", 0)]
        [InlineData(@"""abc""", 3)]
        [InlineData(@"""aaa\""aaa""", 7)]
        [InlineData(@"""\x27""", 1)]
        public void calculate_correct_string_lengths(string input, int expectedAnswer)
        {
            var actualAnswer = AOC_2015_Day08Answer.CalculateCharsInString(input);
            actualAnswer.Should().Be(expectedAnswer);
        }

        [Fact]
        public void calculate_test_final_answer()
        {
            var input = @$"""""{Environment.NewLine}""abc""{Environment.NewLine}""aaa\""aaa""{Environment.NewLine}""\x27""";
            var answer = AOC_2015_Day08Answer.CalculateAnswer(input);
            answer.Should().Be(12);
        }

        [Fact]
        public void AOC_2015_Day08a()
        {
            var input = File.ReadAllText(".//Content2//Day08.txt");
            var answer = AOC_2015_Day08Answer.CalculateAnswer(input);
            answer.Should().Be(1350);
        }

        [Theory]
        [InlineData(@"""""", @"""\""\""""")]
        [InlineData(@"""abc""", @"""\""abc\""""")]
        [InlineData(@"""aaa\""aaa""", @"""\""aaa\\\""aaa\""""")]
        [InlineData(@"""\x27""", @"""\""\\x27\""""")]
        public void encode_as_new_string(string input, string expectedOutput)
        {
            var answer = AOC_2015_Day08Answer.ReencodeString(input);
            answer.Should().Be(expectedOutput);
        }

        [Fact]
        public void calculate_test_second_answer()
        {
            var input = @$"""""{Environment.NewLine}""abc""{Environment.NewLine}""aaa\""aaa""{Environment.NewLine}""\x27""";
            var answer = AOC_2015_Day08Answer.CalculateSecondAnswer(input);
            answer.Should().Be(19);
        }

        [Fact]
        public void AOC_2015_Day08b()
        {
            var input = File.ReadAllText(".//Content2//Day08.txt");
            var answer = AOC_2015_Day08Answer.CalculateSecondAnswer(input);
            answer.Should().Be(2085);
        }
    }
}
