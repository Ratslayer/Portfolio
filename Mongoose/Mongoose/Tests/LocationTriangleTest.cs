using System;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class LocationTriangleTest
	{
		/// <summary>
		/// Tests the constructor.
		/// </summary>
		[Test]
		public void TestConstructor()
		{
			LocationTriangle triangle = new LocationTriangle(new Coordinates(1, 1, 0), new Coordinates(-1, 4, 5), new Coordinates(3, 4, -3));
			Assert.NotNull(triangle);
		}
		/// <summary>
		/// Tests the contains.
		/// </summary>
		[Test]
		public void TestContains()
		{
			LocationTriangle triangle = new LocationTriangle(new Coordinates(1, 1, 0), new Coordinates(-1, 4, 5), new Coordinates(3, 4, -3));
			//test Triangle::Contains(Coordinate)
			Assert.AreEqual(true, triangle.Contains(new Coordinates(1, 3, 10)));
			Assert.AreEqual(true, triangle.Contains(new Coordinates(2, 4, 7)));
			Assert.AreEqual(false, triangle.Contains(new Coordinates(0, 0, 0)));
		}
		/// <summary>
		/// Tests the get points.
		/// </summary>
		[Test]
		public void TestGetPoints()
		{
			LocationTriangle triangle = new LocationTriangle(new Coordinates(1, 1, 0), new Coordinates(-1, 4, 5), new Coordinates(3, 4, -3));
			Coordinates[] coordsArray = triangle.GetPoints();
			Assert.AreEqual(new Coordinates(1, 1, 0), coordsArray[0]);
			Assert.AreEqual(new Coordinates(-1, 4, 5), coordsArray[1]);
			Assert.AreEqual(new Coordinates(3, 4, -3), coordsArray[2]);
		}
		/// <summary>
		/// Tests the location triangle.
		/// </summary>
		[Test]
		public void TestLocationTriangle()
		{
			//test constructors
			LocationTriangle triangle = new LocationTriangle(new Coordinates(1, 1, 0), new Coordinates(-1, 4, 5), new Coordinates(3, 4, -3));
			//test Triangle::Contains(Coordinate)
			Assert.AreEqual(true, triangle.Contains(new Coordinates(1, 3, 10)));
			Assert.AreEqual(true, triangle.Contains(new Coordinates(2, 4, 7)));
			Assert.AreEqual(false, triangle.Contains(new Coordinates(0, 0, 0)));
			//test Triangle::GetPoints()
			Coordinates[] coordsArray = triangle.GetPoints();
			Assert.AreEqual(new Coordinates(1, 1, 0), coordsArray[0]);
			Assert.AreEqual(new Coordinates(-1, 4, 5), coordsArray[1]);
			Assert.AreEqual(new Coordinates(3, 4, -3), coordsArray[2]);
		}
	}
}

