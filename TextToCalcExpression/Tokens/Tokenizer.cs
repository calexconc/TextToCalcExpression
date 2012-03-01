using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of Tokenizer.
	/// </summary>
	public class Tokenizer
	{
		private string _text;
		private Regex _isnumeric = new Regex(@"(^(\+?\-? *[0-9]+)([,0-9 ]*)([0-9 ])*$)|(^ *$)");
		
		public Tokenizer(string text)
		{
			this._text = text;
		}
		
		#region public members
		
		public IEnumerable<Token> ExtractTokens()
		{
			foreach (string item in this.ScanText())
			{
				Token token = Token.Create(item);
				
				if (token.TToken == TokenType.PAR && ((ParToken)token).HasSignal())
				{
					ParToken ptoken = (ParToken)token;
					
					foreach (Token xitem in ptoken.Decompose())
						yield return xitem;
				}
				else
					yield return token;
			}
			
			yield return Token.Create("");
		}
		
		#endregion
		
		#region private methods
		
		private IEnumerable<string> ScanText()
		{
			string curr = string.Empty;
			char? previous = null;
			
			foreach (char item in this._text)
			{
				if (IsOperationSymbol(item))
				{
					if (curr != string.Empty)
						yield return curr;
					
					yield return item.ToString();
					
					curr = string.Empty;
					previous = item;
				}
				else if (item != ' ')
				{
					previous = item;
					curr += item;
				}
			}
			
			if (curr != string.Empty)
				yield return curr;
		}
		
		private bool IsNumeric(string value)
		{
			return _isnumeric.IsMatch(value);
		}
		
		private bool IsOperationSymbol(char value)
		{
			switch (value)
			{
				case '+':
				case '-':
				case '*':
				case '/':
				case '%':
				case '(':
				case ')':
				case '^':
					return true;
				default:
					return false;
			}		
		}
		
		#endregion
		
		
	}
}
