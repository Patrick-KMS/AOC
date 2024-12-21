﻿namespace AOC2024.DaySolvers.Day19;

public class Day19Solver : IDaySolver
{
	public int Day => 19;
	public string Title => "Linen Layout";

	public long SolvePart1(string input)
	{
		var inputLines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
		var aviableTowels = inputLines[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
		var optimizedAviableTowels = OptimizeAviableTowels(aviableTowels);

		var validDesigns = inputLines[1..].Select(design => Validate(design, optimizedAviableTowels)).ToList();

		return validDesigns.Count(designs => designs);
	}

	private static string[] OptimizeAviableTowels(string[] aviableTowels)
	{
		var towelsByLength = aviableTowels.GroupBy(towel => towel.Length).OrderBy(group => group.Key).ToList();
		var optimizedTowels = towelsByLength[0].ToList();
		foreach (var group in towelsByLength[1..])
		{
			var currentOptimizedTowels = optimizedTowels.ToArray();

			foreach (var towel in group)
			{
				if (!Validate(towel, currentOptimizedTowels))
				{
					optimizedTowels.Add(towel);

				}
			}
		}

		return [.. optimizedTowels];
	}

	private static bool Validate(string design, string[] aviableTowels)
	{
		var x = aviableTowels
			.Where(design.StartsWith)
			.OrderByDescending(towel => towel.Length)
			.ToArray();

		var remainingDesign = design;
		var aviableTowelsStack = new Stack<string>(x);

		var remainingTowelsStack = new Stack<Stack<string>>();
		var selectedTowelStack = new Stack<string>();
		while (remainingDesign.Length > 0)
		{
			if (aviableTowelsStack.Count == 0)
			{
				if (selectedTowelStack.Count == 0)
				{
					return false;
				}
				var oldTowel = selectedTowelStack.Pop();
				remainingDesign = $"{oldTowel}{remainingDesign}";
				aviableTowelsStack = remainingTowelsStack.Pop();
				continue;
			}

			var towel = aviableTowelsStack.Pop();

			if (remainingDesign.StartsWith(towel))
			{
				selectedTowelStack.Push(towel);
				remainingDesign = remainingDesign[towel.Length..];
				remainingTowelsStack.Push(aviableTowelsStack);
				var xy = aviableTowels
					.Where(towel => remainingDesign.StartsWith(towel))
					.OrderByDescending(towel => towel.Length)
					.ToArray();
				aviableTowelsStack = new Stack<string>(xy);
			}
		}

		return true;
	}

	public long SolvePart2(string input)
	{
		return SolvePart1(input);
	}


	public string GetInput()
	{
		var input =
			"""
			bb, rgub, ubub, ugwbg, rwbb, rg, guuwbrw, gur, rrgb, bwgrbgg, ugg, wgu, ugbrbb, bbu, ubgw, guw, gubwu, grwr, rgbb, wrurw, rw, urww, uug, ugwu, ubr, rgwwbbb, urur, rwuwbwrr, gbbbburw, uwruu, rrrr, ubwgbbb, wrwr, uuu, uurubuuw, uwgw, gbbgu, wwbu, wuwr, brrug, bggggb, bgw, rrbr, wwuu, ubu, rggrub, rrgr, uwu, uuwguu, gbbbu, wuubb, rgb, bggwwr, g, gbw, wuu, bug, br, gwg, gbb, wgwrrr, rbu, bwwu, buubruwg, gwbub, bbrbw, wwwgb, wbwgw, www, rrw, uggwb, wubgw, gugrr, gruuwur, rrbgrg, bbwg, bgg, wgg, wuw, uugggrbg, wgrruu, uwuwrbw, wbbr, uuburr, wbg, ggbwurug, uubrwu, urug, gururb, bgbg, rurwg, brr, rbrg, bbubuu, rwrbw, uwgbwurb, wbuwb, bbw, rrrgbgb, ru, ugwru, bggrw, gggwbu, ggwuwu, rru, wrrug, rrrgrub, bwbww, guuu, wub, gu, buwbu, rwwww, wugbuwb, bgrr, uguuu, gbub, bww, grb, bwg, wwb, uurw, urwuu, grrbgu, rwg, rrbwb, bwu, wwug, ubwub, wrbg, wgrw, gww, rww, bbgrb, ub, rggbwu, bbuwbrgw, rb, wb, bwbugg, gurw, bubb, guug, wr, wrwg, uwgwbbr, bubwbrw, wru, rrbrbub, rgrg, rwgbrwb, rwbr, uwwrbbu, uwrbgw, gwub, uwbgr, ruugrrub, rubguggr, buuw, wwu, gbuub, gurg, wrg, bwub, rub, bgu, wbwrbb, ggb, bwbb, wwggbw, wgb, uwr, uwgur, bwb, u, urw, bgb, bbr, uwwu, bg, urr, rgbu, bugwg, rbbr, burb, bgruw, bwuu, rrugrgg, wrwubw, bubbgbg, bguwuru, ubg, ubwu, bwbbgbub, brguu, urwgrbu, guurrw, ubrgr, uubuwr, wrw, uuwr, ugur, ubwuwg, wbrw, wgr, rguuw, rwgwu, rbbgb, ggbgg, rgu, ug, gwrb, rrwgwr, rubuu, ugb, grrbr, grwb, bgugwwur, bbugruu, wbwg, ggu, guwbbw, bgr, grrrgw, brwwbgu, wburb, rgbg, bw, ruwb, brb, wwgbgwr, rbr, grr, ugbb, ruruu, wgbr, rwwbwg, wwg, ubgwu, urru, guuuwu, ururr, buuub, bru, bwrrwubu, wuww, rgbr, gub, wwrbg, grw, grg, wu, burguu, gbug, uggbr, brwub, uwb, grgru, rur, rgugugg, rubrbwg, rbrrw, gwrurgr, bur, uurbwu, ggw, rbwbu, wbw, uuw, ugu, bwr, wwwwru, gbwg, gbu, rgrwb, urbuw, rugwr, uwbbgrw, uurrbugg, wbgbrg, ubb, urb, burrb, gbgu, rwu, bbrruwb, uurrru, bugbw, b, ugguuw, uwgugwb, rug, wug, bwurgr, wgwubu, rgr, ugwb, gwwwgu, gwwbgggw, bwug, rbrug, uurb, ruw, wwwrb, brw, wggbubu, ugbrr, bbrgg, rruw, ggr, gbbw, wguwug, rbb, wuugbwgg, gug, rbuguu, gwb, ubrr, burrrb, rrwrrrg, ubw, rwgrrru, wugb, gggbbwg, uur, bu, urg, bgrb, brgrb, rbbwu, rgruu, gugruurr, bgbwb, rrbg, ruub, bgggrb, rugr, ggwb, uw, brrgr, wwugbrr, uruwub, wg, rwbugu, gru, bbrurr, rgbug, rrr, burw, gwgrubw, ww, wrburbb, rwwrwbu, uu, uww, urrw, wwwrbu, ugbugr, bbbb, wbuu, gbg, rrwuwb, rwgugg, ggrg, bub, gwr, rrwbw, rwr, rgg, r, guu, rrg, uggbbggr, wggbbb, wbuuwub, buwbw, ugrgu, gbbrr, gubgguu, bubwbg, ububw, uruw, wbr, gbwgrg, guwu, bwwur, rr, bggr, gg, rrwu, wrb, ruu, wbgb, ruwbwbuw, wbb, rwuug, rrb, ugw, bwbwuruu, ur, rbbbrrr, rgwg, gbr, grrb, wurr, uruwbr, burbww, ugrguw, wgw, wrgwug, buu, wbu, gubw, rgbgu, gbrrgg, buwb, wwr, bgwwwr, bbbug, rwwg, uuwgwbw, uwg, buw, gbbu, bbbw, bgrru, gwbu, ubwg, brg, wrr, ggg, uru, ugr, rwrww, bwgbbr, bwuw, brbgrrg, brrr, bwrb, gbgggu, gr, brubw, rwb, rbgr, bbb, gwu, wur

			wrwubwgbbwgrrrugwwruburrrbwgrrruwwbggrbugbbwgbrb
			uuuuguurgubrburwwwuruuruuuwurbbrwrwgrrbwrbbbggbbbuggrubgrrgw
			wwbrrurwwbgubrgbwgrbgubrrgwrubgrbugrurrrwgrgwwbwwrgw
			wbubrwwrgrgrrggrwubwgubbgrggugurwbggggbuugwrbguwwrbgbrugur
			ubuwbrgbgurgbbgggwwuubugggwbrbrgbrbgbbwuwbrgw
			bbgwwrgbgubuurwwbrugwrurubgbgrwggwuwurwgubbgwggburrwbbbrgw
			rgugrugrggwbwgrgbwrwrurrrrwwwuwrbwugwugrrrrgrbbgwbgrggggu
			wbgggwwubwubbuwuwrubuwurrwwgruwgruuuuugugwuwuguurrrwwbrr
			gurbbrrrgwubrugwwwwrgbggbbwuuwwrugbwwwugugbwwgwrrugbbwrgwg
			urubbubgwgwwubugwugrbgggbrrwugbrwrwubbwrggburgw
			gbgbugurbrwgrbgbugwwbubuwuwwwurbubbgggrrwrrg
			ubwgbuggrugrwuwuruggbgbbrguuwgurbbwbwgguuuburguburwguruggb
			bgwurwrwruugbugwuugrbguurwrrrgbugugugwbuubgrggbbgwwgur
			wbrurugwurwbuggrrrbrgbrgbuuuggugguurubuuwgwgrrwuwgwwbrgb
			wwbbugugbwrrwwrurwubgwwuggugbwbrgrgugggwuwrrwrbug
			bruwwguuwggururggwbbrbbgbgubgwuuwbggbugwrbbbr
			rurwbbgugwugubgwruwbwgbuuwrggwbgbgrgrbbbwrrwububu
			bbugwburrwwwgrrgwrbggwgbrbruburrbgwrggrggbrrbuburgw
			bugggwgbguubuuwgrguubwbrwgbuwbbrguruggwrrbggrugrr
			ruwgburubwwbguubgurugbbguwuugbwwbwbuwbuwbbruuguggr
			wggggbgbwgwwbrgrrbgrgbugwurrwuwwruwrgwugrbrruugbrrrbuur
			guugrbwrwururwbrbruguwgrrbbuwwurwguruguuwggbbwrg
			uubwrwrwuguggbubrgggwrbwgubrugbuurguggrwbgruurr
			ubuuwwwrburgbbgbbuugrrbbgubgugrwguurrbuggwwuwuwrrgg
			rurbuurrrwrwwrwuwwbgwuwburgruubbbggwwrwggru
			wrwwwuwrubgwbgwgwubbgggbubbrrrbwrurbgbrwuwrrr
			wbggrgbrggwwwwubrbgurrwwgburrubbwwgrgbrggwwb
			ruwbwgwuwgbwbgrrrwwbubgrwggbwgrruwwubgwwgbrgbrrubbgw
			wruuubrguwrwrbwubbgrbrwgrugwbrgwgwgurwruwgbrbgbruwrruwb
			rwurbwrrugurgwbuubbwwwbwuwrwrggruurgwruruubugwwrgw
			uwugbwguuwbwbgurwrbrbwwrrrgbwbuguubgurrwwbwgggurrurubrgw
			ugwwrrwugwurwurgwububgruwggruwuwrgbgrrurgwgwu
			uurwgbwwurrbrbwrgburruwrbuwbuurrbuwrwgguwrbububguruwrgbru
			grguggbbuubbrgwggbgrbugrwgwurgbgrwwbbgwubb
			bgrgbgwburgbbbugrgbbbwrbgurruuuwggwrwuggrburgwwuuurgwruwwu
			urbwwuwugwrgrwwwwubrgbgbgggurbrwrrrurwgrwwrbwggrrb
			bwuurwrbuuuguruuubbwuwggrbbwbrbgwbrwrurubuu
			wgrubbwwwrrggurwburwrwrrbgrrwbwubbrwrgurrrgw
			wububwbbrggwwggwrrbrbbbgrguwruugbgruwrgrwbubgru
			uguugrbwrburgruuwwuuggruwbwurwwbbwgrgugwbwb
			ubbrgbrwwwrwbrrbggggbbrrbububwuuggrrwggwrgbwrbwwubur
			bgrwgrgbbuuwgwwwbugrgwburrrwgbubwbwbrbbgrgw
			burugwruruuwbwrrwgggugrbgrguggwgurubwubgwwgwub
			bguwubgwgbrwbubuuburgguwurbrrgwwruugugrrgrbrgw
			uwgrurrburbbwbgwuwgbwurubwrbbwrbbrwgwbruwgwrgwrrgwrgrgw
			rwrrbrbrwwbguwrwwwbbrgwruwuwwbrwgwbwbrwbgwbrwwwrgrw
			gwrburwurgbgrbrrrubbuguwurrbwggwwrbbgbburburrbrgw
			buwrwgwgwuurrbrrugwugrburwrgrbbwgrbwbrrugbuwrbrrgw
			brrbrbbbrbubwwbwbgwrruubburuubggwuruuguuguuburgb
			bwugrrgwurbgbgwgwgbubrgbgurbwgubbgurwbgbuwgubgwwu
			uguggwrwubbgwuuwbbuurbbgugwrrrgurgwrbrbbwggruu
			buwbwbwrugubwrgwrrrwugwgugrruuwwbrgbugwwbggrgw
			wwwurugrggruuuuugbuururrwwbrrgbwuwwrwurubgwwgrubguggr
			bwwrwwbbggrrwbugwwrgbugbubgwbbgburuwbwbuwuwubrgrbwgu
			wgrbbgwwwwugurbrbrurrugwurrgrrrrbuuuwwuurgrurgw
			ggggbuubwbubgwgubrugbgwrgwuwbwbwburwwwwwwbubr
			wuubgrrbbbubuuwgugwbbugbuubrwubbruwurgbbuug
			buubrguuguwgwbrwrurwugbwrubrubgbgrurwwuuwbgrgw
			gwugbrrugrwrrggbwbbbwrwgwwwrurwgwwwuububrrburgbuwuwbrbruug
			gwrbrubrbbrggbwubrrgwgbuguwbubugrgwbbbwbrururgb
			bbubwbbwuwwwguwgwbbbbwurgwbrrwwbruubuggwubbubrguu
			grbbubugrgubbgwgwrbrwbrrgugrrbbwrwuwgurbwwb
			ggwubwwgbwbbwwbbrgwwburwuggugbgggrruwubrgbbrgbuwbb
			rgbguwwbbggrruwbrubbrbubbburgbbwbwurrwgubwb
			uwrbwruwwwrubbgurbguuwwwrwrugrubwbwwwugrguwrgbwb
			gbwuwrugbrrrwwrwrbbrbwrguubbbwgwgbwgbrwbrbubb
			grgrwgrbbbgbrrugrgrwugrgwuurrwrwbwuwwwbbrbrwwgbgugwwur
			wugrbrugubbbwgguwgbbgbbrrgwrggrgggbbbgggwuuurruurwrgw
			wburgrubgggwwgrubgggrgbuuwrggrrbbgurwurgwrgbwwruggrugwrgw
			wuubwrguwrrguggruwurgugwwrgbbugwbwuubggbgubwrwuwru
			wuubgbwrugbbbgwuubuururggrwwgrwuwrrggbwrbrbgbrwur
			wrbgugwguuurwbgrrrrbgugruurrgrrbrrwbrgrgw
			bbbrwrgwuggubwrurgurubgrugruuwurgrruggrgw
			gurbguruugwwgwgbugguwrbbgbubbrwurbwrwgrgggbugrubrrgbbgubg
			uwurrgwbrrbuuwgrbbbbbburwuugrbggbwurugwgbuurgw
			wwrrbruuburbugrguugrbrrbrwwuurwuruugwuwwbruwububuwbub
			rbrbgbwbuwrgwguwgurrbbrbbgruwgbuwggrwwbgbwbrbwbur
			rwgbwrrbguwwwgwrgrguubggguguuburbbbbuwrrgw
			wubggwbrggbubuuubgrbgggugwbruwbrrruggrrwrwuugu
			grrugrbbrgggwbugwwwwbbwgwbbuwuwuwubgbgurrrubguw
			grrwuuwrbbgbbguuwuwrgrrburrwggrwrubgbwgruuwrbbur
			rwbwruuwurrbuwrwbwurwgubbgubwbrubrbgbwwburrwrrgw
			ggrgwuuwburrgbbgwgrrgbgguwbburbrugwwbrgbgrbwwbugurbrgw
			uwgrbgguggwubuwwwgrrwubuwruuurgbwggbuggrrubwr
			rruwbrwgwrgbwwuugwuwuuugwgwwbbgbggwwwwwguguggu
			guwgrrubggubrguubuggrwrgrbgwbuuugbrggruurubgurgrwuururb
			gbrbrrbgwgwbgbwwubwwbrgbgurguugbbugrbwugrruugrrubwuguu
			wbugwgwuruurwbguwggggbguwubbgguwgguwuugrwwuguurrbr
			urbgwwgrrwburuwbuwbbbwgrgrrbgrwgrrurrbbbrrwruuw
			bwgruggrwbwgrwwwugrrbuwbrrwuwwuwbbrggrbruuwwbbuggwbwgwbwww
			rrbubwwbrgguggruruwuubbgbuwwwgwrbwuwuwgwrgugrwugbbrggu
			grbwwrbgbrrgbrrwgwgugbwgrbwwbruwwugbggggwbgwbrgbwgwgr
			gbwbrbgrbgurwwbrbbrwbbbugburbwubgrrwbgrgggrgubrrrrbw
			uuwrruwgwwbuwggwbrurbuguububrbbrrguuwggrgw
			uwwugbbbburwgrrbgwuuubwgwrwrwbwwgwuwrbuuuwbbubwb
			rbuuwwrggbwugbwbgrguubgwrbbgbwgrrwgggburwugubgrrr
			ubwrwgbbbbgrbrrrwrurwgbrrbgwuuuuggbwgbrgw
			bwrwgbuuwurwrbwgwwgburwgbbbbuurrrbbubbugwgwrgg
			wbruwuwwwuggbgwuubbwbugggurubgrgbgurrwwwrrbwwbwbrgbbgrrr
			bwgbgwgbwbgrbggurwwuwbrgbgrwrwurwubbggrbgwgurww
			uwbrgrwwggrggugrwggwgbggrrgggbgwugwrrrwgbgrwbgubu
			wwuwggugubwuwuuugrguruurguwbrgburuwbuuuwbwguwrubwbrbuuuwr
			ubbrwgrbbguggwwwburwwurrbbrubwwggubbbbbugggruruurgr
			guwrrrwgbrgrururuuwbrbwgrubrbuburrgruubguu
			bwguwrggubrwwbbbbubggbrbbwwwrbrwuwwgrwuggrgrburugr
			ggrrwggrwbrgbwgbgbubbrwgugwbrwbgbbgurbburbburrbbgguuubugrr
			bggrgwrbrggbggurbrwugbrgwwwrbguguwugguwbgwuuggugbggbrb
			uubgbrgwgwubbrgugwwubrugurrgggwubbbrrrrgrguwwgurub
			buuggwgruwgbgwwbugwuwwrrurgrwubrwwubrwgbwwuubwrbrrrb
			grgruwuwgugrwwwrugwggruurrrwgugrbuguwugbuguwwwrgw
			urrbwgruubwbwggbgrbggbubbbbrrgwuwguuuurwgwrr
			ubgbwrwrrgbgurwrgwugruuuuwwgrurwwbwwbbgugurgw
			guburbgwgurbuwrbbrbwgubgguwwbuubwwwgwbrgbbwb
			brwwrwbwwbggbbrgrbbbbwugugwbbwwubbgwugugrgg
			grugrguwbbwubwrggwuububwurwbwggguwwbubgwbuwwrurrwgr
			uggrwuggwbrbggwbbrurruwuwgggrwwgwbbrubugggwbwwwggbw
			uwguwgwwbrubgwbbwwrggrbrrrggugbgbwbruubwwuwrrwbguwuwbuu
			rugbbbuuwrwggbwrbugrwbwggwgggrruwrguugwugbugwuuburubw
			uuguwguwwubrggrgggurbbgwuwuguggwwrguguwurgbuwuuwwgbwbgubwg
			rguwugwugwwurggwugruwgrbwwrguggubrrugbbubgbr
			rrgrggrbrburrurguwrbggwrbuwgbwurbwbgubgurbbubwuwwwgrrgbbrgw
			uwgwwwwurrgrbgguwuwbbwwgggbwburgbguuwrurgw
			wrugwrwrubuwbwrubwgggggrgbgubggwubwgurruwgruwwugugbubugub
			uwwbggbwbbugwgrwuggwgrbgwrrurrbbwbbgrgbrugwwwwgbu
			bgwbuugrgugbubbggbggbwbrguwuwwwbwuggrrguwbbbwgbbbwuguwgbru
			brugbruwugrrbuggubugruwgburguuwrwwrwgwrgrwrwrubrbgb
			urrgwugbuubbgwbwwuwggruwuuwbgrbwgbgbrubrurbwbugggww
			gurbgubbubrugubuwugwrwwrwbgggrrurrwuuugwuuwggwggrgwrgrggbb
			guwwwguwgwggbbrggubuurubuwugbwbbuugugubwwrugurgw
			uuwbbrwgggwuubwwgbwbwbwugwggwwubgrugrgbgbwugbubbwgwggubr
			rgbwugbrrbbguuwbbgruubgururrwwgbwgubbgrrbbugu
			wugbgbugburuwgrgwgwubrrbrrguuwwrbubgbrwwuwbugbub
			wurwgrgrruggbbubbgurbwgwrrgwurbubgggrubrurrgw
			wbbgrrbwbrurgrubrgruubggrgrwbbgbrggwgwrgruwugrrrgrwuwrwu
			bbruwuuubggrrgugwubrbgubguwwbbrrrbrgguwgwugrurrurr
			gwbwrbwrwbwbguwbwgrrgbubwgugbrrrbwurwrruwuuuruwrgw
			wbwrbgbwrwbwbrgrugbgrwrurbrgbguwbrgubbwwbgwuwrgwruwwuwgur
			ggubugggurruugwgwuwbgrugrruuwgrbuguruugrwwugwwburbwbr
			uurrruugggrbgrurrgwubbgbbguubbggwuuruuuguurwww
			bggrrrggbwgbubuggrgrubugbububuubruwggububwgubbwwwrbwuwugu
			uwbwrbbwbbubgwwrugwuururbggggbrbbbbruwubrgurwrggbr
			rguwgruwururwrggbwwuubbguguwuruugrubgrrbbgwbuwggwgwbgb
			gwubwbbruugggbgrwgwrgwbrwuwugbwgwgbbwuuwrrbbwuwbugr
			uuwwbbbrwugwubrwurubbwuggwbrgbwwrgbrbrburgggrrurbgg
			ubrbwwbburgubrwrwuwgbuuwwwgguggwbgrrbrurrgggrwwu
			wwgbgrrwggwbuwwugwbuwwurwuwbwrrbrbwwgruwburgwrrwubrbrgw
			wbuwurbggrwbwrrbubwbwuruubrwwrgbbuurwrrbbgrwbbgrrgwugw
			ugurbrbggrrbgbubbruuruuurugbwwbbwgguburuuwr
			rugrbbbgurgubgwbbwwuugggrbggwbbbuurbgrwwbrgw
			brbgwugbbwrbgubwbggwrgwgugurwgrugubwguggbgwrruub
			ubrgwgggwuwrruwuurggwwuururbbwgruwgrrbubgbwgwbbubugrgw
			wwrbbwuugrwubggggrwuuugwwwgugubrggrbbbwugbwggwrgw
			wgwrrbwbrbbuuuuuurgugwggwugbbuggwbrgwgrgurgw
			grwgbrbwuuwubbbggrubggwbbggrgrrbrbwguburgwgbwbwuruuugw
			gwwrgbgwgrubguubwgwurwuwbgbbrugrrgrbbrrgw
			rwwrrubgrwururrgugwgrwugwbbrbgwgrbwrbggrgw
			rbuuwruwgbrwgburguwwrbubuuwbbubgrggrugbwwb
			wgwwwrwwbwrwuwugbuguuwrwgbuubrgwwugggbrbbrwwgrwbuub
			wubgrbgrrgruggugrgrbggwgruwwubrrggbrrgubwrgw
			ubugubgguuwrrgbgrbguurbgrguwwburuwubuuwuuwgrburrgurwwrbrb
			bgwwrggrwrwbgbrrrgggwguwwugrbwrwrrrgwbububbwgwgbu
			bwbwbguuwuuuwugbuurubrbbgggggburbrurbrrwuw
			brbugguuwbwwgurgrgugrggrgrbbgurbbbuggwbrugbgbuwrwgrgwwuugw
			wwrwwggbubrrwbrgugwgguwgwggbwrrbwruuuuwrugggrwbrrbgw
			bbrurwbbgbrwbgbwgbubgbwbbgrgrgwrburgbgugwgrrwgbg
			gbrwbwrwwuwguwrurrubbbgbbrbrwbwurwgwuggrbguwbbrr
			ubwgrrubuwuwwwbguwgbwwggbgbrrbbuwwwuwrgrrwuwrbbgrrrbbbwrgw
			uwgrwguuubgwurrbwgrwgwwbwggbbrruubgrrrubrgbrwurrbuuugrug
			uubwgwubuuwrwbbbuurrgwurwwbwrwgbbuburgwuwwgugubg
			ggwrrgwgwruwgrbwwwrurgrwwuggrubwrbbrrwbrgwggb
			bwrgwgbbrwwuburbubwgbgbrwguuuwrrrbrgbugwrbubbuwwgw
			wbrbbrubgwbbwuwrwrubrubwurgbruurgrgwgubuuurgwbwgrrggburgw
			brbbgbgwbruuwwguggwrbrurbrrguwugrbwugbwbrbrwburrb
			bggrguugwrrbrgurgbbgwbrrgbrgbrbgrugbggwurw
			ugwbrwggrwugwubguggubgbwwwrbguwbrwgbubrwrwrbbwrrw
			bgurwwbgbbguurgugbrubuurubrbgwwggwurgbbuubgrug
			wwwugrubbrwubwgwgwwrrubbgubwwwbwbwwrrwrrrgugbrrurrb
			wurggwugrubrbgwwbgggwrgubwuwrbgbguuburuugbbb
			gbgwuubrwuggruguwgwgbrgrburubwgbbwwbrrrwwbwwbgbgrgw
			rguubrbrburrwbgrrbruwwwgrrggggrrgbbrurbgggrrbgwggrrrbu
			buugggrwgwbrbrwwwuwbbrburububrwbwbgrubgrrgbbrrgw
			wwwubugwrrrwwrbwwbgwwrbwrburguwggbuuwbbwwuuurgw
			ubrrbwuwgburubgbgrwuuwrwgrrrgrbrubbuugbgbrgw
			wbwrbgrrguggbuwggbubwugwbwbuugrwrrrgrgwwrbgg
			bbbbwwwubuuwuuwurrrguurrwgwwbrwgubwwgbguugbwgrwrbbgur
			wgbggbgwubwwuuwrrwbuugurbbrubwwgrwuurugwwrwwuubbwru
			guurgrubbgrrrgwrbuuwbbwbrwgrurwbwgbgrwbuwggwrrruuuggbrg
			gbrgwrwuugugrgubbubruuubruwguurbuwuwguuggw
			wwgguwbbubgwbwbwuwwugrrgbbururgbrbrrwrgbrbgrbbuuuw
			urgrgrbuugwrwbuubrrwwwubggugwwbbggwgrwwwrwrwrgwubbbuguuurgw
			uwgrgbrugrgwbwuwuruwurwrgrbwububugwwggrgbbbbggbg
			guwuuurrgurruubwwbbgubgwrrbgwuwguurwgbugurbrgrgggguubuu
			guubbruugbrgwgrbbbbwrgwbbgwgwuuwrgggbbrbrubbbwuwuwr
			urbrbubbrbwwguwrgwgrgrurwbgurbbrgguurwgruwg
			ubggbbrwggbgbwrbwubggwbbbuwuruuurrgbbwguwwwrgggwgr
			bugwbrgbrrrrbrwbbuwbburgwggrurugrggbbwwgrgbb
			gugwwuugbugwwbbbwwbbuwwurbuuubbbrubrwgwrgburubgwugb
			gwbruwgbwrwbgggwrwggggbrggbrurbwruuuuuurwggbbguurrrbgbrb
			grwgwgwrrbwgugrurwrrurggwbwrrbugwgbrrwbgwwuwurbbbrburg
			uuubguwubbbbbrbrbbgrggbwguurrwgbgrrubrbwubwbrurwrbubwwwwg
			burgwwwruggbrggwgbuwggguuururgguwgwrbugubggurggbuuuu
			uggrbbgwbrbrwugwugwuwwwwbrwbuurrbrbgrrwurrburuwbwbuwgrwurgw
			urwugrugbbubwrgbgbwbgrguuurgrwrwrwbbrwbrgur
			bwgurgugubbwwgrbgbuurwbbbbgruwubruwrwbuubbbuwwrrguwwg
			uuwugrwugurugbuwwwrwgggbrruguurwgbubgrbburrb
			gburggguwwurrwwwubuuurbwbgbruwrgwwbubbuugbwbbwwbrgwgg
			wgggrbbuwwrggbwbrrwuwwubwgguwugwrwwggbrwruubwwbwrrwbgrggw
			rwurwrrrrbggrbbwbruwuuurbuwgbruwbrwbrrgruwruwgwbgwggbrbrug
			uwgwruuwgrgubuwuuwbbrgugrwgugrrrwurwwbwrwwr
			bwuubbgrwbruubruwgbrbgbrwguubbbrbbgwgruwgguwbgrgbrrgw
			ruugguuuwwbbbgbrruwuwwurwuugwgrwwwrrrrbbwbrbbubggw
			rwuugrwbwggwbwgbubbbrugbrwwrbggrrwrwuwrrrgw
			uubrguruwurbbbrbbwwgbwwburugbrbwwurrubgrrubr
			gbwrgbrbggwrurbururubggrbwbwwbugrrbwuwguwrwbbbwguugb
			rrbgbbrwrrgrurruwrgrwguugwbgbbguurwrgwbrubrgbgwbwwuuwuburb
			wuuubrbgwwwwbrbrrbrwbuwruwgbwurbugrrwrwrrubbug
			urrbrgrgurgwgrbggrguurbbbugbbguguwwrrbbrrgw
			rubgbgrurgbrbbbwgwurbggburbuwwwggwbwgrwbubrwrgw
			bgburugrgrurbrguwbbwruggwrubrbbwrgbbrggbubuuwgr
			ubwrrwububwgwwbburruggwwbbubrwgbbuuuguggubrgw
			wrrurgbgwrgrbbugugbuwrrubgbrugrugbubgrrwggbwrgwrrwg
			wruugwbwurbrrbuguwwgwgwbwuubbbugwubbbwwruwgbbgurgubr
			wggguuubwwugurbrrwuwbgrbuwugwgrrbwrwbrrwggubbbbgrgbrrrrrgw
			ggwbwbbuwgwgubbrubbgwgguubrwgrgbgbrguugwgwrwburbruuurbgbgb
			rguugwwwwrbgwrbuwggbrbwuwwwgugrwuggwgggbggbguwggrgurwwrg
			ugggbgbbbbbgggwbubwbguwgwgwgrggbbbwgrwbrubuwgurgw
			ubguugbuwwbubwbbwurgwbgruuwrgrbrggbbguggwgurrrwwurwuwrg
			wuubggrgbugwbbwgbgggwwbwugggggwwbwbuuwwguwgwubg
			brruwrrurgwrgbrrbbwwbrwbguuruwrugwgbwruwguu
			ruubbrgwrgwwubwwruurgrwwbguwrubugbwbuwwwgwrbbrrrrgbubwurrgw
			bgwrbgbrgrgrbrrgwurrurwurugubwwrwwubbbgwgbbbrwgbw
			wwbuwwubwrruwururwubbrrugwwbbgrbrwuwgbbrruugrwrbgwgbbwww
			uwubwwuwubgurwgbgbwbbgbubbgurwwubbrbuugrrbrbgwrub
			bbruruugrrwrwurwwguurgurbgruruwbuubwwgggwwgugrrgbrgw
			bbwbrwwrwrrgggwugugguggbbggrwuruubgwrgrubrb
			burrwbuggwwgrrgbwugwbwuubrgwbgubburguugrgrrugugggbrbugbgrgw
			rrrurwgbbbwbwgrrbbwguwbwrgrrwguwbbwrgbbrugugbggb
			rwgbguwburuwruwwgbuggbbggrrrggrubugwbrgwbubgguwbuwugrgw
			wubrwugwwbgggwwrrbrugguwwgguubgwrbrrwbgruururbuwurgw
			bwbuwrgruuuwbbbrbgbbbbugbggbwwrwbgburuuugbwubruubgbbr
			bgggbwrbrrbbbgwrguwbbwbbwgwbgwruwwubrwwbwwwrgrrgw
			uwrrwwrbwrwbbuurgwbwuugrwurwwuwrbggbrbwurrubgg
			uubbggguwuwgubgbgbggwwgwwrrrggwbubuwurgbuggbbrubrgw
			ggbrurwrgwbuuurrbuggwwgubrrwggbuuggrbbuwbrbbrrgw
			wgwwwguwbwbrbwgurbwwbwggbrrbrugrgwrbuuwbuwrrrrrgwugu
			rbbbrgbgwuuwgwgrrgrbuwbwuwuurbrgwrbwrrburrbwurbuwuu
			brrwbgbgbrrguwruuuuugwwwrrgbbwuguwrrwbrrugrurrgub
			wrgwbrubururrugrgbwrubbguwrgggruwgbbubrwuwuguggrrrrgw
			uuggrugbrwuwbwrrrgbwbgurgbbgbrrugrwbbgbwuwgbwbgbuguub
			rgwwwwgwbrrubuwuuubbbbbgurugggwwubrrbbugrwbuuu
			ubwruguuubgrwbugbwuwrwgwwgrbrbgbugwgurwggurgrgw
			bwwrbbbrgwwbbuuwgwggurggbruwbugwwubuurrugwrrubbu
			buurgwrgrwrgbrrbrrrrrggrrgrwbuwwurwbggggrbbbb
			brwggggrbgwgwgbuwbbrruwrbgrbgwbrwgrruggurwubguwbbu
			wwwbbbwbgubrrwgggwwubgwuwuwgruugwburwrrwbbbrwbgbgwbbbggrub
			bgwgurwuwuwgwgggrgwwggwubuuggrbrgubrgbggwbbgbrbwgurbr
			rggrgrgugwuruugbrbwwbbggbuubwrbgwwgugguguwwwguwrgrrggg
			rburuugubuwrubuwgwwguwbrbbrguwuurbgwrgbgwubgrbbgr
			wbwwrrbwwwgugbwwguubwbuguugbwwgbruguugbwbbwbgbrwwbw
			uguggrrbrgbubwbgugurwgubbburuwggrurguwgburuurgw
			uubggwrbubbuuuwbgurbwrwgugwugbrrbuurwgwugwwrbubruwwg
			brwrgbgbrrwbrrbuwrbuuurgurbbrbrrrwbgwbrwgrwwbw
			wuwbwwuwuwgrubbubguurrruuggrbguwbrugrbgbwbrgw
			uruuuwbuwbwbrgwuuugburbwrbbuuuwbuugburwwgbgwgurgwrgw
			uwgwurwrwwwgrwbwrbgrgbwrbwbwubwgrbbugwugwwubrgrubgggwwwbbr
			rruuuwrgrrbruburrrbwurrwrwbwbbbrubwugubugwurgwuwwubbwb
			urwrwwbgbrrgurrrugubruubwuwburbbwguggurwwuruggbbwrguugbbg
			guurbgbrbbuwburggbbrbbggbubbrbwbwgrgwwwugrugrugggwrgw
			gwbrguuugurwbuwbruwuurbrggwuwwugggwrrrrwbbrrwrrgw
			rggbbgrrwgggrgrbuuwugwurwwbrbwwuwgbwguwwgubrbwr
			rrwrwgbbrugrrwwbubbrwrbugbrrbbggbgruurgw
			rgruurwuuwgbbugbgwgruwrubuuubuwggggbbuuwgubrwwugbuggbr
			bwbbgbubggwggbrguwwgrguuuuugrbruwrgrrgbrrubuugurrwrrgw
			ruubrugrrbrwbrugugrbrbgbbbuwrwwubruuwrgwrrg
			ggugwwbrwgbwuggggwrrrguwrwbrubuwbrwbwuwrbwgrwuwu
			uuwbruburwwwubwwwuwbrburbrrgwuggugugbgwugwburbbggubw
			brrrurbuuwgrgrrubuwbgwbbbwrbbrrgwrwurrrrrwu
			brbgrbrgguggrbbwrrbrbggbbrguguurubuuwbbwwwburrgrgw
			brbbwggrwrwwuguwwggubrubbwgwwwbbgggwwbwwbrg
			wugrwbbwrgrubwbuwgbgbgbwuugggrrgbbggbwwbgrwbbrgrrgu
			gbbwwwruubggrrubugbrbrbgbrwugwgugbgrwbwurubbbwuwrgw
			grgwwgrurrgggbwbwrgbrwwrurbrrwrwbggggurrwbwwurug
			ugwuuburgbrbbrwwgrurwrwubwgugbwugrrburgguwubbuwgg
			rrbrburwwwwubrbgugruurrrwgububrubuwguwwububrurb
			brbbrubrbbbwrggbwguuuurbbwrrwwuuguguwugrbuwrgbgugg
			bbgwbgruuruuburgwwrrrruuugrgrwguwbruubbrrbrrwwrbrbgwgrgr
			uwwrrurrugwuugugwbbrgbrbguwubbwbrrwrrgwugrwwgbgwggggbgu
			wgruuwuubwrwggbbuggbbruubgwbgurbuggubbbwgbruguugrrgw
			gbggrgrgwbugrurrrgbbwwwrwuwuuuggrgrwbrrbrwrggubb
			gubgguguuwrrrrubrwwurrrwguwgugrburubrrgrbgrubrbb
			wwugruwbrubbrruuggrbgrugruuuwrubuubbubrgw
			grwruwrbubgrbrwbguububwurgugguuwwwbubwwwwrgw
			rbwgrgurruurgrwrrrgugurwbbbgbwwuwrrbgwrbbwuwruubbwb
			gubrggrrbbgugwuwwgbbbbgrbgbbrgugrbbubwugggrwwrwu
			grbgbbwuwwuwwwrugbbrwugrbbrwwbbbwrwrgggrwgrubbggwrgrwubrwu
			gbgububrrgbrubwuwrrugbwwrbwrwgbwgrrggbrububrguwrrgwrur
			brugwguuwwuugrrbuugruwuugbgwrugbwrrgwgguurbuubwurrgbrgr
			wbbwuurggbgrrugrwuwuwwuwuuwrbuurrbgwbgbbwwburrgrgubbbwbwb
			uuuwrrbwbgrwburrwrrwwguwgbwrgwgbuuruurgbugwbuwr
			rrgubuwuggggbrugrwgrbrwbgggubgrububugruwwubbwgwrbwwrg
			wgbwbubbgguggrbugrgwbgwbbwubgrrubwwbgrrgbwbwbugrb
			wuwgbggrruwgwrgwgrggrrbwbubrrrgwgrrwurubrgw
			gwgwrbbubrbgrwwwbrruuuwgwbbwurbggbbguburruguwuggwguwbg
			uuwggbwrbuurrrbwwugggwuwwgrrrgwbwbwwggbugwbwrgwuurggubw
			ubruwwrwbwguguuurwrugbrrubbwggbbbburwrururggwgbrgw
			wuuwbugbuggbwurgwbbbwggwbguububrrgwrbgwrubruubwburwuguwrrgw
			uwbgwuwbugbbbwguwrubrbbrbbwggwgrgwbrggbrubwugrb
			gwwbrbgurruwbbwwrrurgburbgbgururrrgwuwwguuubrburbggwwrrgrr
			wrrwugbwuruwbwgrwuuwgbrruuubwugwururuwwguggrbggubbgggwb
			bbrbguugbwrrrggggbruwuguurwuuuwuurggwwrbwwwwgrbbbuwurwrbu
			wguwbwuwrbbrbbgrgbwuuburwbgrwuugbwgguwrrgggbgubbbgwgrwwurg
			ruguuwwwbbbbwgrruwurrgurubgguugbbbgwbwbgugggwggwggwuwwrgbrgw
			urbgugbgwrwuruwbruwbwuguggrwgbbrrugwuwubbg
			rrururuwubgbrgbbrwgrbgbwugubrwgwugrwrurgggwbgugwbb
			wurubwggrwuwgwbwggrguwbwgrwuwuwrbbbrgwugubrwgr
			grwrubggbbggrrgrwrbgrwuggbubgrwbgggwwwgubb
			bgwuguuurrbggugubguurbubwgbwrwbgrrwrbbrgw
			rwwwguwggurrrrugrwurubgurrrrgrbruuuwwwurgw
			ruurbubbgwrbbgbgrubururbwuuwubwruuurrggbwrb
			ugrwrurbugrrrrrbbuurrrurrguuwbruuwgwbwrrrgw
			bbwgrwwgwwgburwgubwbrruwrrrrrrguugwuwwuubr
			gwbgwbgbrbrruugbgrwggbwwbgubrgrbruurwwrrbuubwg
			bgrgruwubbugrbrguwguuwbbuwgrbgggbgbruuwrwwgw
			rwugwbbbugwbrbrgbugbrbrubwrrbgrggrguuuuuugrb
			bgbbrurubgwbwruwubrbbwrgrgwbwwugbrwrrbrgbgbuwbugwgbug
			buwubrwrrgrwwrbruubrgubbgwwwbbwruwgggbrgwbwwrbrgw
			wubwbrrbbwbrbggwwgbguubggggbwgbwgbrrgbbgbbguw
			bgrwuggrwgrbgugwubgwrgugwrwrggurbbgbwgbbruuwgu
			bwwuubwwwrgrrwwurgrugrurwggurwggrbrbrguguww
			gugbgubuwggrbuuwwwbbrguburubrwrwbuwugbwrrruubwgrwbrwrgw
			brwwurbburrbbwwwwbbwubbubrgbwrbwbgrbwgurgw
			urrgwugbwrbbwurrwgbggbbbrgbrgwbbugurwrgwbbwugrgw
			rbwrrggwwruggrugbguwwgbuuwwgrrrbgwruwrgrruugwrgu
			brbubuuwwuwwrbugwbruruburbwuurrbgbwgwwuruuwgwuubgguubgbb
			wgurubgwubrbwwbbbbbwggrgrrguggwguwwuwbrgw
			gugurrguwuwbgwbruwgbrguwrgubwuwubuubrubrwwugrbbuwr
			uurrbrbbgrwgbbbbwguuubwwrwrgrggwwrwwwuuubwbuwbggww
			guugwuwrwrguuururggrwwbwuuwubrbrrwuwrbbwrrrgw
			bbuuubwrrgwwbwbugburubbwrrbrrwgrburbbrbwwgburbgbwrbggbu
			wgrgwurrurgwwbugwbrwuwuuwgbwurbgugrwrwwgbwbwwgrr
			bugwgguwbgbwwuugbwgguubgbgrgrbggrgrrrguwruruwwrrrgw
			bgbubrwurgrgbruuwwrubrrgruububruruubrbgruuwubgwgwgubbrgw
			buuwwrwrrwwubuurugrubugwbbgbrurbrggbggrgwwrwbgguru
			gbrwgbwuwubbbbbuwbrgwuubgrwubgbgggggwrrwrwbgu
			ruwurrwrwuggbbuwbrgwgggggwbubwgggugwwwuwgrrwbwrubgrgugrgw
			bwwwrbubuwgugurbgrgbugbugwbbbgrbbuuruwbrguugwuwb
			gwbgggrbbrgrwrbrgbgurbguwbugrrguwwwugugubugbugrrrguw
			wbrbwrbrggwrurrurwuggwggrgubrrrgbrugrrruwgggr
			gurbrrwwuwbgbuwrgbrgrbbuuwbguwgwrrrgwrrgbbugbrwggb
			uwrwggwwbgwrwwugrwbbggbrwburwrrwrwbbwwrwru
			wurrwbbrrbuugubrbbbgugbuubruwgbgbwwurubrrgwbrrrrrrrugrgw
			gugrwrwburuuubgbbwurwgwrbrrwwwugwgwbgubrguruwrbw
			ururwwgwbrgwwwwrrgwbwrbburbrrgbbugbbbguuwgr
			wbrwrgwugrubbuubrgrgruurgurrbguuuwrgubbgbwurwgugrwubwg
			buwguurrugrggbwruwgugrwuruwrbbgbgrggwgbwrrgur
			wubguggwgbgrgbuwwuwrrwbbgrbwrrgurwuurwurgbubrgwururuwrgburgw
			bbwugwwggurrrbbrwubgggwwrrbuuwgurgggggugwurrr
			wwggbbwbuwwurrrguwrburgwbggbgwuwruwuugrbuubggwbgrgwbwru
			rbururwbbwrruwguruguugggrgwbwuggubwbrwuwrbgruggrrg
			rrgrbwruuuuwgrgguwgwrrbuuwrwrgwbwbwwgbbguuugbbbugu
			brurrbrrwbrruggbuwbrbwwurgwwuwwbgurggurbbrbwgbrwurgw
			rururbrggbwurrbuwgwrbbwrbwrwbubgwwurrrggbwuruggrug
			brbrrgrggrbuuuugrggwubwgrwrwgbbbbgbwwuuubbgbwrwru
			rwbgrgbrbwbbgbburruuwbrrgbuwuwuwbrwgubugrgw
			grrrwuggguuguguwwuguwgbwbgwrbbguurgbbbrrbrrgggr
			uggbururgrggrrguwwbrrgbwbrubgrbwrrwurggbbbuubw
			bbgbbguwguwbbwrubgwgwugwuurwuubburbrgruubbgwwgugrur
			grwrbrwbwuggrugbruuurrburbwrrwwgrruurbrwbrgw
			brruwguwbrruruwbwgrwwwggwrrwrgggwrbugggguwubbwburwubbb
			brggugwgwwuurgrbwguuwubbrgwrwuugurwgwuubrwwbwwrbrgw
			wrrbwwwggurwgwurbgwbuwgwubrugbrgwrbrgwubwgrwwbubrbbwubrgw
			ggbuwrwuwbbrbwwrbuburrwuuwrggubgwruuwwgrgbuurwrgw
			bbwgrugwrugrrwbwgurugbrrbgwbuwrgggrwrbwurb
			uwwrgbuwuwgrrurubuwrbugbrwugbbruwbwwwrbbrgubrgbruubwgwrbrrgw
			bbrbwrbbrbgrgwurbgrurubbgrwbwugggbwgbbgurgurggrurrrgw
			wwubbuuwbwwbbgbrrwwwgwuwurbwburrwbbrwrrgwrrg
			ubrrggbuuwwbwwwrbbrrbrbwbuurwwbbwwgrrwgbrbburbruwbgruuwgbrgw
			gwgbubrguwrbgbugubrbwrgrgurbbwbwrugbbbuggbwgr
			wurwbrwbubwgugbgbugwuwbrgbbbrrbbbrgbuurgwubwbbrgw
			bgbggwbrbrwbggbrwgbwgwbbrubggubbwrwwbgrububgwbbu
			gggwwwgwwbuuwbbburwwuwgugwrbbrurubgrwwrgwugw
			ubrggbburuuwwwrggbwubwrgrbruburugbrggbrwgrrbwrg
			rgrrggrbwurrbrrrrbwbgbgwugrgrwwrububguurbubbugbgwbwb
			rgbbbgbuugwbuwrwrgbbwwbgwwbbubgwgrwrgrbuggrwbrbbubrwgruurgw
			bburrwbrwgrbrruguubwrbbrgbwubugbwgurrbwrugwwwbbwwuwrugg
			rwugbuguwugrbwubwubgrgwbbrrwwbgguubggwurrrrgbgwrgbbbbub
			uwbwurwwbuwggrrrgbgugwwurgrrbubrwwgurgbbwurgurgwgwuurgw
			uguurwgbwwrwggbuugggbgrgbgrwrgugurwrrrrbbuuwbgubu
			rgrbrrbgwgwuwrwwgwrbguwrrrrugwwrwrwgwuwgwwbbb
			grubrgwwuurgwbggrurwugrubruuubbgrrrrbbbrbrgggrbrwugubr
			rrbgwuwgrububrrrgruuuurrgwwurrruugrrubwggbgggrgw
			uggbwuugrurbrurbwruwbwbwwubrbwguugwgwrurwwwggubrgw
			uuurrbrgbwbwuuwububrwrbwwubwgugggbwwgggrbwrwrgw
			guwbbuuwbgwwubwuruuruwuuuuuguwrrrurrbuwuuubbgub
			wbbgruwbgrrrugugwuwwrgbrbbrrgwubguwuburbrwubbwgrguwrgw
			gbrugurgwbuuurgwrggwgbwurrubguggrbrrrgggwgwrbuugwgbwuwbwrrgw
			bgggwwuwguwgwgbbwwuububgrugggrrwgubuurrgw
			buwrrgbwubwrrburuwburwrggbrubgrburgbwwuugg
			rgwggbwrbwbuurwwrwuwrwgrurrrbrguwwrrwrbgrgguw
			gurwbruugugbgwgurubgruwugwuwwbguuuubwbgwgbbwu
			""";

		return input;
	}
}