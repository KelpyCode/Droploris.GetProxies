using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Droploris.GetProxies
{
	public static class CUI
	{
		public static void CursorToLine(int y) => Console.SetCursorPosition(0, y);

		public static int PercentageWidth(int perc, int offset = 0)
		{
			return (int)(perc / 10 * (Console.WindowWidth - offset / 10));
		}

		public static int PercentageHeight(int perc, int offset = 0)
		{
			return (int)(perc / 10 * (Console.WindowHeight - offset / 10));
		}

		public static void ReplaceLine(int y, string t)
		{
			CursorToLine(y);
			Console.Write(t);
			for(int i = t.Length; i < Console.WindowWidth; i++)
				Console.Write(" ");
		}

		public static void ClearLine(int y) => ReplaceLine(y, string.Empty);
		

	}
}
