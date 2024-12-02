using AOC2023.DaySolvers;

namespace AOC2023Tests.DaySolvers;

[TestClass()]
public class Day1SolverTests
{
	private Day1Solver solver = new Day1Solver();

	public Day1SolverTests()
	{
		solver = new Day1Solver();
	}

	[TestMethod()]
	public void SolvePart1_Example()
	{
		var input =
			"""
			1abc2
			pqr3stu8vwx
			a1b2c3d4e5f
			treb7uchet
			""";

		var result = solver.SolvePart1(input);
		Assert.AreEqual(142, result);
	}

	[TestMethod()]
	public void SolvePart1_Input()
	{
		var input = solver.GetInput();
		var result = solver.SolvePart1(input);
		Assert.AreEqual(54450, result);
	}

	[TestMethod()]
	public void SolvePart2()
	{
		var input =
			"""
			two1nine
			eightwothree
			abcone2threexyz
			xtwone3four
			4nineeightseven2
			zoneight234
			7pqrstsixteen
			""";

		var result = solver.SolvePart2(input);
		Assert.AreEqual(281, result);
	}

	[TestMethod()]
	public void GetInput()
	{
		var input = solver.GetInput();
		var result = solver.SolvePart2(input);
		Assert.AreEqual(54265, result);
	}
}