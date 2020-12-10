using FluentAssertions;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{
    public static class AOC_2020_Day06_Answer
    {
        public static int CalculateSumOfCounts(string input) =>
            input.Split("\r\n\r\n")
                .Sum(x => x.Replace("\r\n", string.Empty).Distinct().Count());

        public static int CalculateAnswersGivenByEveryone(string input) =>
            input.Split("\r\n\r\n").Sum(x =>
            {
                var distinctValues = x.Replace("\r\n", string.Empty).Distinct();
                var people = x.Split("\r\n");
                var answersGiven = people.SelectMany(x => x.Distinct()).ToArray();
                var countForEachAnswer = distinctValues.Select(x => answersGiven.Count(y => y == x));
                var answersEveryoneGave = countForEachAnswer.Count(x => x == people.Length);
                return answersEveryoneGave;
            });
    }

    public class AOC_2020_Day06
    {
        [Fact]
        public void count_total_from_each_group()
        {
            var input = @"abc

a
b
c

ab
ac

a
a
a
a

b";
            var answer = AOC_2020_Day06_Answer.CalculateSumOfCounts(input);
            answer.Should().Be(11);
        }

        [Fact]
        public void AOC_2020_Day06a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day06.txt");
            var answer = AOC_2020_Day06_Answer.CalculateSumOfCounts(input);
            answer.Should().Be(6297);
        }

        [Fact]
        public void count_questions_answered_by_everyone()
        {
            var input = @"abc

a
b
c

ab
ac

a
a
a
a

b";
            var answer = AOC_2020_Day06_Answer.CalculateAnswersGivenByEveryone(input);
            answer.Should().Be(6);
        }

        [Fact]
        public void AOC_2020_Day06b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day06.txt");
            var answer = AOC_2020_Day06_Answer.CalculateAnswersGivenByEveryone(input);
            answer.Should().Be(3158);
        }
    }
}
