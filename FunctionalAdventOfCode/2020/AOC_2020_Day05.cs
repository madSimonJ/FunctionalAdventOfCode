using Common;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{
    public static class AOC_2020_Day05_Answer
    {
        public static (int, int) CalculateSeatReference(string input) =>
            (
            input.Take(7).Aggregate(
                    Enumerable.Range(0, 128),
                    (acc, x) => x switch
                    {
                        'F' => acc.TakeLowerHalf(),
                        'B' => acc.TakeUpperHalf(),
                    }).First(),
            input.Skip(7).Aggregate(
                    Enumerable.Range(0, 8),
                    (acc, x) => x switch
                    {
                        'R' => acc.TakeUpperHalf(),
                        'L' => acc.TakeLowerHalf()
                    }
                ).First()
            );

        public static int CalculateSeatNumber(string input) =>
            CalculateSeatReference(input)
                .Map(x => (x.Item1 * 8) + x.Item2);



        public static int CalculateHighestSeatNumber(string input) =>
            input.Split(Environment.NewLine)
                .Max(
                    x => CalculateSeatNumber(x)
                );
    }



    public class AOC_2020_Day05
    {
        [Theory]
        [InlineData("FBFBBFFRLR", 357)]
        [InlineData("BFFFBBFRRR", 567)]
        [InlineData("FFFBBBFRRR", 119)]
        [InlineData("BBFFBBFRLL", 820)]
        public void calculate_seat_number(string input, int expectedAnswer)
        {
            var answer = AOC_2020_Day05_Answer.CalculateSeatNumber(input);
            answer.Should().Be(expectedAnswer);
        }

        [Fact]
        public void AOC_2020_day05a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day05.txt");
            var answer = AOC_2020_Day05_Answer.CalculateHighestSeatNumber(input);
            answer.Should().Be(0);
        }

        [Fact]
        public void AOC_2020_day05b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day05.txt").Split(Environment.NewLine);
            var answer = input.Select(x => AOC_2020_Day05_Answer.CalculateSeatReference(x));
            var rowWithMissingSeat = answer.GroupBy(x => x.Item1).Where(x => x.Count() == 7).First();
            var seatsInMissingRow = rowWithMissingSeat.Select(x => x.Item2);
            var missingSeat = Enumerable.Range(0, 8).Except(seatsInMissingRow).First();

            var seatId = (rowWithMissingSeat.Key * 8) + missingSeat;
            seatId.Should().Be(743);
        }
    }
}
