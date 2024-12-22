using System.Text;

namespace AOC2024.DaySolvers.Day21
{
	public class Robot
	{
		private readonly Dictionary<KeyType, Key> _directionalKeys;
		private readonly Dictionary<KeyType, Key> _numericalKeys;
		private readonly int _depth;

		private Robot(Dictionary<KeyType, Key> directionalKeys, Dictionary<KeyType, Key> numericalKeys, int depth)
		{
			_directionalKeys = directionalKeys;
			_numericalKeys = numericalKeys;
			_depth = depth;
		}

		public IReadOnlyCollection<KeyType> GetMoves(IReadOnlyCollection<KeyType> input)
		{
			IReadOnlyCollection<KeyType> inputStart = [KeyType.KA, .. input];
			var inputZip = inputStart.Zip(input);
			var moves = inputZip
				.AsParallel()
				.AsOrdered()
				.SelectMany(pair => GetMoves(pair.First, pair.Second, _depth))
				.ToList();

			Log(input, moves);
			return moves;
		}

		private List<KeyType> GetMoves(KeyType fromKeyType, KeyType toKeyType, int depth)
		{
			if (depth <= 0)
			{
				return new List<KeyType> { toKeyType };
			}

			var keys = depth == _depth ? _numericalKeys : _directionalKeys;
			var fromKey = keys[fromKeyType];
			var fromRow = fromKey.Postition.Item1;
			var fromCol = fromKey.Postition.Item2;

			var toKey = keys[toKeyType];
			var toRow = toKey.Postition.Item1;
			var toCol = toKey.Postition.Item2;

			var avoidKey = keys[KeyType.KX];
			var avoidRow = avoidKey.Postition.Item1;
			var avoidCol = avoidKey.Postition.Item2;

			var moves = new List<KeyType>();

			var nVert = Math.Abs(fromRow - toRow);
			var nHoriz = Math.Abs(fromCol - toCol);
			KeyType? moveVert = fromRow < toRow ? KeyType.KD : fromRow > toRow ? KeyType.KU : null;
			KeyType? moveHoriz = fromCol < toCol ? KeyType.KR : fromCol > toCol ? KeyType.KL : null;

			if (moveVert is null && moveHoriz is null)
			{
				var newMoves = GetNullMoves();
				moves.Add(newMoves);
			}

			if (moveVert is not null && moveHoriz is null)
			{
				var newMoves = GetMovesInOrder(nVert, moveVert.Value, depth);
				moves.AddRange(newMoves);
			}

			if (moveVert is null && moveHoriz is not null)
			{
				var newMoves = GetMovesInOrder(nHoriz, moveHoriz.Value, depth);
				moves.AddRange(newMoves);
			}

			if (moveVert is not null && moveHoriz is not null)
			{
				if (fromCol == avoidCol && toRow == avoidRow)
				{
					var newMoves = GetMovesInOrder(nHoriz, moveHoriz.Value, nVert, moveVert.Value, depth);
					moves.AddRange(newMoves);
				}
				else if (fromRow == avoidRow && toCol == avoidCol)
				{
					var newMoves = GetMovesInOrder(nVert, moveVert.Value, nHoriz, moveHoriz.Value, depth);
					moves.AddRange(newMoves);
				}
				else
				{
					var newMovesVertFirst = GetMovesInOrder(nVert, moveVert.Value, nHoriz, moveHoriz.Value, depth);
					var newMovesHorizFirst = GetMovesInOrder(nHoriz, moveHoriz.Value, nVert, moveVert.Value, depth);

					if (newMovesHorizFirst.Count <= newMovesVertFirst.Count)
					{
						moves.AddRange(newMovesHorizFirst);
					}
					else
					{
						moves.AddRange(newMovesVertFirst);
					}
				}
			}

			return moves;
		}

		private static KeyType GetNullMoves()
		{
			return KeyType.KA;
		}

