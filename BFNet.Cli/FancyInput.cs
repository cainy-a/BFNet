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
			var     input = new StringBuilder();
			IList<ConsoleKeyInfo> previousInputs = new List<ConsoleKeyInfo>();

			var currentX = 0;
			var currentY = 0;

			string[] Lines() => input.ToString().Split('\n');
			int LineLength() => Lines()[currentY].Length;
			int LineCount()  => Lines().Length;

			while (true)
			{
				var key = Console.ReadKey();

				if (key.Key                            == ConsoleKey.Enter
				 && previousInputs.LastOrDefault().Key == ConsoleKey.Enter) break;

				// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
				switch (key.Key)
				{
					case ConsoleKey.RightArrow:
						if (currentX + 1 < LineLength())
						{
							currentX++;
							Console.Out.Write(Lines()[currentY][currentX]);
						}
						else if (currentY + 1 < LineCount())
						{
							currentX           = 0;
							Console.CursorTop++;
							currentY++;
						}
						break;

					case ConsoleKey.LeftArrow:
						if (currentX > 0)
						{
							currentX--;
							Console.Out.Write(Lines()[currentY][currentX]);
						}
						else if (currentY > 0)
						{
							currentY--;
							Console.CursorTop--;
							currentX           = LineLength() - 1;
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
						input.Remove(input.Length - 1, 1);
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
				Console.CursorLeft = Math.Min(currentX, LineLength());
				previousInputs.Add(key);
			}

			lineCount = LineCount();
			return input.ToString();
		}
	}
}