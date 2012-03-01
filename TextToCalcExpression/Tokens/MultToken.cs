using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of MultToken.
	/// </summary>
	public class MultToken: Token
	{
		public MultToken()
		{
			this.Text = "*";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.MULT;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Operator;
			}
		}
	}
}
