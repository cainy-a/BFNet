using BFNet.PreProcessing;
using NUnit.Framework;

namespace BFNet.Tests
{
	public class OptimizeTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void SimpleCollapseTest()
		{
			TreeRoot unOptimized = new()
			{
				Tree = new TreeObject[]
				{
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment}
				}
			};
			TreeRoot expected = new()
			{
				Tree = new TreeObject[]
				{
					new MoreFck.Instruction {Operation = MoreFck.Operations.Add, OpData = 5}
				}
			};

			var actual = unOptimized.Optimize();
			
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ComplexCollapseTest()
		{
			TreeRoot unOptimized = new()
			{
				Tree = new TreeObject[]
				{
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Decrement},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Decrement},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerForward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerForward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerForward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerForward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerForward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Increment},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Decrement},
					new BrainFck.Instruction {Operation = BrainFck.Operations.Decrement},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerBackward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerBackward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerBackward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerBackward},
					new BrainFck.Instruction {Operation = BrainFck.Operations.PointerBackward}
				}
			};
			TreeRoot expected = new()
			{
				Tree = new TreeObject[]
				{
					new MoreFck.Instruction {Operation = MoreFck.Operations.Add, OpData            = 3},
					new MoreFck.Instruction {Operation = MoreFck.Operations.Subtract, OpData       = 2},
					new MoreFck.Instruction {Operation = MoreFck.Operations.PointerForward, OpData = 5},
					new MoreFck.Instruction {Operation = MoreFck.Operations.Add, OpData            = 3},
					new MoreFck.Instruction {Operation = MoreFck.Operations.Subtract, OpData       = 2},
					new MoreFck.Instruction {Operation = MoreFck.Operations.PointerBackward, OpData = 5}
				}
			};

			var actual = unOptimized.Optimize();
			
			Assert.AreEqual(expected, actual);
		}
	}
}