		private List<KeyType> GetMovesInOrder(int nFirst, KeyType movesFirst, int depth)
		{
			var moves = new List<KeyType>();

			var newMovesFirst = GetMoves(KeyType.KA, movesFirst, depth - 1);
			moves.AddRange(newMovesFirst);
			if (nFirst > 1)
			{
				for (int n = 0; n < nFirst - 1; n++)
				{
					var newMovesFirst2 = GetMoves(movesFirst, movesFirst, depth - 1);
					moves.AddRange(newMovesFirst2);
				}
			}

			var newMovesA = GetMoves(movesFirst, KeyType.KA, depth - 1);
			moves.AddRange(newMovesA);

			return moves;
		}

		private List<KeyType> GetMovesInOrder(int nFirst, KeyType movesFirst, int nSecond, KeyType movesSecond, int depth)
		{
			var moves = new List<KeyType>();

			var newMovesFirst = GetMoves(KeyType.KA, movesFirst, depth - 1);
			moves.AddRange(newMovesFirst);
			if (nFirst > 1)
			{
				for (int n = 0; n < nFirst - 1; n++)
				{
					var newMovesFirst2 = GetMoves(movesFirst, movesFirst, depth - 1);
					moves.AddRange(newMovesFirst2);
				}
			}

			var newMovesSecond = GetMoves(movesFirst, movesSecond, depth - 1);
			moves.AddRange(newMovesSecond);
			if (nSecond > 1)
			{
				for (int n = 0; n < nSecond - 1; n++)
				{
					var newMovesSecond2 = GetMoves(movesSecond, movesSecond, depth - 1);
					moves.AddRange(newMovesSecond2);
				}
			}

			var newMovesA = GetMoves(movesSecond, KeyType.KA, depth - 1);
			moves.AddRange(newMovesA);

			return moves;
		}

		public static Robot Create(int depth)
		{
			var numericLayout = new KeyType[,] {
				{ KeyType.K7, KeyType.K8, KeyType.K9 },
				{ KeyType.K4, KeyType.K5, KeyType.K6 },
				{ KeyType.K1, KeyType.K2, KeyType.K3 },
				{ KeyType.KX, KeyType.K0, KeyType.KA }
			};
			Dictionary<KeyType, Key> numericKeys = GetKeys(numericLayout);

			var directionalLayout = new KeyType[,] {
				{ KeyType.KX, KeyType.KU, KeyType.KA },
				{ KeyType.KL, KeyType.KD, KeyType.KR }
			};
			Dictionary<KeyType, Key> directionalKeys = GetKeys(directionalLayout);

			var keypad = new Robot(directionalKeys, numericKeys, depth);
			return keypad;
		}

		private static Dictionary<KeyType, Key> GetKeys(KeyType[,] layout)
		{
			var maxRows = layout.GetLength(0);
			var maxCols = layout.GetLength(1);

			var keys = new Dictionary<KeyType, Key>();
			for (int r = 0; r < maxRows; r++)
			{
				for (int c = 0; c < maxCols; c++)
				{
					var keyType = layout[r, c];
					var key = new Key((r, c), keyType);
					keys[keyType] = key;
				}
			}

			return keys;
		}

		private static void Log(IReadOnlyCollection<KeyType> input, List<KeyType> moves)
		{
			var sb = new StringBuilder();
			foreach (var inputKey in input)
			{
				var keyChar = GetChar(inputKey);
				sb.Append(keyChar);
			}
			sb.Append($" => ({moves.Count}) ");

			foreach (var moveKey in moves)
			{
				var keyChar = GetChar(moveKey);
				sb.Append(keyChar);
			}

			Console.WriteLine(sb.ToString());
		}

		private static char GetChar(KeyType key)
		{
			var keyChar = key switch
			{
				KeyType.K0 => '0',
				KeyType.K1 => '1',
				KeyType.K2 => '2',
				KeyType.K3 => '3',
				KeyType.K4 => '4',
				KeyType.K5 => '5',
				KeyType.K6 => '6',
				KeyType.K7 => '7',
				KeyType.K8 => '8',
				KeyType.K9 => '9',
				KeyType.KA => 'A',
				KeyType.KU => '^',
				KeyType.KD => 'v',
				KeyType.KR => '>',
				KeyType.KL => '<',
				_ => throw new ArgumentOutOfRangeException(nameof(key))
			};

			return keyChar;
		}

	}
}
