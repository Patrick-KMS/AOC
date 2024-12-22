namespace AOC2024.DaySolvers.Day21
{
	internal class Key
	{
		public Key((int, int) position, KeyType type)
		{
			Postition = position;
			Typ = type;
		}

		public (int, int) Postition { get; }
		public KeyType Typ { get; }

	}
}
