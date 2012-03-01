﻿using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of SubToken.
	/// </summary>
	public class SubToken: Token
	{
		public SubToken()
		{
			this.Text = "-";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.SUB;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Operator;
			}
		}
	}
}
