using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFNet.PreProcessing
{
	public static class Parser
	{
		public static TreeRoot ParseFullTree(this string input, char? lineCommentChar = null) => input.RemoveComments(lineCommentChar).Parse().ExpandTree();

		public static TreeRoot Parse(this string input) => new()
		{
			// LINQ go brrr
			Tree = input
					   .Select(character => new Instruction {Operation = Instruction.GetOperation(character)})
					   .Cast<TreeObject>()
					   .ToArray()
		};

		private static TreeRoot ExpandTree(this TreeRoot root)
		{
			var tree = new List<TreeObject>();
			// Actually it can't - I did that in 2fc90e87 and it broke.
			// ReSharper disable once LoopCanBeConvertedToQuery
			for (var i = 0; i < root.Tree.Length; i++)
			{
				var hierarchyObject = root.Tree[i];

				tree.Add(((Instruction) hierarchyObject).Operation == Operations.StartLoop
							 ? new Loop {TreeChildren = ExpandLoopContentRecursive(ref i, ref root)}
							 : hierarchyObject);
			}

			return new TreeRoot {Tree = tree.ToArray()};
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

		public static string RemoveComments(this string input, char? lineCommentChar = null)
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