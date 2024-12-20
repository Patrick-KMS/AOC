﻿using System;
using System.Collections.ObjectModel;

namespace AOC2024.DaySolvers;

public class Day20Solver : IDaySolver
{
    public int Day => 20;
    public string Title => "Race Condition";

    public long SolvePart1(string input)
    {
        var trackInput = input.Split(Environment.NewLine).Select(line => line.ToCharArray()).ToArray();
        var track = BuildTrack(trackInput);
        ConnectRoads(track.Area, track.Start);
        AddCheats(track.Area, track.Start);

        var time = track.Race();
        var cheatTimes = track.RaceWithCheats();
        var orderedCheatTimes = cheatTimes
            .Where(g => time - g.Key > 0)
            .Select(g => (g.Count(), time - g.Key))
            .OrderBy(g => g.Item2)
            .ToList();

        var sum = orderedCheatTimes
            .Where(g => g.Item2 >= Limit)
            .Sum(g => g.Item1);

        return sum;
    }

    public int Limit { get; set; } = 100;

    private static Track BuildTrack(char[][] input)
    {
        Road? Start = null;
        Road? End = null;
        Road[,] roads = new Road[input.Length, input[0].Length];

        for (int r = 0; r < input.Length; r++)
        {
            for (int c = 0; c < input[r].Length; c++)
            {
                var cell = input[r][c];
                var road = new Road((r, c), cell);
                roads[r, c] = road;

                if (road.IsStart)
                {
                    Start = road;
                }
                if (road.IsEnd)
                {
                    End = road;
                }
            }
        }

        if (Start is null || End is null)
        {
            throw new InvalidOperationException("Start or End not found");
        }

        var track = new Track(Start, End, roads);
        return track;
    }

    private static void ConnectRoads(Road[,] roads, Road start)
    {
        var current = start;
        while (!current.IsEnd)
        {
            int r = current.Position.Item1;
            int c = current.Position.Item2;

            var top = roads[r + 1, c];
            var down = roads[r - 1, c];
            var left = roads[r, c - 1];
            var right = roads[r, c + 1];

            if (top != current.Back && top.IsRoad)
            {
                current.Next = top;
                top.Back = current;
            }
            else if (down != current.Back && down.IsRoad)
            {
                current.Next = down;
                down.Back = current;
            }
            else if (left != current.Back && left.IsRoad)
            {
                current.Next = left;
                left.Back = current;
            }
            else if (right != current.Back && right.IsRoad)
            {
                current.Next = right;
                right.Back = current;
            }
            else
            {
                throw new InvalidOperationException("No road found");
            }

            current = current.Next;
        }
    }

    private static void AddCheats(Road[,] roads, Road start)
    {
        var current = start;
        while (!current.IsEnd)
        {
            var r = current.Position.Item1;
            var c = current.Position.Item2;

            TryAddCheat(roads, current, (r + 1, c), (r + 2, c));
            TryAddCheat(roads, current, (r - 1, c), (r - 2, c));
            TryAddCheat(roads, current, (r, c + 1), (r, c + 2));
            TryAddCheat(roads, current, (r, c - 1), (r, c - 2));

            current = current.Next;
        }
    }

    private static void TryAddCheat(Road[,] roads, Road current, (int, int) next, (int, int) nextnext)
    {
        if (nextnext.Item1 < 0 || nextnext.Item1 >= roads.GetLength(0) || nextnext.Item2 < 0 || nextnext.Item2 >= roads.GetLength(1))
        {
            return;
        }

        Road nextRoad = roads[next.Item1, next.Item2];
        if (nextRoad != current.Back && nextRoad != current.Next && nextRoad.IsWall)
        {
            Road nextnextRoad = roads[nextnext.Item1, nextnext.Item2];
            if (nextnextRoad.IsRoad)
            {
                var cheat = new Road(next, 'C');
                cheat.Back = current;
                cheat.Next = nextnextRoad;
                current.Cheats.Add(cheat);
            }
        }
    }

    public class Track
    {
        public Track(Road start, Road end, Road[,] area)
        {
            Start = start;
            Entd = end;
            Area = area;
            Roads = area.Cast<Road>().Where(r => r.IsRoad).ToList();
        }

        public Road Start { get; }
        public Road Entd { get; }
        public Road[,] Area { get; }
        public ICollection<Road> Roads { get; }

