using System.Collections.ObjectModel;
using System.Text;

namespace AOC2024.DaySolvers.Day21
{
	public class Robot
	{
		private readonly Dictionary<KeyType, Key> _keys;
		private Key _position;

		private Robot(Dictionary<KeyType, Key> keys)
		{
			_keys = keys;
			_position = _keys[KeyType.KA];
		}

		public IReadOnlyCollection<KeyType> Press(IReadOnlyCollection<KeyType> input)
		{
			var moves = new List<KeyType>();
			foreach (var i in input)
			{
				var m = GetMoves(i);
				moves.AddRange(m);
			}

			Log(input, moves);
			return moves;
		}

		private IReadOnlyCollection<KeyType> GetMoves(KeyType keyType)
		{
			var moves = _position.GetMoves(keyType);
			_position = _keys[keyType];
			return moves;
		}

		public static Robot CreateNumeric()
		{
			var keyPadLayout = new KeyType?[,] {
				{ KeyType.K7, KeyType.K8, KeyType.K9 },
				{ KeyType.K4, KeyType.K5, KeyType.K6 },
				{ KeyType.K1, KeyType.K2, KeyType.K3 },
				{ null, KeyType.K0, KeyType.KA }
			};

			var robot = Create(keyPadLayout);
			foreach (var key in robot._keys.Values)
			{
				AddNumericMoves(key, keyPadLayout);
			}

			return robot;
		}

		public static Robot CreateDirectional()
		{
			var keyPadLayout = new KeyType?[,] {
				{ null, KeyType.KU, KeyType.KA },
				{ KeyType.KL, KeyType.KD, KeyType.KR }
			};

			var robot = Create(keyPadLayout);
			foreach (var key in robot._keys.Values)
			{
				GetDirectionalMoves(key, keyPadLayout);
			}

			return robot;
		}

		private static Robot Create(KeyType?[,] layout)
		{
			var maxRows = layout.GetLength(0);
			var maxCols = layout.GetLength(1);

			var keys = new Dictionary<KeyType, Key>();
			for (int r = 0; r < maxRows; r++)
			{
				for (int c = 0; c < maxCols; c++)
				{
					var keyType = layout[r, c];
					if (keyType is null)
					{
						continue;
					}

					var key = new Key((r, c), keyType.Value);
					keys[keyType.Value] = key;
				}
			}

			var keypad = new Robot(keys);
			return keypad;
		}

		private static void AddNumericMoves(Key key, KeyType?[,] layout)
		{
			var fromRow = key.Postition.Item1;
			var fromCol = key.Postition.Item2;
			var maxRows = layout.GetLength(0);
			var maxCols = layout.GetLength(1);

			for (int toRow = 0; toRow < maxRows; toRow++)
			{
				for (int toCol = 0; toCol < maxCols; toCol++)
				{
					var keyType = layout[toRow, toCol];
					if (keyType is null)
					{
						continue;
					}

					var moves = new Collection<KeyType>();

					KeyType? movesVert = fromRow < toRow ? KeyType.KD : fromRow > toRow ? KeyType.KU : null;
					KeyType? movesHoriz = fromCol < toCol ? KeyType.KR : fromCol > toCol ? KeyType.KL : null;

					if (movesVert is not null && movesHoriz is null)
					{
						AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
					}

					if (movesHoriz is not null && movesVert is null)
					{
						AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
					}

					if (movesHoriz is not null && movesVert is not null)
					{
						if (fromCol == 0)
						{
							AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
							AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
						}
						else if (toCol == 0)
						{
							AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
							AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
						}
						else
						{
							AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
							AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
						}

					}

					moves.Add(KeyType.KA);
					key.AddMove(keyType.Value, moves);
				}
			}
		}

		private static void GetDirectionalMoves(Key key, KeyType?[,] layout)
		{
			var fromRow = key.Postition.Item1;
			var fromCol = key.Postition.Item2;
			var maxRows = layout.GetLength(0);
			var maxCols = layout.GetLength(1);

			for (int toRow = 0; toRow < maxRows; toRow++)
			{
				for (int toCol = 0; toCol < maxCols; toCol++)
				{
					var keyType = layout[toRow, toCol];
					if (keyType is null)
					{
						continue;
					}

					var moves = new Collection<KeyType>();

					KeyType? movesVert = fromRow < toRow ? KeyType.KD : fromRow > toRow ? KeyType.KU : null;
					KeyType? movesHoriz = fromCol < toCol ? KeyType.KR : fromCol > toCol ? KeyType.KL : null;

					if (movesVert is not null && movesHoriz is null)
					{
						AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
					}

					if (movesHoriz is not null && movesVert is null)
					{
						AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
					}

					if (movesHoriz is not null && movesVert is not null)
					{
						if (fromCol == 0)
						{
							AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
							AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
						}
						else if (toCol == 0)
						{
							AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
							AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
						}
						else
						{
							AddMoves(Math.Abs(fromRow - toRow), movesVert.Value, moves);
							AddMoves(Math.Abs(fromCol - toCol), movesHoriz.Value, moves);
						}

					}

					moves.Add(KeyType.KA);
					key.AddMove(keyType.Value, moves);
				}
			}
		}

		private static void AddMoves(int v, KeyType kD, Collection<KeyType> moves)
		{
			for (int i = 0; i < v; i++)
			{
				moves.Add(kD);
			}
		}

		private static void Log(IReadOnlyCollection<KeyType> input, IReadOnlyCollection<KeyType> moves)
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
