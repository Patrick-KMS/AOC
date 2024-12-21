using AOC2024.DaySolvers.Day19;
using System.Diagnostics;

namespace AOC2024Tests.DaySolvers.Day19
{
	[TestClass()]
	public class Day19SolverTests
	{
		private readonly Day19Solver solver;

		public Day19SolverTests()
		{
			solver = new();
		}

		[TestMethod()]
		[DataRow("brwrr", 1)]
		[DataRow("bggr", 1)]
		[DataRow("gbbr", 1)]
		[DataRow("rrbgbr", 1)]
		[DataRow("ubwu", 0)]
		[DataRow("bwurrg", 1)]
		[DataRow("brgr", 1)]
		[DataRow("bbrgwb", 0)]
		[DataRow("wrwubwgbbwgrrrugwwruburrrbwgrrruwwbggrbugbbwgbrb", 0)]
		[DataRow("uuuuguurgubrburwwwuruuruuuwurbbrwrwgrrbwrbbbggbbbuggrubgrrgw", 0)]
		[DataRow("wwbrrurwwbgubrgbwgrbgubrrgwrubgrbugrurrrwgrgwwbwwrgw", 0)]
		[DataRow("wbubrwwrgrgrrggrwubwgubbgrggugurwbggggbuugwrbguwwrbgbrugur", 0)]
		[DataRow("bwuurrg", 1)]
		public void SolvePart1_OneLIne(string pattern, int areSave)
		{
			var input =
				"""
				r, wr, b, g, bwu, rb, gb, br, wuu


				""";

			var sw = new Stopwatch();

			sw.Start();
			var result = solver.SolvePart1(input + pattern);
			sw.Stop();

			Assert.AreEqual(areSave, result);
			Assert.IsTrue(sw.ElapsedMilliseconds <= 50, $"ElapsedMilliseconds: {sw.ElapsedMilliseconds} > {50}");
		}

		[TestMethod()]
		[DataRow("wrwubwgbbwgrrrugwwruburrrbwgrrruwwbggrbugbbwgbrb", 1, 50)]
		[DataRow("uuuuguurgubrburwwwuruuruuuwurbbrwrwgrrbwrbbbggbbbuggrubgrrgw", 0, 100)]
		[DataRow("wwbrrurwwbgubrgbwgrbgubrrgwrubgrbugrurrrwgrgwwbwwrgw", 0, 50)]
		[DataRow("wbubrwwrgrgrrggrwubwgubbgrggugurwbggggbuugwrbguwwrbgbrugur", 1, 100)]
		public void SolvePart1_OneLIneExtended(string pattern, int areSave, int milliseconds)
		{
			var input =
				"""
				bb, rgub, ubub, ugwbg, rwbb, rg, guuwbrw, gur, rrgb, bwgrbgg, ugg, wgu, ugbrbb, bbu, ubgw, guw, gubwu, grwr, rgbb, wrurw, rw, urww, uug, ugwu, ubr, rgwwbbb, urur, rwuwbwrr, gbbbburw, uwruu, rrrr, ubwgbbb, wrwr, uuu, uurubuuw, uwgw, gbbgu, wwbu, wuwr, brrug, bggggb, bgw, rrbr, wwuu, ubu, rggrub, rrgr, uwu, uuwguu, gbbbu, wuubb, rgb, bggwwr, g, gbw, wuu, bug, br, gwg, gbb, wgwrrr, rbu, bwwu, buubruwg, gwbub, bbrbw, wwwgb, wbwgw, www, rrw, uggwb, wubgw, gugrr, gruuwur, rrbgrg, bbwg, bgg, wgg, wuw, uugggrbg, wgrruu, uwuwrbw, wbbr, uuburr, wbg, ggbwurug, uubrwu, urug, gururb, bgbg, rurwg, brr, rbrg, bbubuu, rwrbw, uwgbwurb, wbuwb, bbw, rrrgbgb, ru, ugwru, bggrw, gggwbu, ggwuwu, rru, wrrug, rrrgrub, bwbww, guuu, wub, gu, buwbu, rwwww, wugbuwb, bgrr, uguuu, gbub, bww, grb, bwg, wwb, uurw, urwuu, grrbgu, rwg, rrbwb, bwu, wwug, ubwub, wrbg, wgrw, gww, rww, bbgrb, ub, rggbwu, bbuwbrgw, rb, wb, bwbugg, gurw, bubb, guug, wr, wrwg, uwgwbbr, bubwbrw, wru, rrbrbub, rgrg, rwgbrwb, rwbr, uwwrbbu, uwrbgw, gwub, uwbgr, ruugrrub, rubguggr, buuw, wwu, gbuub, gurg, wrg, bwub, rub, bgu, wbwrbb, ggb, bwbb, wwggbw, wgb, uwr, uwgur, bwb, u, urw, bgb, bbr, uwwu, bg, urr, rgbu, bugwg, rbbr, burb, bgruw, bwuu, rrugrgg, wrwubw, bubbgbg, bguwuru, ubg, ubwu, bwbbgbub, brguu, urwgrbu, guurrw, ubrgr, uubuwr, wrw, uuwr, ugur, ubwuwg, wbrw, wgr, rguuw, rwgwu, rbbgb, ggbgg, rgu, ug, gwrb, rrwgwr, rubuu, ugb, grrbr, grwb, bgugwwur, bbugruu, wbwg, ggu, guwbbw, bgr, grrrgw, brwwbgu, wburb, rgbg, bw, ruwb, brb, wwgbgwr, rbr, grr, ugbb, ruruu, wgbr, rwwbwg, wwg, ubgwu, urru, guuuwu, ururr, buuub, bru, bwrrwubu, wuww, rgbr, gub, wwrbg, grw, grg, wu, burguu, gbug, uggbr, brwub, uwb, grgru, rur, rgugugg, rubrbwg, rbrrw, gwrurgr, bur, uurbwu, ggw, rbwbu, wbw, uuw, ugu, bwr, wwwwru, gbwg, gbu, rgrwb, urbuw, rugwr, uwbbgrw, uurrbugg, wbgbrg, ubb, urb, burrb, gbgu, rwu, bbrruwb, uurrru, bugbw, b, ugguuw, uwgugwb, rug, wug, bwurgr, wgwubu, rgr, ugwb, gwwwgu, gwwbgggw, bwug, rbrug, uurb, ruw, wwwrb, brw, wggbubu, ugbrr, bbrgg, rruw, ggr, gbbw, wguwug, rbb, wuugbwgg, gug, rbuguu, gwb, ubrr, burrrb, rrwrrrg, ubw, rwgrrru, wugb, gggbbwg, uur, bu, urg, bgrb, brgrb, rbbwu, rgruu, gugruurr, bgbwb, rrbg, ruub, bgggrb, rugr, ggwb, uw, brrgr, wwugbrr, uruwub, wg, rwbugu, gru, bbrurr, rgbug, rrr, burw, gwgrubw, ww, wrburbb, rwwrwbu, uu, uww, urrw, wwwrbu, ugbugr, bbbb, wbuu, gbg, rrwuwb, rwgugg, ggrg, bub, gwr, rrwbw, rwr, rgg, r, guu, rrg, uggbbggr, wggbbb, wbuuwub, buwbw, ugrgu, gbbrr, gubgguu, bubwbg, ububw, uruw, wbr, gbwgrg, guwu, bwwur, rr, bggr, gg, rrwu, wrb, ruu, wbgb, ruwbwbuw, wbb, rwuug, rrb, ugw, bwbwuruu, ur, rbbbrrr, rgwg, gbr, grrb, wurr, uruwbr, burbww, ugrguw, wgw, wrgwug, buu, wbu, gubw, rgbgu, gbrrgg, buwb, wwr, bgwwwr, bbbug, rwwg, uuwgwbw, uwg, buw, gbbu, bbbw, bgrru, gwbu, ubwg, brg, wrr, ggg, uru, ugr, rwrww, bwgbbr, bwuw, brbgrrg, brrr, bwrb, gbgggu, gr, brubw, rwb, rbgr, bbb, gwu, wur


				""";

			var sw = new Stopwatch();

			sw.Start();
			var result = solver.SolvePart1(input + pattern);
			sw.Stop();

			Assert.AreEqual(areSave, result);
			Assert.IsTrue(sw.ElapsedMilliseconds <= milliseconds, $"ElapsedMilliseconds: {sw.ElapsedMilliseconds} > {milliseconds}");
		}

