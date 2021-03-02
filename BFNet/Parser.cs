using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFNet
{
	public static class Parser
	{
		public static HierarchyRoot ParseFullHierarchy(this string input) { throw new NotImplementedException(); }

		private static HierarchyRoot Parse(this string input) => new()
		{
			// LINQ go brrr
			Hierarchy = input
					   .Select(character => new Instruction {Operation = Instruction.GetOperation(character)})
					   .Cast<HierarchyObject>()
					   .ToArray()
		};

		private static HierarchyRoot ExpandHierarchy(this HierarchyRoot hierarchy)
		{
			var tree = new List<HierarchyObject>();
			for (var i = 0; i < hierarchy.Hierarchy.Length; i++)
			{
				var hierarchyObject = hierarchy.Hierarchy[i];

				if (((Instruction) hierarchyObject).Operation == Operations.StartLoop)
					tree.Add(new Loop {HierarchyChildren = ExpandLoopContentRecursive(ref i, ref tree, ref hierarchy)});
			}

			return new HierarchyRoot {Hierarchy = tree.ToArray()};
		}

		private static HierarchyObject[] ExpandLoopContentRecursive(ref int i, ref List<HierarchyObject> tree, ref HierarchyRoot root)
		{
			var loopContent = new List<HierarchyObject>();
			var oldIndex    = i;
			while (true)
			{
				var currentInstruction = (Instruction) root.Hierarchy[i];

				if (currentInstruction.Operation == Operations.StartLoop)
					loopContent.Add(new Loop
					{
						HierarchyChildren = ExpandLoopContentRecursive(ref i, ref tree, ref root)
					});

				if (currentInstruction.Operation == Operations.EndLoop) break;
				loopContent.Add(currentInstruction);
			}

			i = oldIndex;
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