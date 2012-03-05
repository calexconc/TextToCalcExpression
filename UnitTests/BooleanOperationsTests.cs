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
	}
}
