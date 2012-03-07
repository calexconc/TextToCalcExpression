using System;
using NUnit.Framework;
using System.Linq.Expressions;
using TextToCalcExpression;

namespace UnitTests
{
	[TestFixture]
	public class BooleanOperationsTests
	{
		
		[Test]
		public void TupleAnd_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A AND B");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, true));
		}
		
		[Test]
		public void TupleAndNot_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A AND NOT B");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsFalse(func(true, true));
		}
		
		[Test]
		public void TupleAndNotOR_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A AND NOT B OR A");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, true));
		}
		
		[Test]
		public void TupleOR_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A OR B");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, false));
		}
		
		
		[Test]
		public void TupleLogicMix_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool, bool>> expression = builder.Create<Func<bool,bool, bool, bool>>("A OR B AND C");
			Func<bool,bool, bool, bool> func = expression.Compile();
			
			Assert.IsFalse(func(true, false, false));
		}
		
		[Test]
		public void TupleLogicMix2_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool, bool>> expression = builder.Create<Func<bool,bool, bool, bool>>("A OR NOT(B AND C)");
			Func<bool,bool, bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, false, false));
		}
		
		[Test]
		public void TupleANDWithComparisonEquals_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A==A AND B==B");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, false));
		}
		
		[Test]
		public void TupleANDWithORComparisonMixedEquals_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A==A AND A!=B OR B==B");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, false));
		}
		
		[Test]
		public void TupleANDWithComparisonMixedParentesisEquals_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A==A AND (A!=B OR B==B)");
			Func<bool,bool, bool> func = expression.Compile();
			
			Assert.IsTrue(func(true, false));
		}
		
		[Test]
		public void TupleANDComparison_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, bool>> expression = builder.Create<Func<int,int, bool>>("A <= B AND B > 1");
			Func<int,int, bool> func = expression.Compile();
			
			Assert.IsTrue(func(1, 3));
		}
		
		[Test]
		public void TupleANDArithmeticWithComparison_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, bool>> expression = builder.Create<Func<int,int, bool>>("A + 1 <= B AND B > 1");
			Func<int,int, bool> func = expression.Compile();
			
			Assert.IsTrue(func(1, 3));
		}
	}
}
