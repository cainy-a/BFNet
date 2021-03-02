using System.Collections.Generic;

namespace BFNet
{
	public class Instruction : HierarchyObject
	{
		public static readonly Dictionary<char, Operations> InstructionLookupTable = new()
		{
			{'+', Operations.Increment},
			{'-', Operations.Decrement},
			{'>', Operations.PointerForward},
			{'<', Operations.PointerBackward},
			{'.', Operations.AsciiOut},
			{',', Operations.AsciiIn},
			{'[', Operations.StartLoop},
			{']', Operations.EndLoop}
		};
		public static Operations GetOperation(char character) => InstructionLookupTable[character];

		public Operations Operation;
	}

	public enum Operations
	{
		Increment,
		Decrement,
		PointerForward,
		PointerBackward,
		AsciiOut,
		AsciiIn,
		StartLoop,
		EndLoop
	}
}