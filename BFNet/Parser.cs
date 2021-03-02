using System;
using System.Linq;
using System.Text;

namespace BFNet
{
	public static class Parser
	{
		public static HierarchyRoot ParseFullHierarchy(this string input)
		{
			throw new NotImplementedException();
		}
		public static HierarchyObject ParseSubsection(this string input)
		{
			throw new NotImplementedException();
		}
		
		public static string RemoveComments(this string input, char? lineCommentChar)
		{
			var working = new StringBuilder();
			foreach (var line in input.FixLineEndings().Split('\n'))
			{
				if (lineCommentChar.HasValue && line.StartsWith(lineCommentChar.Value)) continue;
				foreach (var character in line
				   .Where(character => Instruction.InstructionLookupTable
												  .Select(i => i.Key)
												  .Contains(character)))
					working.Append(character);
			}

			return working.ToString();
		}

		public static string FixLineEndings(this string input) => input.Replace("\r\n", "\n").Replace('\r', '\n');
	}
}