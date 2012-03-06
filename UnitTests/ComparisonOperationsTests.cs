using System;
using NUnit.Framework;
using System.Linq.Expressions;
using TextToCalcExpression;

namespace UnitTests
{
	[TestFixture]
	public class ComparisonOperationsTests
	{
		[Test]
		public void EqualsTupleTrue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A == B");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, true));
		}
		
		[Test]
		public void NotEqualsTupleTrue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A != B");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(false, true));
		}
		
		[Test]
		public void GreaterTupleTrue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, bool>> expression = builder.Create<Func<int,int, bool>>("A > B");
			Func<int,int, bool> func = expression.Compile();
			
			Assert.IsTrue(func(3, 0));
		}
		
		[Test]
		public void LowerTupleTrue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, bool>> expression = builder.Create<Func<int,int, bool>>("A < B");
			Func<int,int, bool> func = expression.Compile();
			
			Assert.IsTrue(func(1, 3));
		}
		
		[Test]
		public void GreaterOrEqualTupleTrue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, bool>> expression = builder.Create<Func<int,int, bool>>("A >= B");
			Func<int,int, bool> func = expression.Compile();
			
			Assert.IsTrue(func(3, 0));
		}
		
		[Test]
		public void LowerOrEqualTupleTrue_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, bool>> expression = builder.Create<Func<int,int, bool>>("A <= B");
			Func<int,int, bool> func = expression.Compile();
			
			Assert.IsTrue(func(1, 3));
		}
		
	}
}
