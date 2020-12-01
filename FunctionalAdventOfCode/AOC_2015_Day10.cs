using Common;
using FluentAssertions;
using System.Linq;
using System.Text;
using Xunit;

namespace FunctionalAdventOfCode
{
    public static class AOC_2015_Day10_Answer
    {
        public static string LookAndSay(string input) =>
            input.AggregateAdjacent((CurrChar: input.First(), Count: 1, Progress: new StringBuilder()),
                (acc, prev, curr) =>
                    prev == curr
                        ? (acc.CurrChar, acc.Count + 1, acc.Progress)
                        : (curr, 1, acc.Progress.Append(acc.Count).Append(acc.CurrChar))
                )
                .Map(x =>
                    x.Progress.Append(x.Count).Append(x.CurrChar)
                ).ToString();

        public static string LookAndSay2(string input)
        {
            var sb = new StringBuilder();
            var cnt = 1;
            for(var i = 1; i < input.Length ; i++)
            {
                if (input[i] == input[i - 1])
                {
                    cnt++;
                } 
                else
                {
                    sb.Append(cnt).Append(input[i-1]);
                    cnt = 1;
                }
            }
            sb.Append(cnt).Append(input.Last());
            return sb.ToString();
        }

        public static string LookAndSayXTimes(string input, int times, int i = 0) =>
           input.ApplyXTimes(times, x => LookAndSay2(x));
    }

    public class AOC_2015_Day10
    {
        [Theory]
        [InlineData("1", "11")]
        [InlineData("11", "21")]
        [InlineData("21", "1211")]
        [InlineData("1211", "111221")]
        [InlineData("111221", "312211")]
        public void look_and_say(string input, string expectedOutput)
        {
            var actualAnswer = AOC_2015_Day10_Answer.LookAndSay2(input);
            actualAnswer.Should().Be(expectedOutput);
        }

        [Fact]
        public void AOC_2015_Day10a()
        {
            var answer = AOC_2015_Day10_Answer.LookAndSayXTimes("1321131112", 40);
            answer.Length.Should().Be(492982);
        }

        [Fact]
        public void AOC_2015_Day10b()
        {
            var answer = AOC_2015_Day10_Answer.LookAndSayXTimes("1321131112", 50);
            answer.Length.Should().Be(6989950);
        }
    }
}
