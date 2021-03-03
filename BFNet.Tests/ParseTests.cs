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
			var expected = new TreeRoot
			{
				Tree = new TreeObject[]
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
			// welp that infinite loops, but this is only parsing not executing
			const string bf = "+[>+-<]-";
			var expected = new TreeRoot
			{
				Tree = new TreeObject[]
				{
					new Instruction {Operation = Operations.Increment},
					new Loop
					{
						TreeChildren = new TreeObject[]
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

			var actual = bf.ParseFullTree();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void NestedLoopTest()
		{
			const string bf       = "+[-[>]]";
			var          expected = new TreeRoot
			{
				Tree = new TreeObject[]
				{
					new Instruction {Operation = Operations.Increment},
					new Loop{TreeChildren = new TreeObject[]
					{
						new Instruction {Operation = Operations.Decrement},
						new Loop{TreeChildren = new TreeObject[]
						{
							new Instruction {Operation = Operations.PointerForward},
						}}
					}}
				}
			};
			var          actual   = bf.ParseFullTree();
			
			Assert.AreEqual(expected, actual);
		}
	}
}