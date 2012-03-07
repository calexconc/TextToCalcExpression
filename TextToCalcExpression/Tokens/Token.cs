﻿using System;
using System.Text.RegularExpressions;

namespace TextToCalcExpression.Tokens
{
	
	public enum TokenType { NUM = 0, PAR = 1, SUM = 2, SUB = 3, MULT = 4, DIV = 5, REM = 6, POW = 7, STARTPAR = 8, ENDPAR = 9,
		AND = 10, OR = 11, NOT = 12, EQUALS = 13, NOTEQUALS = 14, LOWER = 15, GREATER = 16, LOWEROREQUALS = 17, GREATEROREQUALS = 18
			,BOOL = 19, EXP = 20, LN = 21, COS = 22, SIN = 23, TAN = 24, COSH = 25, SINH = 26, TANH = 27,
		EOF = 256}
	public enum TokenGroup { Operator = 0, Identifier = 1, Literal = 2, Separator = 3 }
	
	/// <summary>
	/// Description of Token.
	/// </summary>
	public abstract class Token
	{
		private static Regex _isnumeric = new Regex(@"(^(\+?\-? *[0-9]+)([,0-9 ]*)([0-9 ])*$)|(^ *$)");
		private static Regex _isbool = new Regex("(true|false|True|False|FALSE|TRUE)");
		
		public string Text
		{
			get;
			protected set;
		}
		
		public abstract TokenType TToken { get; }
		
		public abstract TokenGroup GToken { get; }
		
		public static Token Create(string value)
		{
			TokenType ttype = GetTokenType(value);
			Token token = null;
			
			switch (ttype)
			{
				case TokenType.SUM:
				case TokenType.SUB:
				case TokenType.MULT:
				case TokenType.DIV:
				case TokenType.POW:
				case TokenType.REM:
				case TokenType.AND:
				case TokenType.OR:
				case TokenType.NOT:
				case TokenType.EQUALS:
				case TokenType.NOTEQUALS:
				case TokenType.GREATER:
				case TokenType.GREATEROREQUALS:
				case TokenType.LOWER:
				case TokenType.LOWEROREQUALS:
					token = new OperatorToken(ttype);
					break;
				case TokenType.STARTPAR:
					token = new StartParToken();
					break;
				case TokenType.ENDPAR:
					token = new EndParToken();
					break;
				case TokenType.PAR:
					token = new ParToken(value);
					break;
				case TokenType.NUM:
					token = new NumToken(value, InferNumericType(value));
					break;
				case TokenType.BOOL:
					token = new BoolToken(value);
					break;
				case TokenType.EOF:
					token = new EofToken();
					break;
			}
			
			return token;
		}
		
		private static bool IsNumeric(string value)
		{
			return _isnumeric.IsMatch(value);
		}
		
		private static bool IsBoolean(string value)
		{
			return _isbool.IsMatch(value);
		}
		
		private static Type InferNumericType(string value)
		{
			if (value.Contains(".") || value.Contains("."))
				return typeof(double);
			else
				return typeof(int);
		}
		
		private static TokenType GetTokenType(string value)
		{
			switch (value)
			{
				case "+":
					return TokenType.SUM;
				case "-":
					return TokenType.SUB;
				case "*":
					return TokenType.MULT;
				case "/":
					return TokenType.DIV;
				case "%":
					return TokenType.REM;
				case "(":
					return TokenType.STARTPAR;
				case ")":
					return TokenType.ENDPAR;
				case "^":
					return TokenType.POW;
				case "==":
					return TokenType.EQUALS;
				case "!=":
					return TokenType.NOTEQUALS;
				case "<":
					return TokenType.LOWER;
				case ">":
					return TokenType.GREATER;
				case "<=":
					return TokenType.LOWEROREQUALS;
				case ">=":
					return TokenType.GREATEROREQUALS;
				case "AND":
					return TokenType.AND;
				case "OR":
					return TokenType.OR;
				case "NOT":
					return TokenType.NOT;
				case "":
					return TokenType.EOF;
				default:
					if (IsNumeric(value))
					   return TokenType.NUM;
					else if (IsBoolean(value))
						return TokenType.BOOL;
					else
						return TokenType.PAR;
			}		
		}
	}
}
