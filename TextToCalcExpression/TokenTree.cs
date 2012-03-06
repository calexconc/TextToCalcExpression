using System;
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
					HandleSumSub(node.Value);
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
		
		private LinkedListNode<Token> GetNextOperaror(LinkedListNode<Token> node)
		{
			LinkedListNode<Token> curr = node.Next;
			
			while (curr != null && (curr.Value.GToken != TokenGroup.Operator && curr.Value.GToken != TokenGroup.Separator))
			{
				curr = curr.Next;
			}
		
			return curr;
		}
		
		private void CheckRoot(BinaryNode<Token> cnode)
		{
			if (this.Root == null && cnode.Value.GToken == TokenGroup.Operator)
				this.Root = cnode;
			else if (this.Root != null && this.Root.Root !=null)
				this.Root = this.Root.Root;
		}
		
		private LinkedListNode<Token> HandleMultDiv(LinkedListNode<Token> node)
		{
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> cnode = this.CreateNode(node.Value);
			cnode.Left = _roots.Pop();
			cnode.Left.Root = cnode;
			
			if (_roots.Count > 0)
			{
				BinaryNode<Token> rootnode = _roots.Pop();
				rootnode.Right = cnode;
				cnode.Root = rootnode;
			}
			
			currentnode = HandleMultDivRigthSide(currentnode, cnode);
			this.CheckRoot(cnode);
			
			return currentnode;
		}
		
		private LinkedListNode<Token> HandleMultDivRigthSide(LinkedListNode<Token> node, BinaryNode<Token> cnode)
		{
			LinkedListNode<Token> nextopnode = GetNextOperaror(node);
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> candnode = null;
			
			switch (nextopnode.Value.TToken)
			{
				case TokenType.SUM:
				case TokenType.SUB: 
					candnode = this.CreateNode(currentnode.Next.Value);
					currentnode = currentnode.Next;
					cnode.Right = candnode;
					candnode.Root = cnode;
					_roots.Push(this.Root);
					break;
				case TokenType.STARTPAR: 
					currentnode = HandleStarPar(nextopnode);
					candnode = _roots.Pop();
					cnode.Right = candnode;
					candnode.Root = cnode;
					_roots.Push(cnode);
					_roots.Push(candnode);
					break;
				default:
					_roots.Push(cnode);
					break;
			}
			
			return currentnode;
		}
		
		private void HandleSumSub(Token token)
		{
			BinaryNode<Token> cnode = this.CreateNode(token);
			BinaryNode<Token> candnode = _roots.Pop();
			
			if (_roots.Count > 0)
			{
				BinaryNode<Token>  rootnode = _roots.Pop();
				
				if (rootnode.Right== null)
				{
					rootnode.Right = cnode;
					cnode.Root = rootnode;
				}
				else if (rootnode.Root == null)
				{
					cnode.Left = rootnode;
					rootnode.Root = cnode;
				}
			}
			
			if (cnode.Left == null)
				cnode.Left = candnode;
			
			candnode.Root = cnode;
			_roots.Push(cnode);
			this.CheckRoot(cnode);
		}
		
		private LinkedListNode<Token> HandleAndOr(LinkedListNode<Token> node)
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
			
			currentnode = HandleAndOrRigthSide(currentnode, cnode);
			_roots.Push(cnode);
			this.CheckRoot(cnode);
			
			return currentnode;
		}
		
		private LinkedListNode<Token> HandleComparisons(LinkedListNode<Token> node)
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
			
			currentnode = HandleComparisonsRigthSide(currentnode, cnode);
			this.CheckRoot(cnode);
			
			return currentnode;
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
		
		private LinkedListNode<Token> HandleComparisonsRigthSide(LinkedListNode<Token> node, BinaryNode<Token> cnode)
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
			
			TokenTree ntree = new TokenTree(GetRightSideExpressionTokens(node.Next, func, 0));
			LinkedListNode<Token> currnode = _current.Previous;
			
			cnode.Right = ntree.Root;
			ntree.Root.Root = cnode.Right;
			
			return currnode;
		}
		
		private LinkedListNode<Token> HandleAndOrRigthSide(LinkedListNode<Token> node, BinaryNode<Token> cnode)
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
			TokenTree ntree = new TokenTree(GetRightSideExpressionTokens(node.Next, func, 0));
			LinkedListNode<Token> currnode = _current.Previous;
			
			cnode.Right = ntree.Root;
			ntree.Root.Root = cnode.Right;
			
			return currnode;
		}
		
		private LinkedListNode<Token> HandleStarPar(LinkedListNode<Token> node)
		{
			Func<LinkedListNode<Token>,bool> func = inode => { 
				switch (inode.Next.Value.TToken)
				{
					case TokenType.ENDPAR:
						return false;
					default:
						return true;
				}
			};
			TokenTree ntree = new TokenTree(GetRightSideExpressionTokens(node, func, 1));
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
