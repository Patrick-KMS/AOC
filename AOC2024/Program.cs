using AOC2024.DaySolvers;
using System.Diagnostics;

namespace AOC2024
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			Stopwatch sw = new();
			ICollection<IDaySolver> daySolvers = DaySolver.Create();

			Console.WriteLine("Advent of Code 2023");
			Console.WriteLine("###################");
			Console.WriteLine();
			foreach (var daySolver in daySolvers)
			{
				ExecuteDaySolver(daySolver, sw);
			}
		}

		private static void ExecuteDaySolver(IDaySolver daySolver, Stopwatch sw)
		{
			Console.WriteLine($"Day {daySolver.Day}: {daySolver.Title}");
			var input = daySolver.GetInput();

			Console.Write("Part1: ");
			Execute(input, daySolver.SolvePart1, sw);

			Console.Write("Part2: ");
			Execute(input, daySolver.SolvePart2, sw);

			Console.WriteLine("-------------------");
			Console.WriteLine();
		}

		private static void Execute(string input, Func<string, long> action, Stopwatch sw)
		{
			sw.Restart();
			var result = action(input);
			sw.Stop();
			Console.WriteLine($"Result = {result} (In {sw.Elapsed:s\\.fff}s)");
		}
	}
}
