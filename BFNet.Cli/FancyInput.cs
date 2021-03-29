using System;
using System.Collections.Generic;
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
			
			int LineLength() => input.ToString().Split('\n')[currentY].Length;
			int LineCount()      => input.ToString().Split('\n').Length;

			while (true)
			{
				var key = Console.ReadKey();

				if (key.Key                            == ConsoleKey.Enter
				 && previousInputs.LastOrDefault().Key == ConsoleKey.Enter) break;

				// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
				switch (key.Key)
				{
					case ConsoleKey.RightArrow:
						if (currentX < LineLength())
						{
							currentX++;
							Console.CursorLeft = Math.Min(currentX, LineLength());
						}
						else
						{
							currentX           = 0;
							Console.CursorLeft = Math.Min(currentX, LineLength());
							Console.CursorTop++;
							currentY++;
						}

						break;

					case ConsoleKey.LeftArrow:
						if (currentX > 0)
						{
							currentX--;
							Console.CursorLeft = Math.Min(currentX, LineLength());
						}
						else
						{
							currentY--;
							Console.CursorTop--;
							currentX           = LineLength() - 1;
							Console.CursorLeft = Math.Min(currentX, LineLength());
						}

						break;

					case ConsoleKey.DownArrow:
						if (currentY < LineCount())
						{
							currentY++;
							Console.CursorTop++;
							Console.CursorLeft = Math.Min(currentX, LineLength());
						}

						break;
					case ConsoleKey.Backspace:
						input.Remove(input.Length - 1, 1);
						if (LineLength() > 0)
						{
							currentX--;
						}
						else
						{
							currentY--;
							Console.CursorTop--;
							currentX = LineLength() - 1;
						}
						Console.CursorLeft = Math.Min(currentX, LineLength());
						break;
					default:
						input.Append(key.KeyChar);
						currentX++;
						Console.CursorLeft = Math.Min(currentX, LineLength());
						break;
				}
				previousInputs.Add(key);
			}

			lineCount = LineCount();
			return input.ToString();
		}
	}
}