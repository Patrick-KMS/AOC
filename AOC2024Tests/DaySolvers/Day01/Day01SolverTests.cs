using AOC2024.DaySolvers.Day01;

namespace AOC2024Tests.DaySolvers.Day01
{
	[TestClass()]
	public class Day01SolverTests
	{
		private readonly Day01Solver solver;

		public Day01SolverTests()
		{
			solver = new Day01Solver();
		}

		[TestMethod()]
		public void SolvePart1_OneLIne()
		{
			var input =
				"""
				3   4
				""";

			var result = solver.SolvePart1(input);

			Assert.AreEqual(1, result);
		}

		[TestMethod()]
		public void SolvePart1_Example()
		{
			var input =
				"""
				3   4
				4   3
				2   5
				1   3
				3   9
				3   3
				""";

			var result = solver.SolvePart1(input);

			Assert.AreEqual(11, result);
		}

		[TestMethod()]
		public void SolvePart1_Input()
		{
			var input = solver.GetInput();

			var result = solver.SolvePart1(input);

			Assert.AreEqual(1580061, result);
		}
		[TestMethod()]
		public void SolvePart2_Example()
		{
			var input =
				"""
				3   4
				4   3
				2   5
				1   3
				3   9
				3   3
				""";

			var result = solver.SolvePart2(input);

			Assert.AreEqual(31, result);
		}

		[TestMethod()]
		public void SolvePart2_Input()
		{
			var input = solver.GetInput();

			var result = solver.SolvePart2(input);

			Assert.AreEqual(23046913, result);
		}
	}
}