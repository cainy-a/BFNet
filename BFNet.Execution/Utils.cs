using System;
using System.Collections.Generic;
using System.Linq;

namespace BFNet.Execution
{
	public static class Utils
	{
		public static char  ToAsciiCode(this short input) => (char) input;
		public static short ToChar(this      char  input) => (short) input;
		
		public static void SafeIncrement(ref IList<short> cells, int index, short amount = 1)
		{
			SafeSet(ref cells, index, (short) (cells.ElementAtOrDefault(index) + amount));
		}

		public static void SafeSet(ref IList<short> cells, int index, short amount)
		{
			try
			{
				// Try to just set the value simply
				cells[index] = amount;
			}
			catch (ArgumentOutOfRangeException) // Value does not already exist
			{
				// If required, populate previous cells with default of 0
				for (var i = cells.Count; i < index; i++) cells.Insert(i, default);
				// Set value
				cells.Insert(index, amount);
			}
		}
	}
}