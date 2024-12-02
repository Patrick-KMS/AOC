using AOC2023.DaySolvers;

namespace AOC2023Tests.DaySolvers;

[TestClass()]
public class Day2SolverTests
{
	private Day2Solver solver = new Day2Solver();

	public Day2SolverTests()
	{
		solver = new Day2Solver();
	}

	[TestMethod()]
	public void SolvePart1_AllGamesArePossible()
	{
		var input =
			"""
			Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
			Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
			Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
			""";

		var result = solver.SolvePart1(input);
		Assert.AreEqual(8, result);
	}

	[TestMethod()]
	public void SolvePart1_Example()
	{
		var input =
			"""
			Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
			Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
			Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
			Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
			Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
			""";

		var result = solver.SolvePart1(input);
		Assert.AreEqual(8, result);
	}

	[TestMethod()]
	public void SolvePart1_Input()
	{
		var input = solver.GetInput();
		var result = solver.SolvePart1(input);
		Assert.AreEqual(3059, result);
	}

	[TestMethod()]
	public void SolvePart2_OneLien()
	{
		var input =
			"""
			Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
			""";

		var result = solver.SolvePart2(input);
		Assert.AreEqual(12, result);
	}

	[TestMethod()]
	public void SolvePart2_Example()
	{
		var input =
			"""
			Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
			Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
			Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
			Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
			Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
			""";

		var result = solver.SolvePart2(input);
		Assert.AreEqual(2286, result);
	}

	[TestMethod()]
	public void SolvePart2_Input()
	{
		var input = solver.GetInput();

		var result = solver.SolvePart2(input);
		Assert.AreEqual(65371, result);
	}
}