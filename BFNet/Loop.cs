using System.Linq;

namespace BFNet
{
	public class Loop : HierarchyObject
	{
		public HierarchyObject[] HierarchyChildren;

		public override bool Equals(object? obj) => obj is Loop casted && HierarchyChildren.SequenceEqual(casted.HierarchyChildren);

		protected bool Equals(Loop other) => Equals(this, other);

		public override int GetHashCode() => HierarchyChildren != null ? HierarchyChildren.GetHashCode() : 0;
	}
}