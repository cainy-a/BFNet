using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFNet
{
	public static class Parser
	{
		public static HierarchyRoot ParseFullHierarchy(this string input)
		{
			var rawParsed = input.Parse();
			var expanded  = rawParsed.ExpandHierarchy();

			return expanded;
		}

		public static HierarchyRoot Parse(this string input) => new()
		{
			// LINQ go brrr
			Hierarchy = input
					   .Select(character => new Instruction {Operation = Instruction.GetOperation(character)})
					   .Cast<HierarchyObject>()
					   .ToArray()
		};

		private static HierarchyRoot ExpandHierarchy(this HierarchyRoot hierarchy)
		{
			return new()
			{
				Hierarchy = hierarchy.Hierarchy
									 .Select((hierarchyObject, i) => // for each item in the hierarchy
                                          ((Instruction) hierarchyObject).Operation == Operations.StartLoop // is it a loop start? 
															   ? new Loop {HierarchyChildren = ExpandLoopContentRecursive(ref i, ref hierarchy)} // Expand the loop (recursively!)
															   : hierarchyObject).ToArray() // Else just copy it untouched
			};
		}

		private static HierarchyObject[] ExpandLoopContentRecursive(ref int i, ref HierarchyRoot root)
		{
			var loopContent = new List<HierarchyObject>();
			while (true)
			{
				var currentInstruction = (Instruction) root.Hierarchy[++i];

				if (currentInstruction.Operation == Operations.StartLoop)
				{
					loopContent.Add(new Loop
					{
						HierarchyChildren = ExpandLoopContentRecursive(ref i, ref root)
					});
					continue;
				}

				if (currentInstruction.Operation == Operations.EndLoop) break;
				loopContent.Add(currentInstruction);
			}

			return loopContent.ToArray();
		}

		public static string RemoveComments(this string input, char? lineCommentChar)
		{
			var working = new StringBuilder();
			foreach (var line in input.FixLineEndings().Split('\n'))
			{
				if (lineCommentChar.HasValue && line.StartsWith(lineCommentChar.Value)) continue;
				foreach (var character in line.Where(character => Instruction.InstructionLookupTable.Select(i => i.Key)
																			 .Contains(character)))
					working.Append(character);
			}

			return working.ToString();
		}

		public static string FixLineEndings(this string input) => input.Replace("\r\n", "\n").Replace('\r', '\n');
	}
}