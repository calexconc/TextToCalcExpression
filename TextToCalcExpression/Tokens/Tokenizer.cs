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
			
			foreach (char item in this._text)
			{
				if (IsMathOperationSymbol(item))
				{
					if (curr != string.Empty)
						yield return curr;
					
					yield return item.ToString();
					
					curr = string.Empty;
				}
				else if (IsBoolOperationSymbol(item.ToString()))
				{
					yield return curr;
					curr = item.ToString();
				}
				else if (IsBoolOperationSymbol(curr))
				{
					if (IsBoolOperationSymbol(curr+item))
					{
						yield return curr+item;
						curr = string.Empty;
					}
					else
					{
						yield return curr;
						curr = item.ToString();
					}
				}
				else if (item == ' ')
				{
					yield return curr;
					curr = string.Empty;
				}
				else
					curr += item;
			}
			
			if (curr != string.Empty)
				yield return curr;
		}
		
		private bool IsMathOperationSymbol(char value)
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
		
		private bool IsBoolOperationSymbol(string value)
		{
			switch (value)
			{
				case "==":
				case "!=":
				case "<":
				case ">":
				case "=<":
				case ">=":
				case "!":
				case "=":
					return true;
				default:
					return false;
			}		
		}
		
		#endregion
		
		
	}
}
