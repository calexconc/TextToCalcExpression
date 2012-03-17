using System;
using NUnit.Framework;
using System.Linq.Expressions;
using TextToCalcExpression;

namespace UnitTests
{
	[TestFixture]
	public class ParseErrorsTests
	{
		[Test]
		[ExpectedException(typeof(ParseException))]
		public void EmptyExpression_Test()
		{
			
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool>> expression = builder.Create<Func<bool>>("");
			Func<bool> func = expression.Compile();
			
			Assert.IsTrue(func());
		}
		
		[Test]
		[ExpectedException(typeof(ParseException))]
		public void EmptyParentesisExpression_Test()
		{
			
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int>> expression = builder.Create<Func<int>>("2+1+()-1");
			Func<int> func = expression.Compile();
			
			Assert.AreEqual(func(), 0);
		}
		
		[Test]
		[ExpectedException(typeof(ParseException))]
		public void EmptyParentesisExpression2_Test()
		{
			
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int>> expression = builder.Create<Func<int>>("2+1+(-1");
			Func<int> func = expression.Compile();
			
			Assert.AreEqual(func(), 0);
		}
		
		[Test]
		[ExpectedException(typeof(ParseException))]
		public void EmptyParentesisExpression3_Test()
		{
			
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int>> expression = builder.Create<Func<int>>("2+1+(2+1*(5)");
			Func<int> func = expression.Compile();
			
			Assert.AreEqual(func(), 0);
		}
	}
}
