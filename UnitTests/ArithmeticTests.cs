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
		
		[Test]
		public void Poli3ArgsFunction_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, int,int>> expression = builder.Create<Func<int,int, int, int>>("A * B+C");
			Func<int,int, int, int> func = expression.Compile();
			
			Assert.AreEqual(func(16, 2, -2), 30);
		}
		
		[Test]
		public void Poli3ArgsPlusConstantFunction_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, int,int>> expression = builder.Create<Func<int,int, int, int>>("A * B+C+6");
			Func<int,int, int, int> func = expression.Compile();
			
			Assert.AreEqual(func(16, 2, -2), 36);
		}
		
		[Test]
		public void Poli3ArgsPlusConstantPlusParentesisFunction_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int, int,int>> expression = builder.Create<Func<int,int, int, int>>("A * (B+C)+6");
			Func<int,int, int, int> func = expression.Compile();
			
			Assert.AreEqual(func(16, 2, -2), 6);
		}
	}
}
