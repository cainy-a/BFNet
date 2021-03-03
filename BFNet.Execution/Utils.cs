namespace BFNet.Execution
{
	public static class Utils
	{
		public static char  ToAsciiCode(this short input) => (char) input;
		public static short ToChar(this      char  input) => (short) input;
	}
}