﻿using System.Collections.Generic;
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
			
			Utils.SafeIncrement(ref working, 0);
			Assert.AreEqual(expected1, working);
			
			Utils.SafeIncrement(ref working, 3);
			Assert.AreEqual(expected2, working);
			
			Utils.SafeIncrement(ref working, 0, -1);
			Assert.AreEqual(expected3, working);
			
			Utils.SafeIncrement(ref working, 1, 2);
			Assert.AreEqual(expected4, working);
		}
	}
}