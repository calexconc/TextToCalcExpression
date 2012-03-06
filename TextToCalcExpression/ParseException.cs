using System;
using TextToCalcExpression.Tokens;

namespace TextToCalcExpression
{
	/// <summary>
	/// Description of ParceException.
	/// </summary>
	public class ParseException : Exception
	{
		public ParseException()
		{
		}
		
		public ParseException(string message):base(message)
		{
		}
		
		public ParseException(Token token):base(string.Format("Error in \"{0}\"", token.Text))
		{
		}
	}
}
