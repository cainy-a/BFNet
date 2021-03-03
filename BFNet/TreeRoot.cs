using System.Linq;

namespace BFNet
{
	public class TreeRoot
	{
		public TreeObject[] Tree;
		
		public override bool Equals(object? obj) => obj is TreeRoot casted && Tree.SequenceEqual(casted.Tree);

		protected bool Equals(TreeRoot other) => Equals(this, other);

		public override int GetHashCode() => Tree != null ? Tree.GetHashCode() : 0;
	}
}