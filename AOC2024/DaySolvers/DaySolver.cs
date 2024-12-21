using AOC2024.DaySolvers.Day01;
using AOC2024.DaySolvers.Day02;
using AOC2024.DaySolvers.Day19;

namespace AOC2024.DaySolvers
{
	internal static class DaySolver
	{
		public static ICollection<IDaySolver> Create()
		{
			ICollection<IDaySolver> daySolvers = [
				new Day01Solver(),
				new Day02Solver(),
				new Day19Solver(),
				new Day20Solver(),
				];

			return daySolvers;
		}
	}
}
