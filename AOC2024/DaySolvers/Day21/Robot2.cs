using System.Collections.Concurrent;
using System.Text;

namespace AOC2024.DaySolvers.Day21
{
	public class Robot2
	{
		private readonly Dictionary<KeyType, Key> _directionalKeys;
		private readonly Dictionary<KeyType, Key> _numericalKeys;
		private readonly int _depth;
		private readonly ConcurrentDictionary<(int, KeyType,KeyType), long> _chache;

		private Robot2(Dictionary<KeyType, Key> directionalKeys, Dictionary<KeyType, Key> numericalKeys, int depth)
		{
			_directionalKeys = directionalKeys;
			_numericalKeys = numericalKeys;
			_depth = depth;
			_chache = new();
		}

		public long GetMoves(IReadOnlyCollection<KeyType> input)
		{
			var moves = GetMoves(input, _depth);

			Log(input, moves);
			return moves;
		}

		private long GetMoves(IReadOnlyCollection<KeyType> input, int depth)
		{
			if (depth <= 0)
			{
				return input.Count;
			}

			IReadOnlyCollection<KeyType> inputStart = [KeyType.KA, .. input];
			var inputZip = inputStart.Zip(input);

			var moves = 0L;
			foreach (var (from, to) in inputZip)
			{
				if (_chache.TryGetValue((depth, from, to), out var cached))
				{
					moves += cached;
					continue;
				}

				var paths = GetMoves(from, to, depth == _depth);
				var minPath = paths
					.Select(path => GetMoves(path, depth - 1))
					.Min();

				_chache[(depth, from, to)] = minPath;
				moves += minPath;
			}

			return moves;
		}

		private List<KeyType[]> GetMoves(KeyType fromKeyType, KeyType toKeyType, bool numericalKeys)
		{
			var keys = numericalKeys ? _numericalKeys : _directionalKeys;
			var fromKey = keys[fromKeyType];
			var fromRow = fromKey.Postition.Item1;
			var fromCol = fromKey.Postition.Item2;

			var toKey = keys[toKeyType];
			var toRow = toKey.Postition.Item1;
			var toCol = toKey.Postition.Item2;

			var avoidKey = keys[KeyType.KX];
			var avoidRow = avoidKey.Postition.Item1;
			var avoidCol = avoidKey.Postition.Item2;

			var moves = new List<KeyType[]>();

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
				var newMoves = GetMovesInOrder(nVert, moveVert.Value);
				moves.Add(newMoves);
			}

			if (moveVert is null && moveHoriz is not null)
			{
				var newMoves = GetMovesInOrder(nHoriz, moveHoriz.Value);
				moves.Add(newMoves);
			}

			if (moveVert is not null && moveHoriz is not null)
			{
				if (fromCol == avoidCol && toRow == avoidRow)
				{
					var newMoves = GetMovesInOrder(nHoriz, moveHoriz.Value, nVert, moveVert.Value);
					moves.Add(newMoves);
				}
				else if (fromRow == avoidRow && toCol == avoidCol)
				{
					var newMoves = GetMovesInOrder(nVert, moveVert.Value, nHoriz, moveHoriz.Value);
					moves.Add(newMoves);
				}
				else
				{
					var newMovesHorizFirst = GetMovesInOrder(nHoriz, moveHoriz.Value, nVert, moveVert.Value);
					moves.Add(newMovesHorizFirst);
					var newMovesVertFirst = GetMovesInOrder(nVert, moveVert.Value, nHoriz, moveHoriz.Value);
					moves.Add(newMovesVertFirst);
				}
			}

			return moves;
		}

		private static KeyType[] GetNullMoves()
		{
			return [KeyType.KA];
		}

		private static KeyType[] GetMovesInOrder(int nFirst, KeyType movesFirst)
		{
			var moves = new KeyType[nFirst + 1];
			for (int n = 0; n < nFirst; n++)
			{
				moves[n] = movesFirst;
			}

			moves[^1] = KeyType.KA;
			return moves;
		}

		private static KeyType[] GetMovesInOrder(int nFirst, KeyType movesFirst, int nSecond, KeyType movesSecond)
		{
			var moves = new KeyType[nFirst + nSecond + 1];
			for (int n = 0; n < nFirst; n++)
			{
				moves[n] = movesFirst;
			}
			for (int n = 0; n < nSecond; n++)
			{
				moves[n + nFirst] = movesSecond;
			}

			moves[^1] = KeyType.KA;
			return moves;
		}

		public static Robot2 Create(int depth)
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

			var keypad = new Robot2(directionalKeys, numericKeys, depth);
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

		private static void Log(IReadOnlyCollection<KeyType> input, long lenght)
		{
			var sb = new StringBuilder();
			foreach (var inputKey in input)
			{
				var keyChar = GetChar(inputKey);
				sb.Append(keyChar);
			}
			sb.Append($" => ({lenght})");

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