        public int Race(Road? cheat = null)
        {
            var current = Start;
            var counter = 0;
            while (current.Next is not null)
            {
                if (cheat is not null && current == cheat.Back)
                {
                    current = cheat;
                    cheat = null;
                }
                else
                {
                    current = current.Next;
                }

                counter++;
            }

            return counter;
        }

        public List<IGrouping<int, Road>> RaceWithCheats()
        {
            var cheats = Roads.SelectMany(r => r.Cheats).ToList();
            var times = cheats
                .AsParallel()
                .GroupBy(cheat => Race(cheat))
                .ToList();

            return times;
        }

    }
    public class Road
    {
        public Road((int, int) position, char typ)
        {
            Position = position;
            switch (typ)
            {
                case '.':
                    IsRoad = true;
                    break;
                case 'S':
                    IsRoad = true;
                    IsStart = true;
                    break;
                case 'E':
                    IsRoad = true;
                    IsEnd = true;
                    break;
                case '#':
                    IsWall = true;
                    break;
                case 'C':
                    IsCheat = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typ));
            }
        }

        public (int, int) Position { get; }
        public Road? Next { get; set; }
        public Road? Back { get; set; }
        public ICollection<Road> Cheats { get; } = [];

        public bool IsRoad { get; }
        public bool IsStart { get; }
        public bool IsEnd { get; }
        public bool IsWall { get; }
        public bool IsCheat { get; }
    }

    public long SolvePart2(string input)
    {
        return SolvePart1(input);
    }


    public string GetInput()
    {
        var input =
            """
			#############################################################################################################################################
			#...........#...#...#.....#.............#.........#...#...........#...#.......#...#...###.....#...#...#...###...#...#...#...#...#...###.....#
			#.#########.#.#.#.#.#.###.#.###########.#.#######.#.#.#.#########.#.#.#.#####.#.#.#.#.###.###.#.#.#.#.#.#.###.#.#.#.#.#.#.#.#.#.#.#.###.###.#
			#.......#...#.#.#.#.#...#.#...#.....#...#...#.....#.#.#.....#.....#.#.#...#...#.#.#.#...#...#...#...#.#.#...#.#.#.#.#.#.#.#.#.#...#...#.#...#
			#######.#.###.#.#.#.###.#.###.#.###.#.#####.#.#####.#.#####.#.#####.#.###.#.###.#.#.###.###.#########.#.###.#.#.#.#.#.#.#.#.#.#######.#.#.###
			###...#.#.....#...#.....#.###.#.###...#...#.#.#...#.#...#...#...#...#.....#.###.#.#.#...#...#.........#...#.#.#...#.#.#...#.#.#.......#.#...#
			###.#.#.#################.###.#.#######.#.#.#.#.#.#.###.#.#####.#.#########.###.#.#.#.###.###.###########.#.#.#####.#.#####.#.#.#######.###.#
			#...#.#...#...........#...#...#...#...#.#.#.#...#.#.#...#.....#.#.........#.#...#.#.#...#...#...#...###...#...#.....#...#...#.#.....###.#...#
			#.###.###.#.#########.#.###.#####.#.#.#.#.#.#####.#.#.#######.#.#########.#.#.###.#.###.###.###.#.#.###.#######.#######.#.###.#####.###.#.###
			#...#.###...#.........#...#.#.....#.#.#.#.#.#.....#.#.#...#...#.#...###...#.#.#...#...#...#...#...#...#.....#...#.....#.#...#.#.....#...#...#
			###.#.#######.###########.#.#.#####.#.#.#.#.#.#####.#.#.#.#.###.#.#.###.###.#.#.#####.###.###.#######.#####.#.###.###.#.###.#.#.#####.#####.#
			#...#...#.....#...#.....#.#.#.....#.#.#.#.#.#.#.....#.#.#.#...#...#...#...#.#.#.#...#.#...###.#.....#.#...#.#.###...#.#.#...#.#.#.....#...#.#
			#.#####.#.#####.#.#.###.#.#.#####.#.#.#.#.#.#.#.#####.#.#.###.#######.###.#.#.#.#.#.#.#.#####.#.###.#.#.#.#.#.#####.#.#.#.###.#.#.#####.#.#.#
			#.....#.#.......#...#...#...#...#.#.#...#...#.#.....#.#.#...#...#.....#...#.#.#...#.#.#.#...#.#...#...#.#.#.#...###.#...#...#.#...#.....#...#
			#####.#.#############.#######.#.#.#.#########.#####.#.#.###.###.#.#####.###.#.#####.#.#.#.#.#.###.#####.#.#.###.###.#######.#.#####.#########
			#.....#.#...#.......#.......#.#...#.....#.....#...#.#.#...#.#...#.#...#...#.#.....#.#.#.#.#.#.#...#...#.#.#...#...#...#.....#.#.....#...#...#
			#.#####.#.#.#.#####.#######.#.#########.#.#####.#.#.#.###.#.#.###.#.#.###.#.#####.#.#.#.#.#.#.#.###.#.#.#.###.###.###.#.#####.#.#####.#.#.#.#
			#.....#.#.#.#.....#.#...#...#...#...###.#.#...#.#.#.#.#...#...#...#.#...#.#...#...#.#.#...#...#...#.#...#...#...#.....#.#...#.#...###.#...#.#
			#####.#.#.#.#####.#.#.#.#.#####.#.#.###.#.#.#.#.#.#.#.#.#######.###.###.#.###.#.###.#.###########.#.#######.###.#######.#.#.#.###.###.#####.#
			#.....#...#.###...#...#.#.#.....#.#.#...#.#.#.#.#.#.#.#...#.....###.#...#...#.#...#.#.#...........#.#.......###.#.......#.#.#...#.....#.....#
			#.#########.###.#######.#.#.#####.#.#.###.#.#.#.#.#.#.###.#.#######.#.#####.#.###.#.#.#.###########.#.#########.#.#######.#.###.#######.#####
			#.........#...#.......#...#.#.....#.#...#.#.#...#.#.#.#...#.#...#...#.#.....#...#.#...#.#...........#.#.....#...#...#...#.#.#...#...#...#...#
			#########.###.#######.#####.#.#####.###.#.#.#####.#.#.#.###.#.#.#.###.#.#######.#.#####.#.###########.#.###.#.#####.#.#.#.#.#.###.#.#.###.#.#
			#...#...#.#...#.....#.....#.#.....#.#...#.#.#.....#.#.#...#...#.#...#.#.......#.#.....#...#...........#.#...#.#.....#.#.#.#.#.#...#...###.#.#
			#.#.#.#.#.#.###.###.#####.#.#####.#.#.###.#.#.#####.#.###.#####.###.#.#######.#.#####.#####.###########.#.###.#.#####.#.#.#.#.#.#########.#.#
			#.#.#.#...#...#...#.......#.#...#.#.#...#.#.#.#...#.#.....#.....#...#...#.....#...#...#...#...#...#...#.#.#...#...#...#.#.#...#.........#.#.#
			#.#.#.#######.###.#########.#.#.#.#.###.#.#.#.#.#.#.#######.#####.#####.#.#######.#.###.#.###.#.#.#.#.#.#.#.#####.#.###.#.#############.#.#.#
			#.#...#.....#...#.........#...#.#.#.#...#.#.#.#.#.#.......#.#...#.#.....#...#.....#.#...#.....#.#.#.#.#.#.#...#...#...#.#.#.............#.#.#
			#.#####.###.###.#########.#####.#.#.#.###.#.#.#.#.#######.#.#.#.#.#.#######.#.#####.#.#########.#.#.#.#.#.###.#.#####.#.#.#.#############.#.#
			#...#...###...#.......###.#.....#.#...#...#.#.#.#.....###.#...#...#...#...#.#...###.#.###...###.#.#.#.#.#...#.#...###.#.#.#.......#.....#.#.#
			###.#.#######.#######.###.#.#####.#####.###.#.#.#####.###.###########.#.#.#.###.###.#.###.#.###.#.#.#.#.###.#.###.###.#.#.#######.#.###.#.#.#
			#...#.#.....#.......#.....#...###.....#.#...#.#.....#.#...#...#.......#.#.#.#...#...#...#.#.#...#.#.#...#...#...#.#...#...#.......#.#...#.#.#
			#.###.#.###.#######.#########.#######.#.#.###.#####.#.#.###.#.#.#######.#.#.#.###.#####.#.#.#.###.#.#####.#####.#.#.#######.#######.#.###.#.#
			#.....#.#...###...#.........#.#...###.#.#.#...#...#.#.#...#.#...###...#.#.#.#.###...#...#.#.#...#.#...#...#...#.#.#.#.......#.......#...#.#.#
			#######.#.#####.#.#########.#.#.#.###.#.#.#.###.#.#.#.###.#.#######.#.#.#.#.#.#####.#.###.#.###.#.###.#.###.#.#.#.#.#.#######.#########.#.#.#
			#...#...#.#.....#.....#...#.#.#.#.#...#.#.#...#.#.#.#.#...#.#...#...#...#.#.#...#...#...#.#.#...#.#...#...#.#.#.#.#.#.#.....#.#.........#.#.#
			#.#.#.###.#.#########.#.#.#.#.#.#.#.###.#.###.#.#.#.#.#.###.#.#.#.#######.#.###.#.#####.#.#.#.###.#.#####.#.#.#.#.#.#.#.###.#.#.#########.#.#
			#.#.#...#.#.........#...#...#...#...###.#.#...#.#.#.#.#...#.#.#.#...#.....#...#.#...#...#.#.#.#...#...###...#...#.#.#...#...#.#.....###...#.#
			#.#.###.#.#########.###################.#.#.###.#.#.#.###.#.#.#.###.#.#######.#.###.#.###.#.#.#.#####.###########.#.#####.###.#####.###.###.#
			#.#.....#...#.......#.................#...#...#.#...#...#.#...#.#...#...#...#.#.#...#...#.#.#.#.#...#.#...........#.#.....#...#...#...#...#.#
			#.#########.#.#######.###############.#######.#.#######.#.#####.#.#####.#.#.#.#.#.#####.#.#.#.#.#.#.#.#.###########.#.#####.###.#.###.###.#.#
			#.......#...#.......#.#...............#...#...#...#...#.#.....#.#.....#.#.#.#.#.#...#...#.#.#.#.#.#...#.............#.###...#...#...#.....#.#
			#######.#.#########.#.#.###############.#.#.#####.#.#.#.#####.#.#####.#.#.#.#.#.###.#.###.#.#.#.#.###################.###.###.#####.#######.#
			#.......#...#.....#...#.................#.#.#...#...#.#.#...#.#.#...#.#.#.#.#.#.....#...#.#.#.#.#.#.....#...........#.....#...###...#.......#
			#.#########.#.###.#######################.#.#.#.#####.#.#.#.#.#.#.#.#.#.#.#.#.#########.#.#.#.#.#.#.###.#.#########.#######.#####.###.#######
			#.........#.#...#...#.....................#...#...#...#.#.#...#.#.#.#.#.#.#.#.###.......#.#...#...#...#.#.........#.#...#...#...#.....#...###
			#########.#.###.###.#.###########################.#.###.#.#####.#.#.#.#.#.#.#.###.#######.###########.#.#########.#.#.#.#.###.#.#######.#.###
			#.........#.....#...#.#.....................#...#.#...#.#...###.#.#...#.#.#.#...#...#...#.#######S..#.#.#.....#...#.#.#...###.#.#...#...#...#
			#.###############.###.#.###################.#.#.#.###.#.###.###.#.#####.#.#.###.###.#.#.#.#########.#.#.#.###.#.###.#.#######.#.#.#.#.#####.#
			#.......#...#...#...#...#...#...#.....#...#...#.#.....#...#...#.#.....#.#.#...#.#...#.#.#.#########...#...###...###.#.........#...#...#...#.#
			#######.#.#.#.#.###.#####.#.#.#.#.###.#.#.#####.#########.###.#.#####.#.#.###.#.#.###.#.#.#########################.###################.#.#.#
			#.......#.#...#...#...#...#...#.#...#...#.......#.......#.....#.#...#.#.#...#.#.#...#.#.#.#####...............#...#.........#...#.....#.#...#
			#.#######.#######.###.#.#######.###.#############.#####.#######.#.#.#.#.###.#.#.###.#.#.#.#####.#############.#.#.#########.#.#.#.###.#.#####
			#.........#...###...#.#...#.....###.###...#...###.#...#.......#...#.#.#...#.#.#.###...#...#####...#.........#.#.#...#...###...#.#.#...#...###
			###########.#.#####.#.###.#.#######.###.#.#.#.###.#.#.#######.#####.#.###.#.#.#.#################.#.#######.#.#.###.#.#.#######.#.#.#####.###
			#...........#...#...#.....#...#...#...#.#...#.....#.#.......#.....#.#...#...#...###############...#.......#...#.#...#.#.#...###...#.....#...#
			#.#############.#.###########.#.#.###.#.###########.#######.#####.#.###.#######################.#########.#####.#.###.#.#.#.###########.###.#
			#.....#...#...#...#...#.....#...#.....#...#...#...#.......#.......#.#...#.......#####.....#...#.#...#...#.......#.....#...#...#.....###...#.#
			#####.#.#.#.#.#####.#.#.###.#############.#.#.#.#.#######.#########.#.###.#####.#####.###.#.#.#.#.#.#.#.#####################.#.###.#####.#.#
			#.....#.#.#.#.#.....#.#...#...#...........#.#.#.#.#...#...#...#...#...#...#...#..E###...#...#...#.#.#.#...........#...........#.#...#...#...#
			#.#####.#.#.#.#.#####.###.###.#.###########.#.#.#.#.#.#.###.#.#.#.#####.###.#.#########.#########.#.#.###########.#.###########.#.###.#.#####
			#.......#...#...#.....#...###...#.........#.#...#...#.#.....#...#...###.....#...#.......#.........#...#.........#.#.............#.....#.....#
			#################.#####.#########.#######.#.#########.#############.###########.#.#######.#############.#######.#.#########################.#
			#...#.....#...#...#...#...#.....#.......#.#.........#.#...#...#...#...###...###...#.....#...............#.......#...#...#...#.....#.........#
			#.#.#.###.#.#.#.###.#.###.#.###.#######.#.#########.#.#.#.#.#.#.#.###.###.#.#######.###.#################.#########.#.#.#.#.#.###.#.#########
			#.#.#...#...#...#...#.#...#...#.......#.#.#.........#.#.#.#.#.#.#...#.#...#.......#...#.#...#...#.......#.#...#...#...#...#...###...#...#...#
			#.#.###.#########.###.#.#####.#######.#.#.#.#########.#.#.#.#.#.###.#.#.#########.###.#.#.#.#.#.#.#####.#.#.#.#.#.###################.#.#.#.#
			#.#.....#...#...#.#...#.#...#...#...#...#.#.........#...#...#.#.#...#.#...#.......#...#...#...#.#.....#.#...#...#...#...###...###.....#...#.#
			#.#######.#.#.#.#.#.###.#.#.###.#.#.#####.#########.#########.#.#.###.###.#.#######.###########.#####.#.###########.#.#.###.#.###.#########.#
			#...#.....#...#.#.#...#.#.#...#...#.....#.#...#.....#...#...#...#.....#...#.....#...#.....#...#.......#...........#...#.....#.....#.....#...#
			###.#.#########.#.###.#.#.###.#########.#.#.#.#.#####.#.#.#.###########.#######.#.###.###.#.#.###################.#################.###.#.###
			#...#.#.........#.###.#.#...#...###.....#.#.#.#...###.#.#.#...#...###...#.....#...#...###...#.....#...#.........#...#...#.....#...#...#.#...#
			#.###.#.#########.###.#.###.###.###.#####.#.#.###.###.#.#.###.#.#.###.###.###.#####.#############.#.#.#.#######.###.#.#.#.###.#.#.###.#.###.#
			#...#.#.........#.#...#.###...#.#...#...#...#...#...#.#.#.#...#.#.#...#...###...#...#...........#...#.#.#.......#...#.#.#...#.#.#.#...#...#.#
			###.#.#########.#.#.###.#####.#.#.###.#.#######.###.#.#.#.#.###.#.#.###.#######.#.###.#########.#####.#.#.#######.###.#.###.#.#.#.#.#####.#.#
			#...#.#.........#.#.#...#...#.#.#...#.#.###...#...#.#.#...#...#.#.#.#...#.....#...###.......#...#...#...#...#...#.....#.#...#.#.#.#...#...#.#
			#.###.#.#########.#.#.###.#.#.#.###.#.#.###.#.###.#.#.#######.#.#.#.#.###.###.#############.#.###.#.#######.#.#.#######.#.###.#.#.###.#.###.#
			#...#.#...........#...#...#...#.....#.#...#.#.###...#.#.......#.#.#...###...#.#...#...#.....#.....#.....#...#.#.....#...#...#.#.#.#...#.#...#
			###.#.#################.#############.###.#.#.#######.#.#######.#.#########.#.#.#.#.#.#.###############.#.###.#####.#.#####.#.#.#.#.###.#.###
			###...###...........#...#...#...#.....#...#.#...#...#.#.....###.#.#...###...#...#...#.#.....#...#.....#...#...#.....#...#...#.#.#.#...#...###
			#########.#########.#.###.#.#.#.#.#####.###.###.#.#.#.#####.###.#.#.#.###.###########.#####.#.#.#.###.#####.###.#######.#.###.#.#.###.#######
			#.......#.....#...#.#.....#...#...#...#...#.#...#.#.#.#.....#...#...#...#.....#.......#...#...#...###.......#...#...###...###.#.#.....#.....#
			#.#####.#####.#.#.#.###############.#.###.#.#.###.#.#.#.#####.#########.#####.#.#######.#.###################.###.#.#########.#.#######.###.#
			#.....#.#...#...#.#.#...###...#.....#.....#.#.#...#...#...###...#.......#.....#.###...#.#...#.......#.........###.#.#...#...#...#.......#...#
			#####.#.#.#.#####.#.#.#.###.#.#.###########.#.#.#########.#####.#.#######.#####.###.#.#.###.#.#####.#.###########.#.#.#.#.#.#####.#######.###
			#.....#...#.......#...#.....#.#.#...###...#.#.#...#.......#...#.#.###...#.....#...#.#.#.#...#.....#.#.............#.#.#.#.#.....#...#.....###
			#.###########################.#.#.#.###.#.#.#.###.#.#######.#.#.#.###.#.#####.###.#.#.#.#.#######.#.###############.#.#.#.#####.###.#.#######
			#.#...#.....................#.#...#.....#...#.....#.......#.#...#...#.#.#...#...#.#.#.#.#.........#.....#...........#.#...#...#.....#...#...#
			#.#.#.#.###################.#.###########################.#.#######.#.#.#.#.###.#.#.#.#.###############.#.###########.#####.#.#########.#.#.#
			#.#.#...#.....#.......#.....#...#...................#.....#.....#...#.#.#.#.....#...#.#...............#...#...........#.....#...........#.#.#
			#.#.#####.###.#.#####.#.#######.#.#################.#.#########.#.###.#.#.###########.###############.#####.###########.#################.#.#
			#...#...#...#.#.#...#.#.....#...#.................#...#...#...#.#.#...#.#.#...#.....#.#.........###...#.....#...........#...###...#.......#.#
			#####.#.###.#.#.#.#.#.#####.#.###################.#####.#.#.#.#.#.#.###.#.#.#.#.###.#.#.#######.###.###.#####.###########.#.###.#.#.#######.#
			###...#.....#...#.#.#.......#...............#.....###...#.#.#.#.#.#...#.#...#.#.###...#.......#.....#...#...#...........#.#...#.#.#.#.......#
			###.#############.#.#######################.#.#######.###.#.#.#.#.###.#.#####.#.#############.#######.###.#.###########.#.###.#.#.#.#.#######
			#...#...#...#...#.#...#.....#...#.........#.#.......#...#...#...#.#...#.....#.#...........###.......#.#...#.............#.#...#.#.#.#.###...#
			#.###.#.#.#.#.#.#.###.#.###.#.#.#.#######.#.#######.###.#########.#.#######.#.###########.#########.#.#.#################.#.###.#.#.#.###.#.#
			#...#.#...#...#.#.#...#.#...#.#.#.#...#...#.......#.###...#.......#.......#...#.....#.....#.......#...#...#...............#.....#...#...#.#.#
			###.#.#########.#.#.###.#.###.#.#.#.#.#.#########.#.#####.#.#############.#####.###.#.#####.#####.#######.#.###########################.#.#.#
			#...#.###...#...#.#...#.#.#...#...#.#...#.......#...#...#.#.......#.......#.....###...#...#...#...###...#...#.....#.....#...#.....#...#...#.#
			#.###.###.#.#.###.###.#.#.#.#######.#####.#####.#####.#.#.#######.#.#######.###########.#.###.#.#####.#.#####.###.#.###.#.#.#.###.#.#.#####.#
			#.#...#...#...#...###...#...#.......#...#.....#...#...#.#.#.......#.......#...#...#...#.#.#...#.....#.#.#...#...#.#...#.#.#.#.###.#.#.....#.#
			#.#.###.#######.#############.#######.#.#####.###.#.###.#.#.#############.###.#.#.#.#.#.#.#.#######.#.#.#.#.###.#.###.#.#.#.#.###.#.#####.#.#
			#.#.#...#.......#.....#.......#.....#.#.#...#...#.#...#.#.#.#...###...###.#...#.#.#.#.#.#.#.....#...#.#.#.#.....#...#.#...#.#...#.#.#...#...#
			#.#.#.###.#######.###.#.#######.###.#.#.#.#.###.#.###.#.#.#.#.#.###.#.###.#.###.#.#.#.#.#.#####.#.###.#.#.#########.#.#####.###.#.#.#.#.#####
			#...#.....#...###...#.#...#...#.#...#.#.#.#.#...#...#.#.#.#.#.#.#...#...#.#...#.#...#.#.#.....#.#.#...#.#.........#.#.....#...#.#.#...#...###
			###########.#.#####.#.###.#.#.#.#.###.#.#.#.#.#####.#.#.#.#.#.#.#.#####.#.###.#.#####.#.#####.#.#.#.###.#########.#.#####.###.#.#.#######.###
			#...........#.....#.#.###...#...#.#...#...#.#...#...#.#.#.#.#.#...###...#.#...#.....#...#.....#.#.#.###.#...#...#.#.#.....###.#.#...#...#...#
			#.###############.#.#.###########.#.#######.###.#.###.#.#.#.#.#######.###.#.#######.#####.#####.#.#.###.#.#.#.#.#.#.#.#######.#.###.#.#.###.#
			#...........#...#...#.#...#...#...#...#.....#...#.#...#.#.#.#.......#.#...#.....#...#.....#...#.#...#...#.#.#.#...#.#.......#...###...#...#.#
			###########.#.#.#####.#.#.#.#.#.#####.#.#####.###.#.###.#.#.#######.#.#.#######.#.###.#####.#.#.#####.###.#.#.#####.#######.#############.#.#
			#...........#.#.#...#...#...#...#...#.#.#...#.###.#.###...#...#...#.#.#.....#...#.#...#...#.#.#.#.....#...#...###...#...#...#.......#.....#.#
			#.###########.#.#.#.#############.#.#.#.#.#.#.###.#.#########.#.#.#.#.#####.#.###.#.###.#.#.#.#.#.#####.#########.###.#.#.###.#####.#.#####.#
			#...........#.#.#.#...#.........#.#.#.#...#.#.#...#.....#...#.#.#.#.#.#.....#...#.#...#.#.#.#...#.#...#.#...#...#...#.#.#...#.#.....#.....#.#
			###########.#.#.#.###.#.#######.#.#.#.#####.#.#.#######.#.#.#.#.#.#.#.#.#######.#.###.#.#.#.#####.#.#.#.#.#.#.#.###.#.#.###.#.#.#########.#.#
			#...........#.#...###...#.......#.#.#.#...#.#.#.#.....#.#.#...#.#.#.#.#.....#...#...#.#.#.#...#...#.#.#...#...#...#.#.#...#.#.#.#...#...#.#.#
			#.###########.###########.#######.#.#.#.#.#.#.#.#.###.#.#.#####.#.#.#.#####.#.#####.#.#.#.###.#.###.#.###########.#.#.###.#.#.#.#.#.#.#.#.#.#
			#.#...........#...###...#.....#...#.#.#.#...#.#.#...#.#.#...#...#.#.#.#...#.#.#...#.#.#.#.#...#.#...#.#...#...#...#.#.#...#...#...#...#.#.#.#
			#.#.###########.#.###.#.#####.#.###.#.#.#####.#.###.#.#.###.#.###.#.#.#.#.#.#.#.#.#.#.#.#.#.###.#.###.#.#.#.#.#.###.#.#.###############.#.#.#
			#...###.........#.....#.......#.###...#.....#.#.#...#.#.###.#...#.#.#.#.#.#.#.#.#.#.#...#.#.###...###.#.#...#.#...#.#.#.....#.....#.....#...#
			#######.#######################.###########.#.#.#.###.#.###.###.#.#.#.#.#.#.#.#.#.#.#####.#.#########.#.#####.###.#.#.#####.#.###.#.#########
			#.....#...#...#...........#...#.......#.....#.#.#...#...#...#...#.#.#...#.#.#.#.#...#.....#.#.........#.#...#.....#...###...#...#.#...#...###
			#.###.###.#.#.#.#########.#.#.#######.#.#####.#.###.#####.###.###.#.#####.#.#.#.#####.#####.#.#########.#.#.#############.#####.#.###.#.#.###
			#...#...#...#.#.#.........#.#...#...#.#...#...#.#...#.....#...#...#.....#.#.#...###...#.....#.#...#...#...#.....#...#...#.#...#.#...#...#...#
			###.###.#####.#.#.#########.###.#.#.#.###.#.###.#.###.#####.###.#######.#.#.#######.###.#####.#.#.#.#.#########.#.#.#.#.#.#.#.#.###.#######.#
			###.#...#.....#.#.........#.###.#.#.#.#...#.#...#...#.#...#...#...#...#.#.#.#.......#...#.....#.#.#.#.#...#...#...#...#.#...#.#...#...#.....#
			###.#.###.#####.#########.#.###.#.#.#.#.###.#.#####.#.#.#.###.###.#.#.#.#.#.#.#######.###.#####.#.#.#.#.#.#.#.#########.#####.###.###.#.#####
			#...#.....#.....#...#.....#...#.#.#.#.#.#...#...#...#.#.#.....#...#.#.#.#.#.#.#.....#.###...#...#...#.#.#.#.#.#...#...#.....#.....#...#.....#
			#.#########.#####.#.#.#######.#.#.#.#.#.#.#####.#.###.#.#######.###.#.#.#.#.#.#.###.#.#####.#.#######.#.#.#.#.#.#.#.#.#####.#######.#######.#
			#...#.....#.#.....#...#.....#.#.#.#.#.#.#...#...#...#.#...#...#.....#...#.#.#.#...#.#.#.....#.....#...#.#.#.#.#.#.#.#.#...#...#...#...#...#.#
			###.#.###.#.#.#########.###.#.#.#.#.#.#.###.#.#####.#.###.#.#.###########.#.#.###.#.#.#.#########.#.###.#.#.#.#.#.#.#.#.#.###.#.#.###.#.#.#.#
			#...#.#...#.#.....#...#.###.#.#...#.#.#...#.#.....#.#.#...#.#...#...#.....#.#.#...#...#.#.....#...#...#.#.#.#.#.#...#.#.#...#...#...#.#.#.#.#
			#.###.#.###.#####.#.#.#.###.#.#####.#.###.#.#####.#.#.#.###.###.#.#.#.#####.#.#.#######.#.###.#.#####.#.#.#.#.#.#####.#.###.#######.#.#.#.#.#
			#.....#...#.#.....#.#.#...#.#.###...#...#.#.#.....#.#.#.....#...#.#...#...#.#.#.......#...###.#.#...#...#.#.#.#.....#.#.###.#.......#.#.#.#.#
			#########.#.#.#####.#.###.#.#.###.#####.#.#.#.#####.#.#######.###.#####.#.#.#.#######.#######.#.#.#.#####.#.#.#####.#.#.###.#.#######.#.#.#.#
			###...###...#...#...#...#.#.#...#...#...#.#.#.....#.#...#...#.#...#...#.#.#.#.#.....#.###.....#...#...#...#.#...#...#...#...#.......#...#.#.#
			###.#.#########.#.#####.#.#.###.###.#.###.#.#####.#.###.#.#.#.#.###.#.#.#.#.#.#.###.#.###.###########.#.###.###.#.#######.#########.#####.#.#
			#...#...........#.....#.#.#...#...#.#...#.#.#.....#...#.#.#.#.#.#...#.#.#.#.#.#.#...#.#...#.....#...#.#...#.#...#.......#.#.....#...#.....#.#
			#.###################.#.#.###.###.#.###.#.#.#.#######.#.#.#.#.#.#.###.#.#.#.#.#.#.###.#.###.###.#.#.#.###.#.#.#########.#.#.###.#.###.#####.#
			#.....................#...###.....#.....#...#.........#...#...#...###...#...#...#.....#.....###...#...###...#...........#...###...###.......#
			#############################################################################################################################################
			""";

        return input;
    }
}
