/*
 * Created by SharpDevelop.
 * User: calex
 * Date: 07-03-2012
 * Time: 14:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using System.Linq.Expressions;
using TextToCalcExpression;

namespace UnitTests
{
	[TestFixture]
	public class FunctionsTests
	{
		[Test]
		public void Cos_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int>> expression = builder.Create<Func<int, int>>("COS A");
			Func<int,int> func = expression.Compile();
						
			Assert.AreEqual(func(0), 1);
		}
		
		[Test]
		public void Cos_Test2()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("COS -A");
			Func<double,double> func = expression.Compile();
						
			Assert.AreEqual(func(Math.PI), -1);
		}
		
		[Test]
		public void Cos_Test3()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("-COS -A");
			Func<double,double> func = expression.Compile();
						
			Assert.AreEqual(func(Math.PI), 1);
		}
		
		
		[Test]
		public void Sin_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("SIN(A/2)");
			Func<double,double> func = expression.Compile();
						
			Assert.AreEqual(func(Math.PI), 1);
		}
		
		[Test]
		public void trig_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("SIN(A)^2+COS(A)^2");
			Func<double,double> func = expression.Compile();
						
			Assert.AreEqual(func(Math.PI), 1);
		}
		
		[Test]
		public void trig_Test2()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("SIN(A)^2+COS(PI)^2");
			Func<double,double> func = expression.Compile();
						
			Assert.AreEqual(func(Math.PI), 1);
		}
		
		[Test]
		public void Exp_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("EXP A");
			Func<double,double> func = expression.Compile();
						
			Assert.AreEqual(func(0), 1);
		}
		
		[Test]
		public void Ln_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("LN A");
			Func<double,double> func = expression.Compile();
			
			Assert.AreEqual(func(Math.E), 1);
		}
		
		[Test]
		public void Abs_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<int,int>> expression = builder.Create<Func<int, int>>("ABS -A");
			Func<int,int> func = expression.Compile();
						
			Assert.AreEqual(func(-1), 1);
		}
		
		[Test]
		public void SQRT_Test()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			Expression<Func<double,double>> expression = builder.Create<Func<double, double>>("SQRT A");
			Func<double,double> func = expression.Compile();
						
			Assert.AreEqual(func(25), 5);
		}
	}
}
