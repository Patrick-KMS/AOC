using static AOC2024.DaySolvers.Day20Solver;

namespace AOC2024.DaySolvers.Day20
{
	internal static class TrackBuilder
	{
		public static Track Build(char[][] input)
		{
			var track = TrackBuilder.BuildTrack(input);
			ConnectRoads(track.Area, track.Start);
			AddCheats(track.Area, track.Start);
			return track;
		}

		public static Track BuildV2(char[][] input)
		{
			var track = TrackBuilder.BuildTrack(input);
			ConnectRoads(track.Area, track.Start);
			AddCheats2(track.Area, track.Start);
			return track;
		}

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
			while (current is not null && !current.IsEnd)
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
					var cheat = new Road(next, 'C')
					{
						Back = current,
						Next = nextnextRoad
					};
					current.Cheats.Add(cheat);
				}
			}
		}

		private static void AddCheats2(Road[,] roads, Road start)
		{
			var current = start;
			while (current is not null && !current.IsEnd)
			{
				var r = current.Position.Item1;
				var c = current.Position.Item2;

				const int max = 20;
				for (int dx = 0; dx <= max; dx++)
				{
					for (int dy = 1; dy <= max - dx; dy++)
					{
						var distance = dx + dy - 1;
						TryAddCheat2(roads, current, (r + dx, c + dy), distance);
						TryAddCheat2(roads, current, (r + dy, c - dx), distance);
						TryAddCheat2(roads, current, (r - dy, c + dx), distance);
						TryAddCheat2(roads, current, (r - dx, c - dy), distance);
					}
				}

				current = current.Next;
			}
		}

		private static void TryAddCheat2(Road[,] roads, Road fromRoad, (int, int) to, int distance)
		{
			if (to.Item1 < 0 || to.Item1 >= roads.GetLength(0) || to.Item2 < 0 || to.Item2 >= roads.GetLength(1))
			{
				return;
			}

			Road toRoad = roads[to.Item1, to.Item2];
			if (toRoad.IsRoad)
			{
				var cheat = new Road(to, 'C', distance)
				{
					Back = fromRoad,
					Next = toRoad
				};

				fromRoad.Cheats.Add(cheat);
			}
		}

	}
}
