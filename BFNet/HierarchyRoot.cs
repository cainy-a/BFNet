using System.Linq;

namespace BFNet
{
	public class HierarchyRoot
	{
		public HierarchyObject[] Hierarchy;
		
		public override bool Equals(object? obj) => obj is HierarchyRoot casted && Hierarchy.SequenceEqual(casted.Hierarchy);

		protected bool Equals(HierarchyRoot other) => Equals(this, other);

		public override int GetHashCode() => Hierarchy != null ? Hierarchy.GetHashCode() : 0;
	}
}