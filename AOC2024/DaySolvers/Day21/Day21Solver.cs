using AOC2024.DaySolvers.Day20;
using System.Text;

namespace AOC2024.DaySolvers.Day21;

public partial class Day21Solver : IDaySolver
{
	public int Day => 21;
	public string Title => "Keypad Conundrum";

	public long SolvePart1(string input)
	{
		var numbers = input.Split(Environment.NewLine).Select(line => int.Parse(line.TrimEnd('A'))).ToArray();
		var numbersKeyTypes = input.Split(Environment.NewLine).Select(GetKeyTypes).ToArray();

		var robot1 = Robot.CreateNumeric();
		var robot2 = Robot.CreateDirectional();
		var robot3 = Robot.CreateDirectional();

		var results = new List<IReadOnlyCollection<KeyType>>();
		foreach (var number in numbersKeyTypes)
		{
			var movesRobot1 = robot1.Press(number);
			var movesRobot2 = robot2.Press(movesRobot1);
			var movesRobot3 = robot3.Press(movesRobot2);
			results.Add(movesRobot3);
		}

		var complexitys = results.Select((moves, i) => GetComplexity(numbers[i],moves));
		var sum = complexitys.Sum();

		return sum;
	}

	private long GetComplexity(int number, IReadOnlyCollection<KeyType> collection)
	{
		var length = collection.Count;
		var complexity = number * length;
		return complexity;
	}

	private IReadOnlyCollection<KeyType> GetKeyTypes(string number)
	{
		var keyTypes = number.Select(GetKeyType).ToArray();
		return keyTypes;
	}

	private KeyType GetKeyType(char number) => number switch
	{
		'0' => KeyType.K0,
		'1' => KeyType.K1,
		'2' => KeyType.K2,
		'3' => KeyType.K3,
		'4' => KeyType.K4,
		'5' => KeyType.K5,
		'6' => KeyType.K6,
		'7' => KeyType.K7,
		'8' => KeyType.K8,
		'9' => KeyType.K9,
		'A' => KeyType.KA,
		_ => throw new ArgumentOutOfRangeException(nameof(number))
	};

	public long SolvePart2(string input)
	{
		var sum = SolvePart1(input);

		return sum;
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
