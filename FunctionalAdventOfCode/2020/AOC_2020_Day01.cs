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
            answer.Should().Be(0);
        }
    }
}
