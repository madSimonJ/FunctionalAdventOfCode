using Common;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode
{
    public class Dimensions
    {
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public static class AOC_2015_Day02_Answer
    {
        public static Dimensions MakeDimension(string input) =>
            input.Split("x").ToArray()
            .Map(x => new Dimensions
            {
                Width = x[0].ToInt(),
                Length = x[1].ToInt(),
                Height = x[2].ToInt()
            });

        public static int CalculateRequiredWrappingPaper(Dimensions d) =>
            new[]
            {
                d.Length * d.Width,
                d.Width * d.Height,
                d.Height * d.Length
            }.Map(x => x.Sum(y => y * 2) + x.Min());

        public static int CalculateRequiredRibbon(Dimensions d) =>
            new[]
            {
                d.Length + d.Width,
                d.Width + d.Height,
                d.Height + d.Length
            }.OrderBy(x => x).First().Map(x => x * 2);

        public static int CalculateRequiredBow(Dimensions d) =>
            d.Height * d.Length * d.Width;
    }

    public class AOC_2015_Day02
    {
        [Theory]
        [InlineData("2x3x4", 58)]
        [InlineData("1x1x10", 43)]
        public void string_to_dimension_conversion(string input, int expectedAnswer)
        {
            var d = AOC_2015_Day02_Answer.MakeDimension(input);
            var output = AOC_2015_Day02_Answer.CalculateRequiredWrappingPaper(d);
            output.Should().Be(expectedAnswer);
        }


        [Fact]
        public void AOC_2015_Day02a()
        {
            var input = File.ReadAllText(".//Content//Day02.txt");
            var output = input.Split(Environment.NewLine)
                                .Select(x => AOC_2015_Day02_Answer.MakeDimension(x))
                                .Sum(x => AOC_2015_Day02_Answer.CalculateRequiredWrappingPaper(x));

            output.Should().Be(1598415);
        }

        [Theory]
        [InlineData("2x3x4", 10)]
        [InlineData("1x1x10", 4)]
        public void calculate_ribbon_length(string input, int expectedAnswer)
        {
            var d = AOC_2015_Day02_Answer.MakeDimension(input);
            var output = AOC_2015_Day02_Answer.CalculateRequiredRibbon(d);
            output.Should().Be(expectedAnswer);
        }

        [Theory]
        [InlineData("2x3x4", 24)]
        [InlineData("1x1x10", 10)]
        public void calculate_bow_length(string input, int expectedAnswer)
        {
            var d = AOC_2015_Day02_Answer.MakeDimension(input);
            var output = AOC_2015_Day02_Answer.CalculateRequiredBow(d);
            output.Should().Be(expectedAnswer);
        }

        [Fact]
        public void AOC_2015_Day02b()
        {
            var input = File.ReadAllText(".//Content//Day02.txt");
            var output = input.Split(Environment.NewLine)
                                .Select(x => AOC_2015_Day02_Answer.MakeDimension(x))
                                .Sum(x => AOC_2015_Day02_Answer.CalculateRequiredBow(x) +
                                AOC_2015_Day02_Answer.CalculateRequiredRibbon(x));

            output.Should().Be(3812909);
        }
    }
}
