﻿using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of EndParToken.
	/// </summary>
	public class EndParToken: Token
	{
		public EndParToken()
		{
			this.Text = ")";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.ENDPAR;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Separator;
			}
		}
	}
}
