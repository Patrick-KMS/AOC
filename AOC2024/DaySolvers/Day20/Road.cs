namespace AOC2024.DaySolvers;

public partial class Day20Solver
{
	public class Road
	{
		public Road((int, int) position, char typ, int distance = 1)
		{
			Position = position;
			Distance = distance;
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
		public int Distance { get; }
		public Road? Next { get; set; }
		public Road? Back { get; set; }
		public ICollection<Road> Cheats { get; } = [];

		public bool IsRoad { get; }
		public bool IsStart { get; }
		public bool IsEnd { get; }
		public bool IsWall { get; }
		public bool IsCheat { get; }
	}
}
