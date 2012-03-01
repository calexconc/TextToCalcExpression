using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of SumToken.
	/// </summary>
	public class SumToken : Token
	{
		public SumToken()
		{
			this.Text = "+";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.SUM;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Operator;
			}
		}
	}
}
