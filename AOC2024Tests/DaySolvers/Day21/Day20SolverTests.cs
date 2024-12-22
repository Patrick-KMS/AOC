using AOC2024.DaySolvers.Day21;
using System.Diagnostics;

namespace AOC2024Tests.DaySolvers.Day21
{
	[TestClass()]
	public class Day21SolverTests
	{
		private readonly Day21Solver solver;
		private readonly Stopwatch sw;

		public Day21SolverTests()
		{
			solver = new();
			sw = new();
		}

		[TestMethod()]
		[DataRow("029A", 1_972)]
		[DataRow("980A", 58_800)]
		[DataRow("179A", 12_172)]
		[DataRow("456A", 29_184)]
		[DataRow("379A", 24_256)]
		public void SolvePart1_Example_Single(string input, int expected)
		{
			sw.Restart();
			var result = solver.SolvePart1(input);
			sw.Stop();

			Assert.AreEqual(expected, result);
			var max = 50;
			Assert.IsTrue(sw.ElapsedMilliseconds <= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
		}

		[TestMethod()]
		public void SolvePart1_Example()
		{
			var input =
				"""
				029A
				980A
				179A
				456A
				379A
				""";

			sw.Restart();
			var result = solver.SolvePart1(input);
			sw.Stop();

			Assert.AreEqual(126384, result);
			var max = 50;
			Assert.IsTrue(sw.ElapsedMilliseconds <= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
		}

		[TestMethod()]
		public void SolvePart1_Input()
		{
			var input = solver.GetInput();

			sw.Restart();
			var result = solver.SolvePart1(input);
			sw.Stop();

			Assert.AreNotEqual(19_2836, result, "wrong");
			Assert.AreEqual(1_402, result);
			var max = 50;
			Assert.IsTrue(sw.ElapsedMilliseconds <= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
		}

		[TestMethod()]
		public void SolvePart2_Example()
		{
			var input =
				"""
				029A
				980A
				179A
				456A
				379A
				""";

			sw.Restart();
			var result = solver.SolvePart2(input);
			sw.Stop();

			Assert.AreEqual(285, result);
			var max = 50;
			Assert.IsTrue(sw.ElapsedMilliseconds <= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
		}

		[TestMethod()]
		public void SolvePart2_Input()
		{
			var input = solver.GetInput();

			sw.Restart();
			var result = solver.SolvePart2(input);
			sw.Stop();

			Assert.AreEqual(1020244, result);
			var max = 50;
			Assert.IsTrue(sw.ElapsedMilliseconds <= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
		}
	}
}