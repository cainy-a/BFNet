using System.Collections.Generic;
using BFNet.Execution;
using NUnit.Framework;

namespace BFNet.Tests
{
	public class InterpreterTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void SingleInstructionTest()
		{
			var interpreter = new Interpreter(new ());

			interpreter.ExecuteInstruction(new (){Operation = Operations.Increment});
			
			Assert.AreEqual(1, interpreter._memoryCells[0]);
		}
		
		[Test]
		public void SafeIncrementTest()
		{
			var          expected1 = new List<short> { 1 };
			var          expected2 = new List<short> { 1, 0, 0, 1 };
			var          expected3 = new List<short> { 0, 0, 0, 1 };
			var          expected4 = new List<short> { 0, 2, 0, 1 };
			IList<short> working   = new List<short>();
			
			IncrementAndTest(expected1, 0);
			IncrementAndTest(expected2, 3);
			IncrementAndTest(expected3, 0, -1);
			IncrementAndTest(expected4, 1, 2);
			
			// I love compact code
			void IncrementAndTest(IList<short> expected, int index, short increment = 1)
			{
				Utils.SafeIncrement(ref working, index, increment);
				Assert.AreEqual(expected, working);
			}
		}
	}
}