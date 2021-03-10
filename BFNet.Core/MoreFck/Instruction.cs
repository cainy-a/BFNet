using System.Diagnostics;

namespace BFNet.MoreFck
{
	[DebuggerDisplay("{Operation.ToString()}")]
	public class Instruction : TreeObject
	{
		public Operations Operation;

		public int OpData;

		public override bool Equals(object? obj) => obj is Instruction casted && Operation.Equals(casted.Operation);

		protected bool Equals(Instruction other) => Equals(this, other);

		public override int GetHashCode() => (int) Operation;
	}

	public enum Operations
	{
		Add,
		Subtract,
		PointerForward,
		PointerBackward,
		AsciiOut,
		AsciiIn,
		StartLoop,
		EndLoop,
		SetZero
	}
}