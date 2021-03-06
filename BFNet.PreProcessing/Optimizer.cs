using System;
using System.Collections.Generic;
using System.Linq;

namespace BFNet.PreProcessing
{
	public static class Optimizer
	{
		public static TreeRoot Optimize(this TreeRoot tree) => new() {Tree = OptimizeRecursive(tree.Tree)};

		private static TreeObject[] OptimizeRecursive(TreeObject[] treeObjects)
		{
			IList<TreeObject> working = new List<TreeObject>();

			for (var i = 0; i < treeObjects.Length; i++)
			{
				if (IsSetZero(treeObjects, i))
				{
					working.Add(new MoreFck.Instruction{Operation = MoreFck.Operations.SetZero});
					continue;
				}
				
				if (treeObjects[i].GetType() == typeof(BrainFck.Loop))
				{
					var loop      = (BrainFck.Loop) treeObjects[i];
					var optimized = OptimizeRecursive(loop.TreeChildren);
					working.Add(new MoreFck.Loop {TreeChildren = optimized});
					continue;
				}

				var instruction = (BrainFck.Instruction) treeObjects[i];
				// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
				switch (instruction.Operation)
				{
					case BrainFck.Operations.Increment:
					case BrainFck.Operations.Decrement:
					case BrainFck.Operations.PointerForward:
					case BrainFck.Operations.PointerBackward:
						var amount = CountConsecutiveOps(treeObjects, i, instruction.Operation, out var endIndex);
						i = endIndex;
						working.Add(new MoreFck.Instruction
						{
							// ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
							Operation = instruction.Operation switch
							{
								BrainFck.Operations.Increment       => MoreFck.Operations.Add,
								BrainFck.Operations.Decrement       => MoreFck.Operations.Subtract,
								BrainFck.Operations.PointerForward  => MoreFck.Operations.PointerForward,
								BrainFck.Operations.PointerBackward => MoreFck.Operations.PointerBackward,
								// ReSharper disable once CA2208
								_ => throw new ArgumentOutOfRangeException(nameof(instruction),
																		   $"Invalid operation {instruction.Operation.ToString()}")
							},
							OpData = (short) amount
						});
						break;
					case BrainFck.Operations.AsciiOut:
						working.Add(new MoreFck.Instruction {Operation = MoreFck.Operations.AsciiOut});
						break;
					case BrainFck.Operations.AsciiIn:
						working.Add(new MoreFck.Instruction {Operation = MoreFck.Operations.AsciiIn});
						break;
					default:
						// ReSharper disable once CA2208
						throw new ArgumentOutOfRangeException(nameof(instruction),
															  $"Unacceptable operation: {instruction.Operation.ToString()}");
				}
			}

			return working.ToArray();
		}

		private static int CountConsecutiveOps(TreeObject[] treeObjects, int startIndex, BrainFck.Operations op,
											   out int      endIndex)
		{
			var amountFound = 0;
			for (var i = startIndex; i < treeObjects.Length; i++)
			{
				if (treeObjects[i] is not BrainFck.Instruction instruction || instruction.Operation != op) break;

				amountFound++;
			}

			endIndex = startIndex + amountFound - 1;
			return amountFound;
		}

		private static bool IsSetZero(TreeObject[] treeObjects, int i)
		{
			if (treeObjects[i].GetType() != typeof(BrainFck.Loop)) return false; // must be a BrainF*ck loop
			
			var loop = (BrainFck.Loop) treeObjects[i]; // we know its safe to cast now
			return loop.TreeChildren.Length == 1 // only one object in loop
				&& loop.TreeChildren[0].GetType() == typeof(BrainFck.Instruction) // must be a BrainF*ck instruction
				&& ((BrainFck.Instruction) loop.TreeChildren[0]).Operation == BrainFck.Operations.Decrement; // must be decrement op
		}
	}
}