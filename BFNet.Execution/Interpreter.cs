using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BFNet.MoreFck;

namespace BFNet.Execution
{
	public class Interpreter
	{
		// list expansion is slow, so to increase memory access
		// performance at first, start with 32 cells
		// (this becomes exponentially less of a problem as the amount of cells grows) 
		public IList<short>        MemoryCells  { get; private set; } = new List<short>(32);
		public int                 Pointer      { get; private set; }
		public int                 InputIndex   { get; private set; }
		public TreeRoot            Tree { get; private set; }
		public InterpreterSettings Settings     { get; private set; }

		public Interpreter(TreeRoot tree, InterpreterSettings settings = null)
		{
			Tree  = tree;
			Settings = settings ?? new InterpreterSettings();
		}

		public void InjectNewInstructions(TreeRoot tree, bool append = false)
		{
			if (!append)
			{
				Tree = tree;
				return;
			}

			var list = tree.Tree.ToList();
			list.AddRange(tree.Tree);
			Tree.Tree = list.ToArray();
		}

		public string StartInterpret() => Interpret(Tree.Tree);

		// TODO: Graceful error handling
		private string Interpret(IEnumerable<TreeObject> tree, bool isLoop = false)
		{
			var output = new StringBuilder(string.Empty);
			if (isLoop)
				while (MemoryCells.ElementAtOrDefault(Pointer) != 0)
					ExecuteAllInTree();
			else
				ExecuteAllInTree();

			return output.ToString();

			void ExecuteAllInTree()
			{
				foreach (var treeObject in tree)
				{
					if (treeObject.GetType() == typeof(Instruction))
					{
						var instructionResult = ExecuteInstruction((Instruction) treeObject);
						if (instructionResult.HasValue) output.Append(instructionResult.Value);
					}
					else output.Append(Interpret(((Loop) treeObject).TreeChildren, true));
				}
			}
		}

		public char? ExecuteInstruction(Instruction instruction)
		{
			var memoryCellsRefHack = MemoryCells; // Hack to get ref to work
			switch (instruction.Operation)
			{
				case Operations.Add:
					Utils.SafeIncrement(ref memoryCellsRefHack, Pointer, instruction.OpData);
					break;
				case Operations.Subtract:
					// Hack to get ref to work
					Utils.SafeIncrement(ref memoryCellsRefHack, Pointer, (short) - instruction.OpData);
					break;
				case Operations.PointerForward:
					Pointer += instruction.OpData;
					break;
				case Operations.PointerBackward:
					Pointer -= instruction.OpData;
					break;
				case Operations.AsciiOut:
					if (Settings.UseConsoleOutput) Console.Write(MemoryCells[Pointer].ToAsciiCode());
					else return MemoryCells[Pointer].ToAsciiCode();
					break;
				case Operations.AsciiIn:
					if (Settings.UseConsoleInput) MemoryCells[Pointer] = Console.ReadKey().KeyChar.ToChar();
					else
					{
						MemoryCells[Pointer] = Settings.Input[InputIndex].ToChar();
						InputIndex++;
					}
					break;
				case Operations.SetZero:
					Utils.SafeSet(ref memoryCellsRefHack, Pointer, 0);
					break;
				
				case Operations.StartLoop:
					throw new InvalidOperationException("A loop start instruction is not valid here - preprocessing failed");
				case Operations.EndLoop:
					throw new InvalidOperationException("A loop end instruction is not valid here - preprocessing failed or unmatched endloop");
				default:
					throw new InvalidDataException($"Invalid instruction \"{instruction.Operation.ToString()}\"");
			}
			
			MemoryCells = memoryCellsRefHack;

			return null;
		}
	}
}