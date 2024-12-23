using AOC2024.DaySolvers.Day21;

namespace AOC2024Tests.DaySolvers.Day21
{
	[TestClass()]
	public class RobotTests
	{
		[TestMethod()]
		[DataRow("A", "A")]
		[DataRow("0", "<A")]
		[DataRow("1", "^<<A")]
		[DataRow("2", "<^A")]
		[DataRow("3", "^A")]
		[DataRow("4", "^^<<A")]
		[DataRow("5", "<^^A")]
		[DataRow("6", "^^A")]
		[DataRow("7", "^^^<<A")]
		[DataRow("8", "<^^^A")]
		[DataRow("9", "^^^A")]
		//Example
		[DataRow("029A", "<A^A>^^AvvvA")]
		[DataRow("980A", "^^^A<AvvvA>A")]
		[DataRow("179A", "^<<A^^A>>AvvvA")]
		[DataRow("456A", "^^<<A>A>AvvA")]
		[DataRow("379A", "^A<<^^A>>AvvvA")]
		public void Press_Numeric(string input, string expected)
		{
			var keys = input.Select(KeyTypeExtensions.GetKeyType).ToArray();

			var robot = Robot1.Create(1);
			var moves = robot.GetMoves(keys);

			var moveStrings = moves.Select(key => key.GetChar()).ToArray();
			var result = string.Concat(moveStrings);
			Assert.AreEqual(expected, result);
		}

		[TestMethod()]
		[DataRow("A", "A")]
		[DataRow("0", "v<<A>>^A")]
		[DataRow("1", "<Av<AA>>^A")]
		[DataRow("2", "v<<A>^A>A")]
		[DataRow("3", "<A>A")]
		[DataRow("4", "<AAv<AA>>^A")]
		[DataRow("5", "v<<A>^AA>A")]
		[DataRow("6", "<AA>A")]
		[DataRow("7", "<AAAv<AA>>^A")]
		[DataRow("8", "v<<A>^AAA>A")]
		[DataRow("9", "<AAA>A")]
		//Example
		[DataRow("029A", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A")]
		[DataRow("980A", "<AAA>Av<<A>>^A<vAAA>^AvA^A")]
		[DataRow("179A", "<Av<AA>>^A<AA>AvAA^A<vAAA>^A")]
		[DataRow("456A", "<AAv<AA>>^AvA^AvA^A<vAA>^A")]
		[DataRow("379A", "<A>Av<<AA>^AA>AvAA^A<vAAA>^A")]
		public void Press_Directional(string input, string expected)
		{
			var keys = input.Select(KeyTypeExtensions.GetKeyType).ToArray();

			var robot = Robot1.Create(2);
			var moves = robot.GetMoves(keys);

			var moveStrings = moves.Select(key => key.GetChar()).ToArray();
			var result = string.Concat(moveStrings);
			Assert.AreEqual(expected, result);
		}

		[TestMethod()]
		[DataRow("A", "A")]
		[DataRow("0", "<vA<AA>>^AvAA<^A>A")]
		[DataRow("1", "v<<A>>^A<vA<A>>^AAvAA<^A>A")]
		[DataRow("2", "<vA<AA>>^AvA<^A>AvA^A")]
		[DataRow("3", "v<<A>>^AvA^A")]
		[DataRow("4", "v<<A>>^AA<vA<A>>^AAvAA<^A>A")]
		[DataRow("5", "<vA<AA>>^AvA<^A>AAvA^A")]
		[DataRow("6", "v<<A>>^AAvA^A")]
		[DataRow("7", "v<<A>>^AAA<vA<A>>^AAvAA<^A>A")]
		[DataRow("8", "<vA<AA>>^AvA<^A>AAAvA^A")]
		//Example
		[DataRow("029A", "<vA<AA>>^AvAA<^A>Av<<A>>^AvA^A<vA>^Av<<A>^A>AAvA^Av<<A>A>^AAAvA<^A>A")]
		[DataRow("980A", "v<<A>>^AAAvA^A<vA<AA>>^AvAA<^A>Av<<A>A>^AAAvA<^A>A<vA>^A<A>A")]
		[DataRow("179A", "v<<A>>^A<vA<A>>^AAvAA<^A>Av<<A>>^AAvA^A<vA>^AA<A>Av<<A>A>^AAAvA<^A>A")]
		[DataRow("456A", "v<<A>>^AA<vA<A>>^AAvAA<^A>A<vA>^A<A>A<vA>^A<A>Av<<A>A>^AAvA<^A>A")]
		[DataRow("379A", "v<<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>Av<<A>A>^AAAvA<^A>A")]
		public void Press_Directional2(string input, string expected)
		{
			var keys = input.Select(KeyTypeExtensions.GetKeyType).ToArray();

			var robot = Robot1.Create(3);
			var moves = robot.GetMoves(keys);

			var moveStrings = moves.Select(key => key.GetChar()).ToArray();
			var result = string.Concat(moveStrings);
			Assert.AreEqual(expected, result);
		}

		[TestMethod()]
		[DataRow("029A", "<vA<AA>>^AvAA<^A>Av<<A>>^AvA^A<vA>^Av<<A>^A>AAvA^Av<<A>A>^AAAvA<^A>A")]
		[DataRow("980A", "v<<A>>^AAAvA^A<vA<AA>>^AvAA<^A>Av<<A>A>^AAAvA<^A>A<vA>^A<A>A")]
		[DataRow("179A", "v<<A>>^A<vA<A>>^AAvAA<^A>Av<<A>>^AAvA^A<vA>^AA<A>Av<<A>A>^AAAvA<^A>A")]
		[DataRow("456A", "v<<A>>^AA<vA<A>>^AAvAA<^A>A<vA>^A<A>A<vA>^A<A>Av<<A>A>^AAvA<^A>A")]
		[DataRow("379A", "v<<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>Av<<A>A>^AAAvA<^A>A")]
		public void Press_Example(string input, string expected)
		{
			var keys = input.Select(KeyTypeExtensions.GetKeyType).ToArray();

			var robot1 = Robot1.Create(3);
			var moves1 = robot1.GetMoves(keys);

			var moveStrings = moves1.Select(key => key.GetChar()).ToArray();
			var result1 = string.Concat(moveStrings);
			Assert.AreEqual(expected, result1);
		}
	}
}