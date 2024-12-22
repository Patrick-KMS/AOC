namespace AOC2024.DaySolvers.Day21
{
	internal class Key
	{
		private readonly Dictionary<KeyType, IReadOnlyCollection<KeyType>> _moves;

		public Key((int,int) position, KeyType type)
		{
			Postition = position;
			Typ = type;
			_moves = new();
		}

		public (int,int) Postition { get; }
		public KeyType Typ { get; }

		public void AddMove(KeyType keyType, IReadOnlyCollection<KeyType> moves)
		{
			_moves[keyType] = moves;
		}

		public IReadOnlyCollection<KeyType> GetMoves(KeyType keyType)
		{
			return _moves[keyType];
		}
	}
}
