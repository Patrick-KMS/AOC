using AOC2024.DaySolvers;
using System.Diagnostics;

namespace AOC2024Tests.DaySolvers
{
    [TestClass()]
	public class Day20SolverTests
	{
		private readonly Day20Solver solver;
		private readonly Stopwatch sw;

		public Day20SolverTests()
		{
			solver = new();
            sw = new();
        }

        [TestMethod()]
		public void SolvePart1_Example()
		{
			solver.Limit = 0;
            var input =
                """
				###############
				#...#...#.....#
				#.#.#.#.#.###.#
				#S#...#.#.#...#
				#######.#.#.###
				#######.#.#...#
				#######.#.###.#
				###..E#...#...#
				###.#######.###
				#...###...#...#
				#.#####.#.###.#
				#.#...#.#.#...#
				#.#.#.#.#.#.###
				#...#...#...###
				###############
				""";

			sw.Restart();
            var result = solver.SolvePart1(input);
			sw.Stop();

            Assert.AreEqual(44, result);
			var max = 50;
            Assert.IsTrue(sw.ElapsedMilliseconds<= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
        }

        [TestMethod()]
		public void SolvePart1_Input()
		{
			var input = solver.GetInput();

            sw.Restart();
            var result = solver.SolvePart1(input);
            sw.Stop();

            Assert.AreEqual(1402, result);
            var max = 500;
            Assert.IsTrue(sw.ElapsedMilliseconds <= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
        }

		[TestMethod()]
		public void SolvePart2_Example()
        {
            solver.Limit = 0;
            var input =
                """
				###############
				#...#...#.....#
				#.#.#.#.#.###.#
				#S#...#.#.#...#
				#######.#.#.###
				#######.#.#...#
				#######.#.###.#
				###..E#...#...#
				###.#######.###
				#...###...#...#
				#.#####.#.###.#
				#.#...#.#.#...#
				#.#.#.#.#.#.###
				#...#...#...###
				###############
				""";

            sw.Restart();
            var result = solver.SolvePart2(input);
            sw.Stop();

            Assert.AreEqual(44, result);
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

            Assert.AreEqual(1402, result);
            var max = 500;
            Assert.IsTrue(sw.ElapsedMilliseconds <= max, $"Elapsed: {sw.ElapsedMilliseconds}>{max}");
        }
	}
}