		[TestMethod()]
		public void SolvePart1_Example()
		{
			var input =
				"""
				r, wr, b, g, bwu, rb, gb, br

				brwrr
				bggr
				gbbr
				rrbgbr
				ubwu
				bwurrg
				brgr
				bbrgwb
				""";

			var result = solver.SolvePart1(input);

			Assert.AreEqual(6, result);
		}

		[TestMethod()]
		[DataRow(1, 1, 50)]
		[DataRow(2, 1, 100)]
		[DataRow(3, 1, 200)]
		[DataRow(5, 2, 500)]
		[DataRow(10, 5, 1000)]
		public void SolvePart1_Input_First(int take, int expected, int milliseconds)
		{
			var input = solver.GetInput();
			var inputFirst10 = string.Join(Environment.NewLine, input.Split(Environment.NewLine).Take(2 + take));

			var sw = new Stopwatch();

			sw.Start();
			var result = solver.SolvePart1(inputFirst10);
			sw.Stop();

			Assert.AreEqual(expected, result);
			Assert.IsTrue(sw.ElapsedMilliseconds <= milliseconds, $"ElapsedMilliseconds: {sw.ElapsedMilliseconds} > {milliseconds}");
		}

		[TestMethod()]
		public void SolvePart1_Input()
		{
			var input = solver.GetInput();

			var result = solver.SolvePart1(input);

			Assert.AreEqual(283, result);
		}

		[Ignore("Not ready")]
		[TestMethod()]
		[DataRow("brwrr", 1)]
		[DataRow("bggr", 1)]
		[DataRow("gbbr", 1)]
		[DataRow("rrbgbr", 1)]
		[DataRow("ubwu", 0)]
		[DataRow("bwurrg", 1)]
		[DataRow("brgr", 1)]
		[DataRow("bbrgwb", 0)]
		public void SolvePart2_OneLIne(string pattern, int areSave)
		{
			var input =
				"""
				r, wr, b, g, bwu, rb, gb, br


				""";

			var result = solver.SolvePart2(input + pattern);

			Assert.AreEqual(areSave, result);
		}

		[Ignore("Not ready")]
		[TestMethod()]
		public void SolvePart2_Example()
		{
			var input =
				"""
				r, wr, b, g, bwu, rb, gb, br

				brwrr
				bggr
				gbbr
				rrbgbr
				ubwu
				bwurrg
				brgr
				bbrgwb
				""";

			var result = solver.SolvePart2(input);

			Assert.AreEqual(6, result);
		}

		[Ignore("Not ready")]
		[TestMethod()]
		public void SolvePart2_Input()
		{
			var input = solver.GetInput();

			var result = solver.SolvePart2(input);

			Assert.AreEqual(413, result);
		}
	}
}