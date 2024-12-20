using AOC2024.DaySolvers;
using System.Diagnostics;

namespace AOC2024
{
	internal class Program
	{
        static void Main(string[] args)
		{
            Stopwatch sw = new();
            ICollection<IDaySolver> daySolvers = [
				new Day1Solver(),
				new Day2Solver(),
                new Day19Solver(),
                ];

			Console.WriteLine("Advent of Code 2023");
			Console.WriteLine("###################");
			Console.WriteLine();
			foreach (var daySolver in daySolvers)
			{
				Console.WriteLine($"Day {daySolver.Day}: {daySolver.Title}");
				var input = daySolver.GetInput();

				sw.Restart();
                var resultPart1 = daySolver.SolvePart1(input);
				sw.Stop();
                Console.WriteLine($"Result Part1 = {resultPart1} (In {sw.Elapsed:s\\.fff}s)");

                sw.Restart();
                var resultPart2 = daySolver.SolvePart2(input);
                sw.Stop();
                Console.WriteLine($"Result Part2 = {resultPart2} (In {sw.Elapsed:s\\.fff}s)");
				Console.WriteLine("-------------------");
				Console.WriteLine();
			}
		}
	}
}
