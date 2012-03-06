using System;
using TextToCalcExpression.Tokens;

namespace TextToCalcExpression
{
	/// <summary>
	/// Description of ParceException.
	/// </summary>
	public class ParceException : Exception
	{
		public ParceException()
		{
		}
		
		public ParceException(string message):base(message)
		{
		}
		
		public ParceException(Token token):base(string.Format("Error in \"{0}\"", token.Text))
		{
		}
	}
}
