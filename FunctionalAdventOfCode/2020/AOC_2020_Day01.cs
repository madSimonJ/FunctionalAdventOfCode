using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode
{
    public static class AOC_2020_Day01_Answer
    {
        public static int GetAuditChecksum(IEnumerable<int> input) =>
            input.ToHashSet()
                .Map(x => x.First(y => x.Contains(2020 - y)))
                .Map(x => x * (2020 - x));

        public static int GetAuditChecksum3(IEnumerable<int> input)
        {
            var hashSet = input.ToHashSet();
            var numbersBelow1000 = input.Where(x => x < 1000).ToArray();
            var allPossibleCombos = numbersBelow1000.SelectMany(x => numbersBelow1000.Select(y => (x, y)));
            var answer = allPossibleCombos.First(x => hashSet.Contains(2020 - x.x - x.y));
            var returnValue = answer.x * answer.y * (2020 - answer.x - answer.y);
            return returnValue;

        }
    }

    public class AOC_2020_Day01
    {
        [Fact]
        public void Day01a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day01.txt")
                .Split(Environment.NewLine)
                .Select(x => x.ToInt());
            var answer = AOC_2020_Day01_Answer.GetAuditChecksum(input);
            answer.Should().Be(913824);
        }

        [Fact]
        public void Day01b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day01.txt")
                .Split(Environment.NewLine)
                .Select(x => x.ToInt());
            var answer = AOC_2020_Day01_Answer.GetAuditChecksum3(input);
            answer.Should().Be(240889536);
        }
    }
}
