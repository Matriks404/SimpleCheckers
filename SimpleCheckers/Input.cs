using System;
using System.Text.RegularExpressions;

namespace SimpleCheckers
{
	public static class Input
	{
		public static bool IsValid(string move)
		{
			string pattern = "^([a-h][1-8]),\\s*([a-h][1-8])$";

			Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

			MatchCollection matches = r.Matches(move);

            if (matches.Count == 1)
			{
				return true;
			}

			return false;
		}

		public static bool IsValid(string move, out int x1, out int y1, out int x2, out int y2)
		{
			x1 = 0;
			y1 = 0;
			x2 = 0;
			y2 = 0;

			string pattern = "^([a-h][1-8]),\\s*([a-h][1-8])$";

			Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

			MatchCollection matches = r.Matches(move);

			if (matches.Count == 1)
			{
				GroupCollection groups = matches[0].Groups;

				char first = Char.ToLower(groups[1].Value[0]);
				char second = Char.ToLower(groups[2].Value[0]);

				x1 = (int)first - 97;
				y1 = (int)groups[1].Value[1] - 48 - 1;

				x2 = (int)second - 97;
				y2 = (int)groups[2].Value[1] - 48 - 1;

				return true;
			}

			return false;
		}
	}
}
