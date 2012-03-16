using System;
using NUnit.Framework;
using System.Linq.Expressions;
using TextToCalcExpression;

namespace UnitTests
{
	[TestFixture]
	public class EvalTests
	{
		[Test]
		public void EvaluateNumericalValue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int>> expression = builder.Create<Func<int>>("9");
			Func<int> func = expression.Compile();
			
			Assert.AreEqual(func(), 9);
		}
		
		[Test]
		public void EvaluateBoolValue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool>> expression = builder.Create<Func<bool>>("false");
			Func<bool> func = expression.Compile();
			
			Assert.AreEqual(func(), false);
		}
		
		[Test]
		public void EvaluateNumericalOperation_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int>> expression = builder.Create<Func<int>>("9+1");
			Func<int> func = expression.Compile();
			
			Assert.AreEqual(func(), 10);
		}
		
		
		[Test]
		public void EvaluateNumericalOperation2_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int>> expression = builder.Create<Func<int>>("2^3");
			Func<int> func = expression.Compile();
			
			Assert.AreEqual(func(), 8);
		}
		
		[Test]
		public void EvaluateNumericalOperation3_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int>> expression = builder.Create<Func<int>>("(2^3+2-1)/3");
			Func<int> func = expression.Compile();
			
			Assert.AreEqual(func(), 3);
		}
		
		[Test]
		public void EvaluateBoolExpression_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool>> expression = builder.Create<Func<bool>>("false OR true");
			Func<bool> func = expression.Compile();
			
			Assert.AreEqual(func(), true);
		}
		
		[Test]
		public void EvaluateBoolExpression2_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool>> expression = builder.Create<Func<bool>>("NOT(false) AND true");
			Func<bool> func = expression.Compile();
			
			Assert.AreEqual(func(), true);
		}
		
		[Test]
		public void EvaluateBoolExpression3_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool>> expression = builder.Create<Func<bool>>("2 > 3 AND 1 >= 2");
			Func<bool> func = expression.Compile();
			
			Assert.AreEqual(func(), false);
		}
	}
}
