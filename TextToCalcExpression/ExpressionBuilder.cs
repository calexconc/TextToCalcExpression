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
		Dictionary<string, ParameterInfo> _mapParam;
		TokenTree _tree;
		ParameterInfo[] _parametersInfo;
		ParameterInfo _returnType;
		
		public ExpressionBuilder()
		{
		}
		
		public Expression<T> Create<T>(string expression)
		{
			_parameters = new Dictionary<string, ParameterExpression>();
			_mapParam = new Dictionary<string, ParameterInfo>();
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
				case TokenType.REM: break;
				case TokenType.PAR: 
					return CreateParameter(node.Value);
				case TokenType.NUM: 
					return CreateConstant(node.Value);
				case TokenType.STARTPAR: 
					break;
				case TokenType.ENDPAR: 
					break;
			}
			
			return null;
		}
		
		private Expression CreateParameter(Token node)
		{
			if (_parameters.ContainsKey(node.Text))
				return _parameters[node.Text];
			else
			{
				ParameterExpression par = Expression.Parameter(((ParToken)node).ParameterType, node.Text);
				_parameters.Add(node.Text, par);
				
				return par;
			}
		}
		
		private Expression CreateConstant(Token node)
		{
			NumToken tok = (NumToken) node;
			
			return Expression.Constant(tok.GetValue(), tok.ParameterType);
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
		
		private Expression HandleRem(LinkedListNode<Token> node)
		{
			
			return null;
		}
		
		#endregion
	}
}
