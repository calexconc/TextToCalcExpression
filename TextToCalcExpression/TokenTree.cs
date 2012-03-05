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
		public TokenTree(IEnumerable<Token> tokens)
		{
			_roots = new Stack<BinaryNode<Token>>();
			this.Populate(tokens);
			
	//		var xx = (1==2) == ((2!=3) != false);
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
					HandleSumSub(node.Value);
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
			
			if (!eof)
				GenerateNodes(currentnode.Next);
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
		
		private LinkedListNode<Token> HandleComparisons(LinkedListNode<Token> node)
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
			
			currentnode = HandleComparisonsRigthSide(currentnode, cnode);
			this.CheckRoot(cnode);
			
			return currentnode;
		}
		
		private LinkedListNode<Token> HandleComparisonsRigthSide(LinkedListNode<Token> node, BinaryNode<Token> cnode)
		{
			LinkedListNode<Token> nextopnode = GetNextOperaror(node);
			LinkedListNode<Token> currentnode = node;
			BinaryNode<Token> candnode = null;
			
			switch (nextopnode.Value.TToken)
			{
				case TokenType.AND:
				case TokenType.OR: 
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
				BinaryNode<Token> rootnode = _roots.Pop();
				
				rootnode.Right = cnode;
				cnode.Root = rootnode;
			}
		}
		
		private LinkedListNode<Token> HandleStarPar(LinkedListNode<Token> node)
		{
			LinkedListNode<Token> currnode = GetEndParentesis(node);
			TokenTree ntree = new TokenTree(GetInParentesis(node));
			BinaryNode<Token> cnode = ntree.Root;
			
			_roots.Push(cnode);
			
			return currnode;
		}
		
		private LinkedListNode<Token> GetEndParentesis(LinkedListNode<Token> node)
		{
			LinkedListNode<Token> current = node;
			int level = 1;
			
			while (current.Value.TToken != TokenType.ENDPAR || level > 0)
			{
				current = current.Next;
				
				if (current.Value.TToken == TokenType.STARTPAR)
					level++;
				else if (current.Value.TToken == TokenType.ENDPAR)
					level--;
			}
			
			return current;
		}
		
		private IEnumerable<Token> GetInParentesis(LinkedListNode<Token> node)
		{
			LinkedListNode<Token> current = node;
			int level = 1;
				
			while (current.Value.TToken != TokenType.ENDPAR || level > 0)
			{
				if (current.Value.TToken != TokenType.STARTPAR || level > 1)
					yield return current.Value;
				
				current = current.Next;
			
				if (current.Value.TToken == TokenType.STARTPAR)
					level++;
				else if (current.Value.TToken == TokenType.ENDPAR)
				{
					level--;
					
					if (level == 0)
						yield return new EofToken();
				}
			}
		}
		
		#endregion
	}
}
