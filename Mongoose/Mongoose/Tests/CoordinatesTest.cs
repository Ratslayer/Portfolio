using System;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class CoordinatesTest
	{
		/// <summary>
		/// Tests the constructor.
		/// </summary>
		[Test]
		public void TestConstructor()
		{
			Coordinates c = new Coordinates(1, 2, 3);
			Assert.NotNull(c);
			Assert.AreEqual(1, c.latitude);
			Assert.AreEqual(2, c.longitude);
			Assert.AreEqual(3, c.altitude);
			c = new Coordinates();
			Assert.NotNull(c);
		}
		/// <summary>
		/// Tests the equals.
		/// </summary>
		[Test]
		public void TestEquals()
		{
			Coordinates c1 = new Coordinates(1, 2, 3);
			Coordinates c2 = new Coordinates(2, 3, 4);
			Coordinates c3 = new Coordinates(1, 2, 3);
			//test equals
			Assert.AreEqual(true, c1.Equals(c3));
			Assert.AreEqual(false, c1.Equals(c2));
		}
		/// <summary>
		/// Tests the operators.
		/// </summary>
		[Test]
		public void TestOperators()
		{
			Coordinates c1 = new Coordinates(1, 2, 3);
			Coordinates c2 = new Coordinates(2, 3, 4);
			//test operators
			Assert.AreEqual(new Coordinates(-1, -1, -1), c1 - c2);
			Assert.AreEqual(new Coordinates(3, 5, 7), c1 + c2);
			Assert.AreEqual(new Coordinates(2, 6, 12), c1 * c2);
			Assert.AreEqual(new Coordinates(0.5, 2.0 / 3.0, 0.75), c1 / c2);
			Assert.AreEqual(new Coordinates(3, 6, 9), c1 * 3);
			Assert.AreEqual(new Coordinates(1, 1.5, 2), c2 / 2);
		}
		/// <summary>
		/// Tests the length.
		/// </summary>
		[Test]
		public void TestLength()
		{
			Assert.AreEqual(0, new Coordinates(0, 0, 0).Length);
			Assert.AreEqual(1, new Coordinates(1, 0, 0).Length);
		}
		/// <summary>
		/// Tests to string.
		/// </summary>
		[Test]
		public void TestToString()
		{
			Assert.AreEqual("1 2 3", new Coordinates(1, 2, 3).ToString());
		}
		/// <summary>
		/// Tests the minimum max.
		/// </summary>
		[Test]
		public void TestMinMax()
		{
			Coordinates c1 = new Coordinates(4, -2, 3);
			Coordinates c2 = new Coordinates(2, 5, 0);
			Assert.AreEqual(new Coordinates(2, -2, 0), Coordinates.Min(c1, c2));
			Assert.AreEqual(new Coordinates(4, 5, 3), Coordinates.Max(c1, c2));
		}
		/// <summary>
		/// Tests the clamp.
		/// </summary>
		[Test]
		public void TestClamp()
		{
			Coordinates c1, c2, c3;
			c1 = new Coordinates(-3, -1, 0);
			c2 = new Coordinates(5, 3, 2);
			c3 = new Coordinates(10, -10, 1);
			Assert.AreEqual(new Coordinates(5, -1, 1), Coordinates.Clamp(c3, c1, c2));
		}
	}
}