using System;
using NUnit.Framework;
using System.Linq.Expressions;
using TextToCalcExpression;

namespace UnitTests
{
	[TestFixture]
	/// <summary>
	/// Description of ArithmeticTests.
	/// </summary>
	public class ArithmeticTests
	{
		[Test]
		public void TwoArgsSum_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, int>> expression = builder.Create<Func<int,int, int>>("A + B");
			Func<int,int, int> func = expression.Compile();
			
			Assert.AreEqual(func(1, 3), 4);
		}
		
		[Test]
		public void TwoArgsSubtraction_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, int>> expression = builder.Create<Func<int,int, int>>("A - B");
			Func<int,int, int> func = expression.Compile();
			
			Assert.AreEqual(func(2, 2), 0);
		}
		
		[Test]
		public void TwoArgsMultiplication_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, int>> expression = builder.Create<Func<int,int, int>>("A * B");
			Func<int,int, int> func = expression.Compile();
			
			Assert.AreEqual(func(16, 2), 32);
		}
		
		[Test]
		public void TwoArgsDivision_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, int>> expression = builder.Create<Func<int,int, int>>("A / B");
			Func<int,int, int> func = expression.Compile();
			
			Assert.AreEqual(func(16, 2), 8);
		}
	}
}
