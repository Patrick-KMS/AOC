namespace AOC2024
{
	public interface IDaySolver
	{
		int Day { get; }
		string Title { get; }

		long SolvePart1(string input);
		long SolvePart2(string input);
		string GetInput();
	}
}