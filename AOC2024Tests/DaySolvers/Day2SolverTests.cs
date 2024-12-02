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
	public class Day2SolverTests
	{
		private Day2Solver solver;

		public Day2SolverTests()
		{
			solver = new();
		}

		[TestMethod()]
		[DataRow("7 6 4 2 1", 1)]
		[DataRow("1 2 7 8 9", 0)]
		[DataRow("9 7 6 2 1", 0)]
		[DataRow("1 3 2 4 5", 0)]
		[DataRow("8 6 4 4 1", 0)]
		[DataRow("1 3 6 7 9", 1)]
		public void SolvePart1_OneLIne(string input, int areSave)
		{
			var result = solver.SolvePart1(input);

			Assert.AreEqual(areSave, result);
		}

		[TestMethod()]
		public void SolvePart1_Example()
		{
			var input =
				"""
				7 6 4 2 1
				1 2 7 8 9
				9 7 6 2 1
				1 3 2 4 5
				8 6 4 4 1
				1 3 6 7 9
				""";

			var result = solver.SolvePart1(input);

			Assert.AreEqual(2, result);
		}

		[TestMethod()]
		public void SolvePart1_Input()
		{
			var input = solver.GetInput();

			var result = solver.SolvePart1(input);

			Assert.AreEqual(356, result);
		}

		[TestMethod()]
		[DataRow("7 6 4 2 1", 1)]
		[DataRow("1 2 7 8 9", 0)]
		[DataRow("9 7 6 2 1", 0)]
		[DataRow("1 3 2 4 5", 1)]
		[DataRow("8 6 4 4 1", 1)]
		[DataRow("1 3 6 7 9", 1)]
		public void SolvePart2_OneLIne(string input, int areSave)
		{
			var result = solver.SolvePart2(input);

			Assert.AreEqual(areSave, result);
		}

		[TestMethod()]
		public void SolvePart2_Example()
		{
			var input =
				"""
				7 6 4 2 1
				1 2 7 8 9
				9 7 6 2 1
				1 3 2 4 5
				8 6 4 4 1
				1 3 6 7 9
				""";

			var result = solver.SolvePart2(input);

			Assert.AreEqual(4, result);
		}

		[TestMethod()]
		public void SolvePart2_Input()
		{
			var input = solver.GetInput();

			var result = solver.SolvePart2(input);

			Assert.AreEqual(413, result);
		}
	}
}