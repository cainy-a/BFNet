using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BFNet.Cli
{
	internal static class FancyInput
	{
		public static string GetInput(out int lineCount)
		{
			var showCursor = true;
			try
			{
				showCursor = Console.CursorVisible;
			}
			catch
			{
				// this only works on windows.
			}
			//Console.CursorVisible = false;

			var                   input          = new StringBuilder();
			IList<ConsoleKeyInfo> previousInputs = new List<ConsoleKeyInfo>();

			var currentX = 0;
			var currentY = 0;

			string[] Lines()      => input.ToString().Split('\n');
			int      LineLength() => Lines()[currentY].Length;
			int      LineCount()  => Lines().Length;

			while (true)
			{
				var key = Console.ReadKey();

				if (key.Key == ConsoleKey.Enter && previousInputs.LastOrDefault().Key == ConsoleKey.Enter) break;

				// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
				switch (key.Key)
				{
					case ConsoleKey.RightArrow:
						if (currentX      + 1 < LineLength()) currentX++;
						else if (currentY + 1 < LineCount())
						{
							currentX = 0;
							currentY++;
							Console.CursorTop++;
						}

						break;

					case ConsoleKey.LeftArrow:
						if (currentX      > 0) currentX--;
						else if (currentY > 0)
						{
							currentY--;
							Console.CursorTop--;
							currentX = LineLength() - 1;
						}

						break;

					case ConsoleKey.DownArrow:
						if (currentY < LineCount())
						{
							currentY++;
							Console.CursorTop++;
						}

						break;

					case ConsoleKey.Backspace:
						input.RemoveLast();
						if (LineLength() > 0)
							currentX--;
						else
						{
							currentY--;
							Console.CursorTop--;
							currentX = LineLength() - 1;
						}

						break;

					case ConsoleKey.Enter:
						input.Append('\n');
						currentX = 0;
						currentY++;
						Console.CursorTop++;
						break;
					default:
						input.Append(key.KeyChar);
						currentX++;
						break;
				}

				previousInputs.Add(key);

				Clear(LineCount());
				ReRender(input, currentX, currentY);
			}


			lineCount             = LineCount();
			Console.CursorVisible = showCursor;
			return input.ToString();
		}
		
		private static void ReRender(StringBuilder input, int currentX, int currentY)
		{
			var lines = input.ToString().Split('\n');

			var currentIndex = 0;
			for (var i = 0; i + 1 < currentY; i++)
			{
				currentIndex += lines[currentY].Length // the line
							  + 1;                     // the newline char
			}

			var text = lines.Select(l => l + ' ').Aggregate(string.Empty, (current, next) => current + next + '\n');

			currentIndex += currentX; // account for x pos

			var textBefore  = text[new Range(0, currentIndex)];
			var currentChar = text[currentIndex];
			var textAfter   = string.Empty;
			if (text.Length - 1 > currentIndex + 1)
				textAfter = text[new Range(currentIndex + 1, text.Length - 1)];

			// we need these later
			var fgCol = Console.ForegroundColor;
			var bgCol = Console.BackgroundColor;

			// start text
			Console.Write(textBefore);
			// current char
			Console.BackgroundColor = fgCol;
			Console.ForegroundColor = bgCol;
			Console.Write(currentChar);
			// text after
			Console.BackgroundColor = bgCol;
			Console.ForegroundColor = fgCol;
			Console.Write(textAfter);
		}

		private static void Clear(int lines)
		{
			Console.CursorTop  -= lines - 1;
			Console.CursorLeft =  0;

			for (var i = 0; i < lines; i++)
			{
				var sb = new StringBuilder();
				for (var j = 0; j < Console.BufferWidth; j++) sb.Append(' ');
				Console.Write(sb.ToString());
			}

			Console.CursorTop  -= lines;
			Console.CursorLeft =  0;
		}
	}
}