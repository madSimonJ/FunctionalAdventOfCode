using Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FunctionalAdventOfCode._2020
{
    public record Seat
    {
        public int X { get; init; }
        public int Y { get; init; }
        public char Status { get; set; }
    }

    public static class AOC_2020_Day11_Answer
    {
        public static IEnumerable<Seat> Parse(string input) =>
            input.Split("\r\n")
                .SelectMany((y, j) =>
                    y.Select((x, i) =>
                        new Seat
                        {
                            X = i,
                            Y = j,
                            Status = x
                        }
                    )
                    .Where(x => x.Status != '.')
                );

        public static Seat UpdateSeat(Seat oldSeat, IEnumerable<char> adjacentSeats) =>
            oldSeat.Status switch
            {
                'L' when adjacentSeats.Count(y => y == '#') == 0 => oldSeat with { Status = '#' },
                '#' when adjacentSeats.Count(y => y == '#') >= 4 => oldSeat with { Status = 'L' },
                _ => oldSeat
            };

        public static Seat UpdateSeatWithDirectionSearch(Seat oldSeat, IEnumerable<char> adjacentSeats) =>
            oldSeat.Status switch
            {
                'L' when !adjacentSeats.Any(y => y == '#') => oldSeat with { Status = '#' },
                '#' when adjacentSeats.Count(y => y == '#') >= 5 => oldSeat with { Status = 'L' },
                _ => oldSeat
            };

        public static IEnumerable<Seat> GenerateDelta(IEnumerable<Seat> input) =>
            input.Select(x =>
            {
                var adjacentSeats = input.Where(y => y.X.InRange(x.X - 1, x.X + 1) && y.Y.InRange(x.Y - 1, x.Y + 1) && y != x);
                var returnValue = UpdateSeat(x, adjacentSeats.Select(y => y.Status));
                return returnValue;
            }).Where(x => !input.Contains(x));

        public static char GetFirstSeatNorth(IEnumerable<Seat> seats, int xCoord, int yCoord) =>
            seats.Where(x => x.X == xCoord && x.Y < yCoord).OrderByDescending(x => x.Y).FirstOrDefault()?.Status ?? 'L';

        public static char GetFirstSeatSouth(IEnumerable<Seat> seats, int xCoord, int yCoord) =>
            seats.Where(x => x.X == xCoord && x.Y > yCoord).OrderBy(x => x.Y).FirstOrDefault()?.Status ?? 'L';

        public static char GetFirstSeatEast(IEnumerable<Seat> seats, int xCoord, int yCoord) =>
            seats.Where(x => x.X > xCoord && x.Y == yCoord).OrderBy(x => x.X).FirstOrDefault()?.Status ?? 'L';

        public static char GetFirstSeatWest(IEnumerable<Seat> seats, int xCoord, int yCoord) =>
                    seats.Where(x => x.X < xCoord && x.Y == yCoord).OrderByDescending(x => x.X).FirstOrDefault()?.Status ?? 'L';

        public static char GetFirstNorthEast(IEnumerable<Seat> seats, int xCoord, int yCoord)
        {
            var xLimit = seats.Max(x => x.X);
            var yLimit = 0;

            var diagonalLimit =  Math.Min(Math.Abs(xCoord - xLimit), Math.Abs(yCoord - yLimit));
            var range = Enumerable.Range(1, diagonalLimit);
            var coords = range.Select(x => (X: xCoord + x, Y: yCoord - x));
            var seatsOnDiagonal = seats.Join(coords, x => (x.X, x.Y),
                                                        x => x,
                                                        (x, y) => x).OrderBy(x => x.X);
            var firstSeat = seatsOnDiagonal.FirstOrDefault();
            return firstSeat?.Status ?? 'L';
        }

        public static char GetFirstSouthEast(IEnumerable<Seat> seats, int xCoord, int yCoord)
        {
            var xLimit = seats.Max(x => x.X);
            var yLimit = seats.Max(x => x.Y);

            var diagonalLimit = Math.Min(Math.Abs(xCoord - xLimit), Math.Abs(yCoord - yLimit));
            var range = Enumerable.Range(1, diagonalLimit);
            var coords = range.Select(x => (X: xCoord + x, Y: yCoord + x));
            var seatsOnDiagonal = seats.Join(coords, x => (x.X, x.Y),
                                                        x => x,
                                                        (x, y) => x).OrderBy(x => x.X);
            var firstSeat = seatsOnDiagonal.FirstOrDefault();
            return firstSeat?.Status ?? 'L';
        }

        public static char GetFirstSouthWest(IEnumerable<Seat> seats, int xCoord, int yCoord)
        {
            var xLimit = 0;
            var yLimit = seats.Max(x => x.Y);

            var diagonalLimit = Math.Min(Math.Abs(xCoord - xLimit), Math.Abs(yCoord - yLimit));
            var range = Enumerable.Range(1, diagonalLimit);
            var coords = range.Select(x => (X: xCoord - x, Y: yCoord + x));
            var seatsOnDiagonal = seats.Join(coords, x => (x.X, x.Y),
                                                        x => x,
                                                        (x, y) => x).OrderBy(x => x.X);
            var firstSeat = seatsOnDiagonal.FirstOrDefault();
            return firstSeat?.Status ?? 'L';
        }

        public static char GetFirstNorthhWest(IEnumerable<Seat> seats, int xCoord, int yCoord)
        {
            var diagonalLimit = Math.Min(Math.Abs(xCoord - 0), Math.Abs(yCoord - 0));
            var range = Enumerable.Range(1, diagonalLimit);
            var coords = range.Select(x => (X: xCoord - x, Y: yCoord - x));
            var seatsOnDiagonal = seats.Join(coords, x => (x.X, x.Y),
                                                        x => x,
                                                        (x, y) => x).OrderBy(x => x.X);
            var firstSeat = seatsOnDiagonal.FirstOrDefault();
            return firstSeat?.Status ?? 'L';
        }


        public static IEnumerable<Seat> GenerateDeltaWithDirectionSearch(IEnumerable<Seat> input) =>
            input.Select(x =>
            {
                var adjacentSeats = new[]
                {
                    GetFirstSeatNorth(input, x.X, x.Y),
                    GetFirstNorthEast(input, x.X, x.Y),
                    GetFirstSeatEast(input, x.X, x.Y),
                    GetFirstSouthEast(input, x.X, x.Y),
                    GetFirstSeatSouth(input, x.X, x.Y),
                    GetFirstSouthWest(input, x.X, x.Y),
                    GetFirstSeatWest(input, x.X, x.Y),
                    GetFirstNorthhWest(input, x.X, x.Y)
                }.ToArray();
                var returnValue = UpdateSeatWithDirectionSearch(x, adjacentSeats);
                return returnValue;
            }).Where(x => !input.Contains(x));


        public static IEnumerable<Seat> ApplyDelta(IEnumerable<Seat> prev, IEnumerable<Seat> delta) =>
            prev.Select(x =>
                delta.FirstOrDefault(y => x.X == y.X && x.Y == y.Y)
                    .Map(y =>
                        y ?? x));

        //public static IEnumerable<Seat> GetFinalConfiguration(IEnumerable<Seat> input) =>
        //    GenerateDelta(input).ToArray()
        //        .Map(x =>
        //            x.Any()
        //                ? GetFinalConfiguration(ApplyDelta(input, x)).ToArray()
        //                : input
        //        ).ToArray();

        public static IEnumerable<Seat> GetFinalConfiguration(IEnumerable<Seat> input)
        {
            var delta = GenerateDelta(input).ToArray();
            var newInput = input.ToArray();
            while(delta.Any())
            {
                newInput = ApplyDelta(newInput, delta).ToArray();
                delta = GenerateDelta(newInput).ToArray();
            }
            return newInput;
        }

        public static IEnumerable<Seat> GetFinalConfigurationWhenLookingInEachDirection(IEnumerable<Seat> input)
        {
            var delta = GenerateDeltaWithDirectionSearch(input).ToArray();
            var newInput = input.ToArray();
            while (delta.Any())
            {
                newInput = ApplyDelta(newInput, delta).ToArray();
                delta = GenerateDeltaWithDirectionSearch(newInput).ToArray();
            }
            return newInput;
        }




    }

    public class AOC_2020_Day11
    {

        [Fact]
        public void create_starting_positions()
        {
            var seats = @"L.LL.LL.LL
LLLLLLL.LL";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(seats);
            parsedSeats.Should().BeEquivalentTo(
                    new Seat
                    {
                        X = 0,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 2,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 3,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 5,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 6,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 8,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 9,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 0,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 1,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 2,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 3,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 4,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 5,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 6,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 8,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 9,
                        Y = 1,
                        Status = 'L'
                    }
                );


        }

        [Fact]
        public void generate_delta_1()
        {
            var seats = @"L.LL.LL.LL
LLLLLLL.LL";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(seats);
            var delta = AOC_2020_Day11_Answer.GenerateDelta(parsedSeats);
            delta.Should().BeEquivalentTo(
                    new Seat
                    {
                        X = 0,
                        Y = 0,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 2,
                        Y = 0,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 3,
                        Y = 0,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 5,
                        Y = 0,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 6,
                        Y = 0,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 8,
                        Y = 0,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 9,
                        Y = 0,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 0,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 1,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 2,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 3,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 4,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 5,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 6,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 8,
                        Y = 1,
                        Status = '#'
                    },
                    new Seat
                    {
                        X = 9,
                        Y = 1,
                        Status = '#'
                    }
                );


        }


        [Fact]
        public void generate_delta_2()
        {
            var seats = @"#.##.##.##
#######.##";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(seats);
            var delta = AOC_2020_Day11_Answer.GenerateDelta(parsedSeats);
            delta.Should().BeEquivalentTo(
                    new Seat
                    {
                        X = 2,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 3,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 5,
                        Y = 0,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 1,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 2,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 3,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 4,
                        Y = 1,
                        Status = 'L'
                    },
                    new Seat
                    {
                        X = 5,
                        Y = 1,
                        Status = 'L'
                    }
                );
        }

        [Fact]
        public void calculate_final_configuration()
        {
            var input = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(input);
            var answer = AOC_2020_Day11_Answer.GetFinalConfiguration(parsedSeats);
            var finalAnswer = answer.Count(x => x.Status == '#');
            finalAnswer.Should().Be(37);

        }

        [Fact]
        public void AOC_2020_Day11a()
        {
            var input = File.ReadAllText(".//Content2//2020/Day11.txt");

            var parsedSeats = AOC_2020_Day11_Answer.Parse(input);
            var answer = AOC_2020_Day11_Answer.GetFinalConfiguration(parsedSeats);
            var finalAnswer = answer.Count(x => x.Status == '#');
            finalAnswer.Should().Be(2093);
        }

        [Fact]
        public void generate_delta_when_looking_in_each_direction()
        {
            var seats = @".......#.
...#.....
.#.......
.........
..#L....#
....#....
.........
#........
...#.....";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(seats);
            var delta = AOC_2020_Day11_Answer.GenerateDeltaWithDirectionSearch(parsedSeats);
            delta.Should().BeEmpty();
        }


        [Fact]
        public void generate_delta_when_looking_in_each_direction_2()
        {
            var seats = @".............
.L.L.#.#.#.#.
.............";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(seats);
            var delta = AOC_2020_Day11_Answer.GenerateDeltaWithDirectionSearch(parsedSeats);
            delta.Should().BeEquivalentTo(
                    new Seat
                    {
                        X = 1,
                        Y = 1,
                        Status = '#'
                    }
                );
        }

        [Fact]
        public void generate_delta_when_looking_in_each_direction_3()
        {
            var seats = @".##.##.
#.#.#.#
##...##
...L...
##...##
#.#.#.#
.##.##.";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(seats);
            var delta = AOC_2020_Day11_Answer.GenerateDeltaWithDirectionSearch(parsedSeats);
            delta.Should().BeEquivalentTo(
                    new Seat
                    {
                        X = 1,
                        Y = 1,
                        Status = '#'
                    }
                );
        }

        [Fact]
        public void calculate_final_configuration_when_looking_in_each_direction()
        {
            var input = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

            var parsedSeats = AOC_2020_Day11_Answer.Parse(input);
            var answer = AOC_2020_Day11_Answer.GetFinalConfigurationWhenLookingInEachDirection(parsedSeats);
            var finalAnswer = answer.Count(x => x.Status == '#');
            finalAnswer.Should().Be(26);

        }

        [Fact]
        public void AOC_2020_Day11b()
        {
            var input = File.ReadAllText(".//Content2//2020/Day11.txt");

            var parsedSeats = AOC_2020_Day11_Answer.Parse(input);
            var answer = AOC_2020_Day11_Answer.GetFinalConfigurationWhenLookingInEachDirection(parsedSeats);
            var finalAnswer = answer.Count(x => x.Status == '#');
            finalAnswer.Should().BeLessThan(2059);
        }

    }
}
