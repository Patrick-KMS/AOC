namespace AOC2024.DaySolvers;

public partial class Day20Solver
{
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

				counter += current.Distance;
			}

			return counter;
		}

		public List<IGrouping<int, Road>> RaceWithCheats()
		{
			var cheats = Roads.SelectMany(r => r.Cheats).ToList();
			var times = cheats
				.AsParallel()
				.GroupBy(Race)
				.ToList();

			return times;
		}

	}
}
