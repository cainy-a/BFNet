using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFNet.Cli
{
	public static class Utils
	{
		public static bool Contains(this IEnumerable<char> enumerable, params char[] chars) 
			=> chars.Any(c => Contains(enumerable, c)); // check for any characters which are contained in enumerable

		public static void RemoveLast(this StringBuilder sb) => sb.Remove(sb.Length - 1, 1);
	}
}