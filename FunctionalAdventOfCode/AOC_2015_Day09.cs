using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode
{
    public class CityConnection
    {
        public IEnumerable<string> Nodes { get; set; }
        public int Distance { get; set; }
    }

    public static class AOC_2015_Day09_Answer
    {
        public static IEnumerable<CityConnection> ParseConnections(string input) =>
            input.Split(Environment.NewLine)
            .Select(x => x.Split(" "))
            .Select(x => new CityConnection
            {
                Nodes = new[] { x[0], x[2] },
                Distance = x[4].ToInt()
            });

        public static int CalculateShortestRoute(IEnumerable<CityConnection> connections) =>
            connections.SelectMany(x => x.Nodes).Distinct()
                    .PermutateRoutes()
                    .Min(x => x.AggregateAdjacent(
                            0,
                            (acc, prev, curr) => acc + connections.FirstOrDefault(x => x.Nodes.Contains(prev) && x.Nodes.Contains(curr)).Distance
                        ));

        public static int CalculateLongestRoute(IEnumerable<CityConnection> connections) =>
            connections.SelectMany(x => x.Nodes).Distinct()
                    .PermutateRoutes()
                    .Max(x => x.AggregateAdjacent(
                            0,
                            (acc, prev, curr) => acc + connections.FirstOrDefault(x => x.Nodes.Contains(prev) && x.Nodes.Contains(curr)).Distance
                        ));


    }

    public class AOC_2015_Day09
    {

        [Fact]
        public void parse_instructions()
        {
            var input = $"London to Dublin = 464{Environment.NewLine}London to Belfast = 518{Environment.NewLine}Dublin to Belfast = 141";
            var output = AOC_2015_Day09_Answer.ParseConnections(input);
            output.Should().BeEquivalentTo(
                    new[]
                    {
                        new CityConnection
                        {
                            Distance = 464,
                            Nodes = new [] {"London", "Dublin"}
                        },
                        new CityConnection
                        {
                            Distance = 518,
                            Nodes = new [] {"London", "Belfast"}
                        },
                        new CityConnection
                        {
                            Distance = 141,
                            Nodes = new [] {"Dublin", "Belfast"}
                        }
                    }
                );
        }

        [Fact]
        public void permutate_all_possible_routes_when_3_items()
        {
            var items = new[] { "place1", "place2", "place3" };
            var items2 = items.PermutateRoutes().ToArray();
            items2.Should().BeEquivalentTo(
                    new[] { "place1", "place2", "place3" },
                    new [] {"place1", "place3", "place2"},
                    new [] {"place2", "place3", "place1"},
                    new [] {"place2", "place1", "place3"},
                    new [] {"place3", "place2", "place1"},
                    new [] {"place3", "place1", "place2"}
                );
        }


        [Fact]
        public void calculate_Shortest_route()
        {
            var input = $"London to Dublin = 464{Environment.NewLine}London to Belfast = 518{Environment.NewLine}Dublin to Belfast = 141";
            var parsedConnections = AOC_2015_Day09_Answer.ParseConnections(input).ToArray();
            var minValue = AOC_2015_Day09_Answer.CalculateShortestRoute(parsedConnections);
            minValue.Should().Be(605);
        }

        [Fact]
        public void AOC_2015_Day09a()
        {
            var input = File.ReadAllText(".//Content2//Day09.txt");
            var parsedConnections = AOC_2015_Day09_Answer.ParseConnections(input).ToArray();
            var minValue = AOC_2015_Day09_Answer.CalculateShortestRoute(parsedConnections);
            minValue.Should().Be(141);
        }

        [Fact]
        public void AOC_2015_Day09b()
        {
            var input = File.ReadAllText(".//Content2//Day09.txt");
            var parsedConnections = AOC_2015_Day09_Answer.ParseConnections(input).ToArray();
            var minValue = AOC_2015_Day09_Answer.CalculateLongestRoute(parsedConnections);
            minValue.Should().Be(736);
        }

    }


}
