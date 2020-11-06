using Common;
using FluentAssertions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace FunctionalAdventOfCode
{
    public static class AOC_2015_Day04_Answer
    {
        public static string CalculateM5Hash(string input)
        {
            var md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(Encoding.ASCII.GetBytes(input));

            //get hash result after compute it  
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        internal static object CalculateIsNice(string input)
        {
            throw new System.NotImplementedException();
        }

        public static int CalculateLowestMD5HashNumber(string input, int numberOfZeroes = 5) 
        {
            var i = 0;
            var hashStartToFind = Enumerable.Repeat(0, numberOfZeroes).Join();
            var format = Enumerable.Repeat('0', numberOfZeroes).Join();
            while(true)
            {
                var calculatedHash = CalculateM5Hash($"{input}{i.ToString(format)}");
                if (calculatedHash.Substring(0, numberOfZeroes) == hashStartToFind)
                    return i;
                i++;
            }
        } 

}
public class AOC_2015_Day04
{
    [Theory]
    [InlineData("abcdef", 609043)]
    [InlineData("pqrstuv", 1048970)]
    public void input_to_lowest_md5_number(string input, int expectedAnswer)
    {
        var output = AOC_2015_Day04_Answer.CalculateLowestMD5HashNumber(input);
        output.Should().Be(expectedAnswer);
    }


    [Fact]
    public void AOC_2015_Day04a()
    {
        var output = AOC_2015_Day04_Answer.CalculateLowestMD5HashNumber("ckczppom");
        output.Should().Be(117946);
    }

        [Fact]
        public void AOC_2015_Day04b()
        {
            var output = AOC_2015_Day04_Answer.CalculateLowestMD5HashNumber("ckczppom", 6);
            output.Should().Be(3938038);
        }
    }
}
