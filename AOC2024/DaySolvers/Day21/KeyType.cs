namespace AOC2024.DaySolvers.Day21
{
	public enum KeyType
	{
		K0, K1, K2, K3, K4, K5, K6, K7, K8, K9, KA, KU, KD, KL, KR, KX
	}

	public static class KeyTypeExtensions
	{
		public static KeyType GetKeyType(this char number)
		{
			var keyChar = number switch
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
				'U' => KeyType.KU,
				'D' => KeyType.KD,
				'R' => KeyType.KR,
				'L' => KeyType.KL,
				'^' => KeyType.KU,
				'v' => KeyType.KD,
				'>' => KeyType.KR,
				'<' => KeyType.KL,
				'X' => KeyType.KX,
				_ => throw new ArgumentOutOfRangeException(nameof(number))
			};

			return keyChar;
		}

		public static char GetChar(this KeyType key)
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
				KeyType.KX => 'X',
				_ => throw new ArgumentOutOfRangeException(nameof(key))
			};

			return keyChar;
		}
	}
}
