using Common;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using FluentAssertions;
using System.IO;
using System.Collections.Generic;

namespace FunctionalAdventOfCode._2020
{
    public static class AOC_2020_Day04_Answer
    {

        private static string[] fields = new[]
        {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid",
            "cid"
        };

        private static string[] ValidEyeColours = new[]
        {
            "amb",
            "blu",
            "brn",
            "gry",
            "grn",
            "hzl",
            "oth"
        };

        public static int CountCompleteDatasets(string input) =>
            input.Split(new[] { "\r\n\r\n" }, System.StringSplitOptions.RemoveEmptyEntries)
                    .Count(x => Regex.Matches(x, "...:").Select(y => y.Value[..^1])
                                            .Map(y => RequiredFieldsArePresent(y))
                    );


        private static bool RequiredFieldsArePresent(IEnumerable<string> listOfFieldNames) =>
            fields.Except(listOfFieldNames)
                    .Map(x => !x.Any() || x.Count() == 1 && x.First() == "cid");

        public static bool BirthYearValid(string input) =>
            input.ToInt().InRange(1920, 2002);

        public static bool HeightValid(string input) =>
            input.Map(x => (
                Value: x[..^2].ToInt(),
                Demonination: x[^2..]
            ))
            .Map(x =>
                x.Demonination switch
                {
                    "cm" => x.Value.InRange(150, 193),
                    "in" => x.Value.InRange(59, 76),
                    _ => false
                });

        public static int CountCompleteValidDatasets(string input) =>
            input.Split(new[] { "\r\n\r\n" }, System.StringSplitOptions.RemoveEmptyEntries)
                    .Count(x =>
                    {
                        var dataEntries = x.Replace("\r\n", " ").Split(' ')
                                        .Select(y => y.Split(':'))
                                        .Select(y => (Key: y.First(), Value: y.Last()));
                        var validAndCorrect = RequiredFieldsArePresent(dataEntries.Select(x => x.Key)) &&
                            dataEntries.All(y => y.Key switch
                            {
                                "byr" => BirthYearValid(y.Value),
                                "iyr" => y.Value.ToInt().InRange(2010, 2020),
                                "eyr" => y.Value.ToInt().InRange(2020, 2030),
                                "hgt" => HeightValid(y.Value),
                                "ecl" => ValidEyeColours.Contains(y.Value),
                                "pid" => y.Value.Length == 9 && int.TryParse(y.Value, out _),
                                "hcl" => Regex.Match(y.Value, "#[a-f0-9]{6}").Success,
                                "cid" => true,
                                _ => false
                            });
                        return validAndCorrect;
                        }
                    );
    }

    public class AOC_2020_Day04
    {
        [Fact]
        public void count_how_many_have_all_required_fields_completed()
        {
            var input = @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr: 1937 iyr: 2017 cid: 147 hgt: 183cm

    iyr:2013 ecl: amb cid:350 eyr: 2023 pid: 028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr: 2024
ecl: brn pid:760753108 byr: 1931
hgt: 179cm

 hcl:#cfa07d eyr:2025 pid:166559648
iyr: 2011 ecl: brn hgt:59in";
            var answer = AOC_2020_Day04_Answer.CountCompleteDatasets(input);
            answer.Should().Be(2);

        }

        [Fact]
        public void AOC_2020_day04a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day04.txt");
            var answer = AOC_2020_Day04_Answer.CountCompleteDatasets(input);
            answer.Should().Be(245);
        }

        [Fact]
        public void count_how_many_are_complete_and_valid()
        {
            var input = @"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007";
            var answer = AOC_2020_Day04_Answer.CountCompleteValidDatasets(input);
            answer.Should().Be(0);
        }

        [Fact]
        public void count_how_many_are_complete_and_valid_2()
        {
            var input = @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
            var answer = AOC_2020_Day04_Answer.CountCompleteValidDatasets(input);
            answer.Should().Be(4);
        }

        [Fact]
        public void AOC_2020_day04b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day04.txt");
            var answer = AOC_2020_Day04_Answer.CountCompleteValidDatasets(input);
            answer.Should().BeLessThan(133);
        }

        [Theory]
        [InlineData("2002", true)]
        [InlineData("2003", false)]
        public void test_birth_year(string year, bool expectedResult)
        {
            AOC_2020_Day04_Answer.BirthYearValid(year).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("60in", true)]
        [InlineData("190cm", true)]
        [InlineData("190in", false)]
        [InlineData("190", false)]

        public void test_height(string height, bool expectedResult)
        {
            AOC_2020_Day04_Answer.HeightValid(height).Should().Be(expectedResult);
        }
    }
}
