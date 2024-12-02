using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOC2024.DaySolvers;

namespace AOC2024Tests.DaySolvers
{
	[TestClass()]
	public class Day1SolverTests
	{
		private Day1Solver solver = new Day1Solver();

		public Day1SolverTests()
		{
			solver = new Day1Solver();
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