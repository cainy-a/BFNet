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
			
			// this program is so simple that it doesnt output anything. We manually check the program's memory.
			Assert.AreEqual(1, interpreter.MemoryCells[0]);
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

		[Test]
		public void NoLoopInterpretTest()
		{
			var interpreter = new Interpreter(new TreeRoot{Tree = new TreeObject[]
			{
				new Instruction{Operation = Operations.Increment},
				new Instruction{Operation = Operations.PointerForward},
				new Instruction{Operation = Operations.PointerForward},
				new Instruction{Operation = Operations.Increment},
				new Instruction{Operation = Operations.Increment}
			}});

			interpreter.StartInterpret();

			IList<short> expected = new List<short>{ 1, 0, 2 };
			
			Assert.AreEqual(expected, interpreter.MemoryCells);
		}

		[Test]
		public void LoopInterpretTest()
		{
			// Add two numbers together in brainf*ck! (it's smaller than "Hello, World!", at least)
			// Example program & comments source: https://en.wikipedia.org/wiki/Brainfuck#Adding_two_values
			var interpreter = new Interpreter(new TreeRoot{Tree = new TreeObject[]
			{
				// Cell c0 = 2
				I(Operations.Increment), I(Operations.Increment),
				// Cell c1 = 5
				I(Operations.PointerForward),
				I(Operations.Increment), I(Operations.Increment), I(Operations.Increment), I(Operations.Increment), I(Operations.Increment),
				//Start your loops with your cell pointer on the loop counter (c1 in our case)
				L(new TreeObject[]
				{
					// Add 1 to c0
					I(Operations.PointerBackward),
					I(Operations.Increment)
				}), // End your loops with the cell pointer on the loop counter
				
				/*
				 * At this point our program has added 5 to 2 leaving 7 in c0 and 0 in c1
				 * but we cannot output this value to the terminal since it is not ASCII encoded.
				 *
				 * To display the ASCII character "7" we must add 48 to the value 7.
				 * We use a loop to compute 48 = 6 * 8.
				 */
				
				// c1 = 8 and this will be our loop counter again
				I(Operations.Increment), I(Operations.Increment), I(Operations.Increment), I(Operations.Increment),
				I(Operations.Increment), I(Operations.Increment), I(Operations.Increment), I(Operations.Increment),
				L(new TreeObject[]
				{
					// Add 6 to c0
					I(Operations.PointerBackward),
					I(Operations.Increment), I(Operations.Increment), I(Operations.Increment),
					I(Operations.Increment), I(Operations.Increment), I(Operations.Increment),
					// Subtract 1 from c1
					I(Operations.PointerForward),
					I(Operations.Decrement)
				}),
				// Print out c0 which has the value 55 which translates to "7"!
				I(Operations.PointerBackward),
				I(Operations.AsciiOut)
			}});

			var actual = interpreter.StartInterpret();

			const string expected = "7";
			
			Assert.AreEqual(expected, actual);


			static Instruction I(Operations   op)   => new() {Operation    = op};
			static Loop        L(TreeObject[] tree) => new() {TreeChildren = tree};
		}
	}
}