﻿using System;
using System.Linq;
using System.Collections.Generic;
using TextToCalcExpression.Tokens;
using TextToCalcExpression.Collections;

namespace TextToCalcExpression
{
	/// <summary>
	/// Description of TokenTree.
	/// </summary>
	public class TokenTree : BinaryTree<Token>
	{	
		private LinkedListNode<Token> _current;
		
		public TokenTree(IEnumerable<Token> tokens)
		{
			_roots = new Stack<BinaryNode<Token>>();
			this.Populate(tokens);
		}
		
		protected override void Populate(IEnumerable<Token> values)
		{
			LinkedList<Token> lnkList = new LinkedList<Token>(values);
			
			GenerateNodes(lnkList.First);
		}
		
		#region private members
		
		private void GenerateNodes(LinkedListNode<Token> node)
		{
			LinkedListNode<Token> currentnode = node;
			bool eof = false;
			
			switch (node.Value.TToken)
			{
				case TokenType.SUM: 
				case TokenType.SUB: 
					currentnode = HandleSumSub(node);
					break;
				case TokenType.AND:
				case TokenType.OR:
					currentnode = HandleAndOr(node);
					break;
				case TokenType.MULT:
				case TokenType.DIV: 
					currentnode = this.HandleMultDiv(currentnode);
					break;
				case TokenType.NOT:
					break;
				case TokenType.POW: 
				case TokenType.REM:
					break;
				case TokenType.EQUALS:
				case TokenType.NOTEQUALS:
				case TokenType.GREATER:
				case TokenType.GREATEROREQUALS:
				case TokenType.LOWER:
				case TokenType.LOWEROREQUALS:
					currentnode = HandleComparisons(currentnode);
					break;
				case TokenType.STARTPAR: 
					currentnode = HandleStarPar(currentnode);
					break;
				case TokenType.ENDPAR: 
					break;
				case TokenType.PAR:
				case TokenType.NUM:
					this.HandleParConst(node.Value);
					break;
				case TokenType.EOF: 
					this.HandleEOF();
					eof = true;
					break;
				default:
					break;
			}
			
			if (currentnode.Value.TToken != TokenType.EOF)
				GenerateNodes(currentnode.Next);
			else if (!eof)
				GenerateNodes(currentnode);
		}
		
		
		private void CheckRoot(BinaryNode<Token> cnode)
		{
			if (this.Root == null && cnode.Value.GToken == TokenGroup.Operator)
				this.Root = cnode;
			else if (this.Root != null && this.Root.Root !=null)
				this.Root = this.Root.Root;
		}
		
		private LinkedListNode<Token> HandleSumSub(LinkedListNode<Token> node)
		{
			Func<LinkedListNode<Token>,bool> func = inode => { 
				switch (inode.Value.TToken)
				{
					case TokenType.SUM:
					case TokenType.SUB:
					case TokenType.EOF:
						return false;
					default:
						return true;
				}
			};
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> cnode = this.HandleCommon(node);
			currentnode = HandleRigthSide(currentnode, cnode, func);
			_roots.Push(cnode);
			
			this.CheckRoot(cnode);
			
			return currentnode;
		}
		
		private LinkedListNode<Token> HandleMultDiv(LinkedListNode<Token> node)
		{
			Func<LinkedListNode<Token>,bool> func = inode => { 
				switch (inode.Value.TToken)
				{
					case TokenType.SUM:
					case TokenType.SUB:
					case TokenType.EOF:
						return false;
					default:
						return true;
				}
			};
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> cnode = this.HandleCommon(node);
			
			currentnode = HandleRigthSide(currentnode, cnode, func);
			_roots.Push(cnode);
			this.CheckRoot(cnode);
			
			return currentnode;
		}
		
		private LinkedListNode<Token> HandleAndOr(LinkedListNode<Token> node)
		{
			Func<LinkedListNode<Token>,bool> func = inode => { 
				switch (inode.Value.TToken)
				{
					case TokenType.AND:
					case TokenType.OR:
					case TokenType.EOF:
						return false;
					default:
						return true;
				}
			};
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> cnode = this.HandleCommon(node);
			
			currentnode = HandleRigthSide(currentnode, cnode, func);
			_roots.Push(cnode);
			this.CheckRoot(cnode);
			
			return currentnode;
		}
		
