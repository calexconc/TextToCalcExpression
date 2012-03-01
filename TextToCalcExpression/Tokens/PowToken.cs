using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of PowToken.
	/// </summary>
	public class PowToken: Token
	{
		public PowToken()
		{
			this.Text = "^";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.POW;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Operator;
			}
		}
	}
}
