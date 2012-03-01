﻿using System;

namespace TextToCalcExpression.Tokens
{
	/// <summary>
	/// Description of StartParToken.
	/// </summary>
	public class StartParToken: Token
	{
		public StartParToken()
		{
			this.Text = "(";
		}
		
		public override TokenType TToken {
			get {
				return TokenType.STARTPAR;
			}
		}
		
		public override TokenGroup GToken {
			get {
				return TokenGroup.Separator;
			}
		}
	}
}