		private LinkedListNode<Token> HandleComparisons(LinkedListNode<Token> node)
		{
			Func<LinkedListNode<Token>,bool> func = inode => { 
				switch (inode.Value.TToken)
				{
					case TokenType.AND:
					case TokenType.OR:
					case TokenType.EQUALS:
					case TokenType.NOTEQUALS:
					case TokenType.GREATER:
					case TokenType.GREATEROREQUALS:
					case TokenType.LOWER:
					case TokenType.LOWEROREQUALS:
					case TokenType.EOF:
						return false;
					default:
						return true;
				}
			};
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> cnode = this.HandleCommon(node);
			
			currentnode = HandleRigthSide(currentnode, cnode, func);
			_roots.Push(cnode);
			this.CheckRoot(cnode);
			
			return currentnode;
		}
		
		private BinaryNode<Token> HandleCommon(LinkedListNode<Token> node)
		{
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> cnode = this.CreateNode(node.Value);
			BinaryNode<Token> candnode = _roots.Pop();
			
			if (_roots.Count > 0)
			{
				BinaryNode<Token> rootnode = _roots.Pop();
				rootnode.Right = candnode;
				candnode.Root = rootnode;
				cnode.Left = rootnode;
			}
			else
			{
				cnode.Left = candnode;
				candnode.Root = cnode;
			}
			
			return cnode;
		}
		
		private void HandleParConst(Token token)
		{
			BinaryNode<Token> cnode = this.CreateNode(token);
			_roots.Push(cnode);
		}
		
		private void HandleEOF()
		{
			if (_roots.Count > 0)
			{
				BinaryNode<Token> cnode = _roots.Pop();
				
				if (_roots.Count > 0)
				{
					BinaryNode<Token> rootnode = _roots.Pop();
					rootnode.Right = cnode;
					cnode.Root = rootnode;
				}
				else if (this.Root == null)
					this.Root = cnode;
			}
		}
		
		private LinkedListNode<Token> HandleRigthSide(LinkedListNode<Token> node, BinaryNode<Token> cnode, Func<LinkedListNode<Token>,bool> func)
		{
			LinkedListNode<Token> currnode = null;
			BinaryNode<Token> rnode = HandleRigthSide(node, func);
			
			currnode = _current;
			cnode.Right = rnode;
			rnode.Root = cnode.Right;
			
			return currnode;
		}
		
		private BinaryNode<Token> HandleRigthSide(LinkedListNode<Token> node, Func<LinkedListNode<Token>,bool> func)
		{
			BinaryNode<Token> rnode = null;
			int level = 0;
			
			if (node.Next.Value.TToken == TokenType.STARTPAR)
				level = 1;
			
			TokenTree ntree = new TokenTree(GetRightSideExpressionTokens(node.Next, func, level));
			_current = _current.Previous;
			rnode = ntree.Root;
			
			return rnode;
		}
		
		private LinkedListNode<Token> HandleStarPar(LinkedListNode<Token> node)
		{
			Func<LinkedListNode<Token>,bool> func = inode => { 
				switch (inode.Value.TToken)
				{
					case TokenType.ENDPAR:
						return false;
					default:
						return true;
				}
			};
			TokenTree ntree = new TokenTree(GetParExpressionTokens(node, func));
			LinkedListNode<Token> currnode = _current;
			BinaryNode<Token> cnode = ntree.Root;
			
			_roots.Push(cnode);
			
			return currnode;
		}
		
		private IEnumerable<Token> GetRightSideExpressionTokens(LinkedListNode<Token> node, Func<LinkedListNode<Token>,bool> func, int starparlevel)
		{
			LinkedListNode<Token> current = node;
			int level = starparlevel;
				
			while (func(current) || level > 0)
			{
				yield return current.Value;
				
				current = current.Next;
			
				if (current.Value.TToken == TokenType.STARTPAR)
					level++;
				else if (current.Value.TToken == TokenType.ENDPAR)
					level--;
			}
			
			_current = current;
			
			yield return new EofToken();
		}
		
		private IEnumerable<Token> GetParExpressionTokens(LinkedListNode<Token> node, Func<LinkedListNode<Token>,bool> func)
		{
			LinkedListNode<Token> current = node;
			int level = 1;
				
			while (func(current) || level > 0)
			{
				if (current.Value.TToken != TokenType.STARTPAR || level > 1)
					yield return current.Value;
				
				current = current.Next;
			
				if (current.Value.TToken == TokenType.STARTPAR)
					level++;
				else if (current.Value.TToken == TokenType.ENDPAR)
					level--;
			}
			
			_current = current;
			
			yield return new EofToken();
		}
		
		#endregion
	}
}
