using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of DivToken.
	/// </summary>
	public class DivToken: Token
	{
		public DivToken()
		{
			this.Text = "/";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.DIV;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Operator;
			}
		}
	}
}
