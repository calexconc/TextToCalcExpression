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
			bool start = true;
			
			foreach (string item in this.ScanText())
			{
				Token token = Token.Create(item);
				
				if (start && token.TToken == TokenType.SUM)
					;
				else if (start && token.TToken == TokenType.SUB)
				{
					yield return Token.Create("-1");
					yield return Token.Create("*");
				}
				else if (token.TToken == TokenType.PAR && ((ParToken)token).HasSignal())
				{
					ParToken ptoken = (ParToken)token;
					
					foreach (Token xitem in ptoken.Decompose())
						yield return xitem;
				}
				else
					yield return token;
				
				if (token.TToken == TokenType.STARTPAR)
					start = true;
				else
					start = false;
			}
			
			yield return Token.Create("");
		}
		
		#endregion
		
		#region private methods
		
		private IEnumerable<string> ScanText()
		{
			string curr = string.Empty;
			string curr_signal = string.Empty;
			bool start = true;
			
			foreach (char item in this._text)
			{
				if (IsOperationSymbol(item.ToString()))
				{
					if (!IsOperationSymbol(curr))
					{
						if (curr != string.Empty)
							yield return curr;
						
						if (start && IsSignalSymbol(item.ToString()))
							yield return item.ToString();
						else
							curr = item.ToString();
					}
					else if (curr==")")
					{
						yield return curr;
						curr = item.ToString();
					}
					else if (IsOperationSymbol(curr+item))
					{
						curr +=item;
						yield return curr;
						
						curr = string.Empty;
					}
					else if (IsSignalSymbol(item.ToString()))
					{
						if (item == '-')
							curr_signal = item.ToString();
						if (curr != string.Empty)
							yield return curr;
						curr = string.Empty;
					}
					else
						curr +=item;
				}
				else if (item == ' ' && curr !=string.Empty)
				{
					yield return curr;
					curr = string.Empty;
				}
				else if (item != ' ')
				{
					if (IsOperationSymbol(curr))
					{
						yield return curr;
						curr = string.Empty;
					}
					
					if (curr == string.Empty && curr_signal != string.Empty)
					{
						curr += curr_signal;
						curr_signal = string.Empty;
					}
					
					curr += item;
				}
				
				start = false;
			}
			
			if (curr != string.Empty)
				yield return curr;
		}
		
		private bool IsSignalSymbol(string value)
		{
			switch (value)
			{
				case "+":
				case "-":
					return true;
				default:
					return false;
			}	
		}
		
		private bool IsOperationSymbol(string value)
		{
			switch (value)
			{
				case "+":
				case "-":
				case "*":
				case "/":
				case "%":
				case "(":
				case ")":
				case "^":
				case "==":
				case "!=":
				case "<":
				case ">":
				case "<=":
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
