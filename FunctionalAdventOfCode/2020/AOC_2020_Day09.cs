using Common;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{

    public static class AOC_2020_Day09_Answer
    {
        public static int GetFirstInvalidValue(IEnumerable<int> input, int preambleLength) =>
            Enumerable.Range(0, input.Count() - preambleLength - 1)
                .Select(x => input.Skip(x).Take(preambleLength + 1))
                .Select(x => (
                    Values: x.Take(x.Count() - 1).ToHashSet(),
                    ValueToCheck: x.Last()
                ))
                .First(x => 
                    !x.Values.Any(y => x.Values.Contains(x.ValueToCheck - y)  )
                ).ValueToCheck;




        public static double GetRange(IEnumerable<double> input, double valueToFind) =>
            Enumerable.Range(2, 20)
                .SelectMany(SizeOfInterval =>
                    Enumerable.Range(0, input.Count() - SizeOfInterval)
                        .Select(startOfSelection => input.Skip(startOfSelection).Take(SizeOfInterval))
                ).First(x => {

                    return x.Sum() == valueToFind && x.CountAdjacent((prev, curr) => curr > prev) >= 2;


                })
                .Map(x => x.Min() + x.Max());


    }

    public class AOC_2020_Day09
    {
        [Fact]
        public void find_first_invalid_character()
        {
            var input = Enumerable.Range(1, 25).Concat(new[]
            {
                26,
                49,
                100,
                50
            });

            var answer = AOC_2020_Day09_Answer.GetFirstInvalidValue(input, 25);
            answer.Should().Be(100);
        }

        [Fact]
        public void find_first_invalid_character_2()
        {
            var input = new[]
            {
                35 ,
                20 ,
                15 ,
                25 ,
                47 ,
                40 ,
                62 ,
                55 ,
                65 ,
                95 ,
                102,
                117,
                150,
                182,
                127,
                219,
                299,
                277,
                309,
                576
            };

            var answer = AOC_2020_Day09_Answer.GetFirstInvalidValue(input, 5);
            answer.Should().Be(127);
        }

        [Fact]
        public void AOC_2020_Day09a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day09.txt");

            var parsedInput = input.Split("\r\n").Select(x => x.ToInt());
            var answer = AOC_2020_Day09_Answer.GetFirstInvalidValue(parsedInput, 25);
            answer.Should().Be(104054607);
        }

        [Fact]
        public void AOC_2020_Day09b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day09.txt");

            var parsedInput = input.Split("\r\n").Select(x => x.ToDouble());
            var answer = AOC_2020_Day09_Answer.GetRange(parsedInput, 104054607D);
            answer.Should().Be(13935797);
        }

        [Theory]
        [InlineData(new[] { 15, 25, 47, 40 }, true)]
        public void Are_contiguous(int[] input, bool expectedAnswer)
        {
            input.AreContiguous().Should().BeFalse();
        }

        [Fact]
        public void find_real_sum()
        {
            var input = new[]
            {
                35D ,
                20D ,
                15D ,
                25D ,
                47D ,
                40D ,
                62D ,
                55D ,
                65D ,
                95D ,
                102D,
                117D,
                150D,
                182D,
                127D,
                219D,
                299D,
                277D,
                309D,
                576D
            };

            var answer = AOC_2020_Day09_Answer.GetRange(input, 127);
            answer.Should().Be(62);
        }

    }
}
