using Common;
using FluentAssertions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace FunctionalAdventOfCode
{
    public static class AOC_2015_Day11_Answer
    {
        public static string IncrementString(string input)
        {
            var sb = new StringBuilder();
            var stringWithZTailRemoved = Regex.Replace(input, "z*$", string.Empty);
            sb.Append(stringWithZTailRemoved[..^1]);
            sb.Append((char)(stringWithZTailRemoved.Last() + 1));
            sb.Append(new string(Enumerable.Repeat('a', 8 - sb.Length).ToArray()));

            return sb.ToString();
        }
    }

    public class AOC_2015_Day11
    {
        [Theory]
        [InlineData("aaaaaaxx", "aaaaaaxy")]
        [InlineData("aaaaaaxy", "aaaaaaxz")]
        [InlineData("aaaaaaxz", "aaaaaaya")]
        [InlineData("aaaaaaya", "aaaaaayb")]
        public void increment_string(string input, string expectedOutput)
        {
            var answer = AOC_2015_Day11_Answer.IncrementString(input);
            answer.Should().Be(expectedOutput);
        }

    }
}
