using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TextToCalcExpression.Tokens;
using TextToCalcExpression.Collections;

namespace TextToCalcExpression
{
	/// <summary>
	/// Description of ExpressionBuilder.
	/// </summary>
	public class ExpressionBuilder
	{
		Dictionary<string, ParameterExpression> _parameters;
		TokenTree _tree;
		ParameterInfo[] _parametersInfo;
		ParameterInfo _returnType;
		int _curr_index = 0;
		
		public ExpressionBuilder()
		{
		}
		
		public Expression<T> Create<T>(string expression)
		{
			_parameters = new Dictionary<string, ParameterExpression>();
			_tree = new TokenTree(GetTokens(expression));
			this.GetExpressionSpec(typeof(T));
			Expression body = ProcessToken(_tree.Root);
			
			return Expression.Lambda<T>(body, _parameters.Values);
		}
		
		#region private members
		
		private void GetExpressionSpec(Type delegatedType)
		{
			MethodInfo mInfo = delegatedType.GetMethod("Invoke");
			
			_returnType = mInfo.ReturnParameter;
			_parametersInfo = mInfo.GetParameters();
		}
		
		private IEnumerable<Token> GetTokens(string expression)
		{
			Tokenizer tokenizer = new Tokenizer(expression);
			
			foreach (var item in tokenizer.ExtractTokens())
				yield return item;
		}
		
		private Expression ProcessToken(BinaryNode<Token> node)
		{
			switch (node.Value.TToken)
			{
				case TokenType.SUM: 
					return DefineSum(node);
				case TokenType.SUB: 
					return DefineSub(node);
				case TokenType.MULT: 
					return DefineMult(node);
				case TokenType.DIV: 
					return DefineDiv(node);
				case TokenType.POW: 
					return DefinePow(node);
				case TokenType.AND:
					return DefineAnd(node);
				case TokenType.OR:
					return DefineOr(node);
				case TokenType.NOT:
					return DefineNot(node);
				case TokenType.EQUALS:
					return DefineEqual(node);
				case TokenType.NOTEQUALS:
					return DefineNotEqual(node);
				case TokenType.GREATER:
					return DefineGreater(node);
				case TokenType.GREATEROREQUALS:
					return DefineGreaterOrEqual(node);
				case TokenType.LOWER:
					return DefineLower(node);
				case TokenType.LOWEROREQUALS:
					return DefineLowerOrEqual(node);
				case TokenType.REM: break;
				case TokenType.PAR: 
					return CreateParameter(node.Value);
				case TokenType.NUM: 
				case TokenType.BOOL: 
					return CreateConstant(node.Value);
				case TokenType.STARTPAR: 
					break;
				case TokenType.ENDPAR: 
					break;
			}
			
			return null;
		}
		
		private Expression CreateParameter(Token token)
		{
			if (_parameters.ContainsKey(token.Text))
				return _parameters[token.Text];
			else
			{
				this.InferParameterType(token);
				ParameterExpression par = Expression.Parameter(((ParToken)token).ParameterType, token.Text);
				_parameters.Add(token.Text, par);
				
				return par;
			}
		}
		
		private void InferParameterType(Token token)
		{
			if (_curr_index < _parametersInfo.Length)
			{
				ParameterInfo info = _parametersInfo[_curr_index];
				
				_curr_index++;
				
				if (info.IsRetval)
					InferParameterType(token);
				else
					((ParToken)token).ParameterType = info.ParameterType;
			}
		}
		
		private Expression CreateConstant(Token token)
		{
			if (token.TToken == TokenType.BOOL)
			{
				BoolToken tok = (BoolToken) token;
				return Expression.Constant(tok.GetValue(), typeof(bool));
			}
			else
			{
				NumToken tok = (NumToken) token;
				return Expression.Constant(tok.GetValue(), tok.ParameterType);
			}
		}
		
		private Expression DefineSum(BinaryNode<Token> node)
		{
			return BinaryExpression.Add(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineSub(BinaryNode<Token> node)
		{
			return BinaryExpression.Subtract(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineMult(BinaryNode<Token> node)
		{
			return BinaryExpression.Multiply(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineDiv(BinaryNode<Token> node)
		{
			return BinaryExpression.Divide(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefinePow(BinaryNode<Token> node)
		{
			return BinaryExpression.Power(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineAnd(BinaryNode<Token> node)
		{
			return BinaryExpression.AndAlso(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineOr(BinaryNode<Token> node)
		{
			return BinaryExpression.OrElse(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineNot(BinaryNode<Token> node)
		{
			return BinaryExpression.Negate(ProcessToken(node.Right));
		}
		
		private Expression DefineNotEqual(BinaryNode<Token> node)
		{
			return BinaryExpression.NotEqual(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineEqual(BinaryNode<Token> node)
		{
			return BinaryExpression.Equal(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineGreater(BinaryNode<Token> node)
		{
			return BinaryExpression.GreaterThan(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineLower(BinaryNode<Token> node)
		{
			return BinaryExpression.LessThan(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineGreaterOrEqual(BinaryNode<Token> node)
		{
			return BinaryExpression.GreaterThanOrEqual(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression DefineLowerOrEqual(BinaryNode<Token> node)
		{
			return BinaryExpression.LessThanOrEqual(ProcessToken(node.Left), ProcessToken(node.Right));
		}
		
		private Expression HandleRem(LinkedListNode<Token> node)
		{
			
			return null;
		}
		
		#endregion
	}
}
