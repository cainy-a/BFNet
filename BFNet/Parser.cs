using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFNet
{
	public static class Parser
	{
		public static TreeRoot ParseFullTree(this string input)
		{
			var rawParsed = input.Parse();
			var expanded  = rawParsed.ExpandTree();

			return expanded;
		}

		public static TreeRoot Parse(this string input) => new()
		{
			// LINQ go brrr
			Tree = input
					   .Select(character => new Instruction {Operation = Instruction.GetOperation(character)})
					   .Cast<TreeObject>()
					   .ToArray()
		};

		private static TreeRoot ExpandTree(this TreeRoot tree)
		{
			return new()
			{
				Tree = tree.Tree
						   .Select((hierarchyObject, i) => // for each item in the tree
											((Instruction) hierarchyObject).Operation == Operations.StartLoop // is it a loop start? 
												? new Loop {TreeChildren = ExpandLoopContentRecursive(ref i, ref tree)} // Expand the loop (recursively!)
												: hierarchyObject).ToArray() // Else just copy it untouched
			};
		}

		private static TreeObject[] ExpandLoopContentRecursive(ref int i, ref TreeRoot root)
		{
			var loopContent = new List<TreeObject>();
			while (true)
			{
				var currentInstruction = (Instruction) root.Tree[++i];

				if (currentInstruction.Operation == Operations.StartLoop)
				{
					loopContent.Add(new Loop
					{
						TreeChildren = ExpandLoopContentRecursive(ref i, ref root)
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