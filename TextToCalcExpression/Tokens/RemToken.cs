using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of RemToken.
	/// </summary>
	public class RemToken: Token
	{
		public RemToken()
		{
			this.Text = "%";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.REM;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Operator;
			}
		}
	}
}
