using AOC2023.DaySolvers;
using System;
using System.Collections.Generic;

namespace AOC2023;

internal static class Program
{
	private static void Main(string[] args)
	{
		ICollection<IDaySolver> daySolvers = [
			new Day1Solver(),
			new Day2Solver()
			];

		Console.WriteLine("Advent of Code 2023");
		Console.WriteLine("###################");
		Console.WriteLine();
		foreach (var daySolver in daySolvers)
		{
			Console.WriteLine($"Day {daySolver.Day}: {daySolver.Title}");
			var input = daySolver.GetInput();
			var resultPart1 = daySolver.SolvePart1(input);
			Console.WriteLine($"Result Part1 = {resultPart1}");
			var resultPart2 = daySolver.SolvePart2(input);
			Console.WriteLine($"Result Part2 = {resultPart2}");
			Console.WriteLine("-------------------");
			Console.WriteLine();
		}
	}
}