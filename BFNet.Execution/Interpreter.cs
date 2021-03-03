using System;
using System.Collections.Generic;
using System.IO;

namespace BFNet.Execution
{
	public class Interpreter
	{
		// list expansion is slow, so to increase memory access
		// performance at first, start with 32 cells
		// (this becomes exponentially less of a problem as the amount of cells grows) 
		private          IList<short>        _memoryCells = new List<short>(32);
		private          int                 _pointer     = 0;
		private          int                 _inputIndex  = 0;
		private readonly TreeRoot            _instructions;
		private readonly InterpreterSettings _settings;

		public Interpreter(TreeRoot instructions, InterpreterSettings settings = null)
		{
			_instructions  = instructions;
			_settings = settings ?? new InterpreterSettings();
		}

		public void Interpret()
		{
			
		}

		public char? ExecuteInstruction(Instruction instruction)
		{
			switch (instruction.Operation)
			{
				case Operations.Increment:
					_memoryCells[_pointer]++;
					break;
				case Operations.Decrement:
					_memoryCells[_pointer]--;
					break;
				case Operations.PointerForward:
					_pointer++;
					break;
				case Operations.PointerBackward:
					_pointer--;
					break;
				case Operations.AsciiOut:
					if (_settings.UseConsole) Console.Write(_memoryCells[_pointer].ToAsciiCode());
					else return _memoryCells[_pointer].ToAsciiCode();
					break;
				case Operations.AsciiIn:
					if (_settings.UseConsole) _memoryCells[_pointer] = Console.ReadKey().KeyChar.ToChar();
					else
					{
						_memoryCells[_pointer] = _settings.Input[_inputIndex].ToChar();
						_inputIndex++;
					}
					break;
				case Operations.StartLoop:
					throw new InvalidOperationException("Loop start & end instructions are not valid here - preprocessing failed");
				case Operations.EndLoop:
					throw new InvalidOperationException("Loop start & end instructions are not valid here - preprocessing failed or unmatched endloop");
				default:
					throw new InvalidDataException($"Invalid instruction \"{instruction.Operation.ToString()}\"");
			}

			return null;
		}
	}
}