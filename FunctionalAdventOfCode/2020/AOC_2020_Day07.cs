using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{
    public class ContainedBag
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public static class AOC_2020_Day07_Answer
    {
        private static IEnumerable<ContainedBag> ParseContainingBag(string input) =>
            input.StartsWith("no ")
                ? Enumerable.Empty<ContainedBag>()
                : input.Split(", ")
                        .Select(x => x.Split(" "))
                        .Select(x => new ContainedBag
                        {
                            Quantity = x[0].ToInt(),
                            Description = $"{x[1]} {x[2]}"
                        });


        public static IDictionary<string, IEnumerable<ContainedBag>> ParseBags(string input) =>
            input.Split("\r\n")
                .Select(x => x.Split(" bags contain ").ToArray())
                .Select(x => (
                        x[0],
                        ParseContainingBag(x[1])
                ))
                .ToDictionary(x => x.Item1, x => x.Item2);

        public static int CaulculateNumberOfBags(string input) =>
            ParseBags(input)
            .Map(x =>
            {

                var starterBag = x["shiny gold"];
                var total = numberOfAllBags(starterBag);
                return total - 1;

                int numberOfAllBags(IEnumerable<ContainedBag> bags) =>
                    bags.Any()
                        ? 1 + bags.Sum(y => y.Quantity * numberOfAllBags(x[y.Description]))
                        : 1;

            });


        public static int CalculateContainingBags(string input) =>
            ParseBags(input)
                .Map(x =>
                {
                    var startSet = x.Where(y => y.Value.Any(z => z.Description == "shiny gold")).Select(y => y.Key).ToArray();
                    var startRemainder = x.Where(y => !startSet.Contains(y.Key) && y.Value.Any()).ToDictionary(x => x.Key, x => x.Value);
                    var finalSet = GetMoreBags(startSet, startRemainder);
                    return finalSet.Count();

                    IEnumerable<string> GetMoreBags(IEnumerable<string> set, IDictionary<string, IEnumerable<ContainedBag>> remainder)
                    {
                        var moreBags = remainder.Where(y => set.Intersect(y.Value.Select(z => z.Description)).Any()).Select(y => y.Key).ToArray();
                        return moreBags.Any()
                            ? GetMoreBags(
                                set.Concat(moreBags),
                                remainder.Where(y => !moreBags.Contains(y.Key)).ToDictionary(x => x.Key, x => x.Value)
                            )
                            : set;
                    };
                });


    }

    public class AOC_2020_Day07
    {
        [Fact]
        public void parse_bags()
        {
            var input = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";
            var answer = AOC_2020_Day07_Answer.ParseBags(input);
            answer.Should().BeEquivalentTo(
                new Dictionary<string, IEnumerable<ContainedBag>>
                {
                    {
                        "light red",  new[] {
                            new ContainedBag
                            {
                                Description = "bright white",
                                Quantity = 1
                            },
                            new ContainedBag
                            {
                                Description = "muted yellow",
                                Quantity = 2
                            }
                        }
                    },
                    {
                        "dark orange", new [] {
                            new ContainedBag
                            {
                                Description = "bright white",
                                Quantity = 3
                            },
                            new ContainedBag
                            {
                                Description = "muted yellow",
                                Quantity = 4
                            }
                        }
                    },
                    {
                        "bright white",
                        new[] {
                            new ContainedBag
                            {
                                Description = "shiny gold",
                                Quantity = 1
                           }
                        }
                    },
                    {
                        "muted yellow",
                        new[] {
                            new ContainedBag
                            {
                                Description = "shiny gold",
                                Quantity = 2
                            },
                            new ContainedBag
                            {
                                Description = "faded blue",
                                Quantity = 9
                            }
                        }
                    },
                    {
                        "shiny gold",
                        new[] {
                            new ContainedBag
                            {
                                Description = "dark olive",
                                Quantity = 1
                            },
                            new ContainedBag
                            {
                                Description = "vibrant plum",
                                Quantity = 2
                            }
                        }
                    },
                    {
                        "dark olive",
                        new[] {
                            new ContainedBag
                            {
                                Description = "faded blue",
                                Quantity = 3
                            },
                            new ContainedBag
                            {
                                Description = "dotted black",
                                Quantity = 4
                            }
                        }
                    },
                    {
                        "vibrant plum",
                        new[] {
                            new ContainedBag
                            {
                                Description = "faded blue",
                                Quantity = 5
                            },
                            new ContainedBag
                            {
                                Description = "dotted black",
                                Quantity = 6
                            }
                        }
                    },
                    {
                        "faded blue",
                        Enumerable.Empty<ContainedBag>()
                    },
                    {
                        "dotted black",
                        Enumerable.Empty<ContainedBag>()
                    }
            });
        }

        [Fact]
        public void calculate_the_number_of_bags_that_could_contain_a_shiny_gold_bag()
        {
            var input = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";
            var answer = AOC_2020_Day07_Answer.CalculateContainingBags(input);
            answer.Should().Be(4);
        }

        [Fact]
        public void AOC_2020_Day07a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day07.txt");
            var answer = AOC_2020_Day07_Answer.CalculateContainingBags(input);
            answer.Should().Be(355);
        }

        [Fact]
        public void calculate_the_number_of_bags()
        {
            var input = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";
            var answer = AOC_2020_Day07_Answer.CaulculateNumberOfBags(input);
            answer.Should().Be(32);
        }

        [Fact]
        public void calculate_the_number_of_bags_2()
        {
            var input = @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";
            var answer = AOC_2020_Day07_Answer.CaulculateNumberOfBags(input);
            answer.Should().Be(126);
        }

        [Fact]
        public void AOC_2020_Day07b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day07.txt");
            var answer = AOC_2020_Day07_Answer.CaulculateNumberOfBags(input);
            answer.Should().Be(355);
        }
    }
}