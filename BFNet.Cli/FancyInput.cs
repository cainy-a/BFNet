using System;
using System.Collections.Generic;
using System.Linq;

namespace BFNet.Cli
{
	internal static class FancyInput
	{
		public static string GetInput(out int lineCount)
		{
			IList<string> lines         = new List<string>();
			var           lastLineEmpty = false;
			while (true)
			{
				// get line from user
				var line = Console.ReadLine();

				// add to list
				lines.Add(line);

				// if two consecutive empty lines exit
				if (!string.IsNullOrWhiteSpace(line)) continue;
				if (lastLineEmpty) break;
				lastLineEmpty = true;
			}

			lineCount = lines.Count;
			
			return lines.Aggregate(string.Empty,                  // start with nothing
								   (current, next)                // get the current value and next
									   => current + '\n' + next); // add each new line separated by \n
		}
	}
}