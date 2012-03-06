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
	}
}
