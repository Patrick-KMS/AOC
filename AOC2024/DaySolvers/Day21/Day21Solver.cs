﻿namespace AOC2024.DaySolvers.Day21;

public partial class Day21Solver : IDaySolver
{
	public int Day => 21;
	public string Title => "Keypad Conundrum";

	public long SolvePart1(string input)
	{
		var numbers = input.Split(Environment.NewLine).Select(line => int.Parse(line.TrimEnd('A'))).ToArray();
		var numbersKeyTypes = input.Split(Environment.NewLine).Select(GetKeyTypes).ToArray();

		var robot = Robot1.Create(3);

		var results = new List<IReadOnlyCollection<KeyType>>();
		foreach (var number in numbersKeyTypes)
		{
			var result = robot.GetMoves(number);
			results.Add(result);
		}

		var complexitys = results.Select((moves, i) => GetComplexity(numbers[i], moves.Count));
		var sum = complexitys.Sum();

		return sum;
	}

	public long SolvePart2(string input)
	{
		var numbers = input.Split(Environment.NewLine).Select(line => int.Parse(line.TrimEnd('A'))).ToArray();
		var numbersKeyTypes = input.Split(Environment.NewLine).Select(GetKeyTypes).ToArray();

		var robot = Robot2.Create(26);

		var results = numbersKeyTypes
			.AsParallel()
			.AsOrdered()
			.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
			.Select(robot.GetMoves)
			.ToList();

		var complexitys = results.Select((moves, i) => GetComplexity(numbers[i], moves));
		var sum = complexitys.Sum();

		return sum;
	}

	private static long GetComplexity(int number, long length)
	{
		var complexity = number * length;
		return complexity;
	}

	private static IReadOnlyCollection<KeyType> GetKeyTypes(string number)
	{
		var keyTypes = number.Select(KeyTypeExtensions.GetKeyType).ToArray();
		return keyTypes;
	}

	public string GetInput()
	{
		var input =
			"""
			964A
			140A
			413A
			670A
			593A
			""";

		return input;
	}
}
