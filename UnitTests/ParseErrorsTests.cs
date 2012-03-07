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
		public void TestMethod()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<bool,bool, bool>> expression = builder.Create<Func<bool,bool, bool>>("A AND B+1");
			Func<bool,bool, bool> func = expression.Compile();
			
	//		Assert.Throws(
			
			Assert.IsTrue(func(true, true));
		}
	}
}
