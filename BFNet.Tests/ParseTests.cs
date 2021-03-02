using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace BFNet.Tests
{
	public class ParseTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void NoLoopsTest()
		{
			const string bf = "+>+<";
			var expected = new HierarchyRoot
			{
				Hierarchy = new HierarchyObject[]
				{
					new Instruction {Operation = Operations.Increment},
					new Instruction {Operation = Operations.PointerForward},
					new Instruction {Operation = Operations.Increment},
					new Instruction {Operation = Operations.PointerBackward}
				}
			};

			var actual = bf.Parse();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void OneLevelLoopTest()
		{
			// welp that infinite loops
			const string bf = "+[>+-<]-";
			var expected = new HierarchyRoot
			{
				Hierarchy = new HierarchyObject[]
				{
					new Instruction {Operation = Operations.Increment},
					new Loop
					{
						HierarchyChildren = new HierarchyObject[]
						{
							new Instruction {Operation = Operations.PointerForward},
							new Instruction {Operation = Operations.Increment},
							new Instruction {Operation = Operations.Decrement},
							new Instruction {Operation = Operations.PointerBackward}
						}
					},
					new Instruction {Operation = Operations.Decrement}
				}
			};

			var actual = bf.ParseFullHierarchy();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void NestedLoopTest()
		{
			const string bf       = "+[-[>]]";
			var          expected = new HierarchyRoot
			{
				Hierarchy = new HierarchyObject[]
				{
					new Instruction {Operation = Operations.Increment},
					new Loop{HierarchyChildren = new HierarchyObject[]
					{
						new Instruction {Operation = Operations.Decrement},
						new Loop{HierarchyChildren = new HierarchyObject[]
						{
							new Instruction {Operation = Operations.PointerForward},
						}}
					}}
				}
			};
			var          actual   = bf.ParseFullHierarchy();
			
			Assert.AreEqual(expected, actual);
		}
	}
}