using System.Collections.Generic;
using System.Diagnostics;

namespace BFNet.BrainFck
{
	[DebuggerDisplay("{Operation.ToString()}")]
	public class Instruction : TreeObject
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

		public override bool Equals(object? obj) => obj is Instruction casted && Operation.Equals(casted.Operation);

		protected bool Equals(Instruction other) => Equals(this, other);

		public override int GetHashCode() => (int) Operation;
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