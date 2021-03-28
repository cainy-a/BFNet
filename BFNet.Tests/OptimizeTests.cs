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
					BI(BrainFck.Operations.Increment),
					BI(BrainFck.Operations.Increment),
					BI(BrainFck.Operations.Increment),
					BI(BrainFck.Operations.Decrement),
					BI(BrainFck.Operations.Decrement),
					BI(BrainFck.Operations.PointerForward),
					BI(BrainFck.Operations.PointerForward),
					BI(BrainFck.Operations.PointerForward),
					BI(BrainFck.Operations.PointerForward),
					BI(BrainFck.Operations.PointerForward),
					BI(BrainFck.Operations.Increment),
					BI(BrainFck.Operations.Increment),
					BI(BrainFck.Operations.Increment),
					BI(BrainFck.Operations.Decrement),
					BI(BrainFck.Operations.Decrement),
					BI(BrainFck.Operations.PointerBackward),
					BI(BrainFck.Operations.PointerBackward),
					BI(BrainFck.Operations.PointerBackward),
					BI(BrainFck.Operations.PointerBackward),
					BI(BrainFck.Operations.PointerBackward)
				}
			};
			TreeRoot expected = new()
			{
				Tree = new TreeObject[]
				{
					MI(MoreFck.Operations.Add,             3),
					MI(MoreFck.Operations.Subtract,        2),
					MI(MoreFck.Operations.PointerForward,  5),
					MI(MoreFck.Operations.Add,             3),
					MI(MoreFck.Operations.Subtract,        2),
					MI(MoreFck.Operations.PointerBackward, 5)
				}
			};

			var actual = unOptimized.Optimize();

			Assert.AreEqual(expected, actual);

			BrainFck.Instruction BI(BrainFck.Operations op)                 => new() {Operation = op};
			MoreFck.Instruction  MI(MoreFck.Operations  op, short data = 1) => new() {Operation = op, OpData = data};
		}
	}
}