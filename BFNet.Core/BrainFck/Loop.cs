using System.Linq;

namespace BFNet.BrainFck
{
	public class Loop : TreeObject
	{
		public TreeObject[] TreeChildren;

		public override bool Equals(object? obj) => obj is Loop casted && TreeChildren.SequenceEqual(casted.TreeChildren);

		protected bool Equals(Loop other) => Equals(this, other);

		public override int GetHashCode() => TreeChildren != null ? TreeChildren.GetHashCode() : 0;
	}
}