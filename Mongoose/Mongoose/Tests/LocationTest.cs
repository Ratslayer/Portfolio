using System;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class LocationTest
	{
		/// <summary>
		/// Tests the location bounding box.
		/// </summary>
		[Test]
		public void TestLocationBoundingBox()
		{
			//test constructors
			LocationBoundingBox box = new LocationBoundingBox(new Coordinates(-2, -4, 1), new Coordinates(3, 5, 7));
			//test properties
			Assert.AreEqual(new Coordinates(-2, -4, 1), box.Min);
			Assert.AreEqual(new Coordinates(3, 5, 7), box.Max);
			//test LocationBoundingBox::Contains(Coordinates)
			Assert.AreEqual(true, box.Contains(new Coordinates(0, 0, 4)));
			Assert.AreEqual(true, box.Contains(new Coordinates(-2, 5, 7)));
			Assert.AreEqual(false, box.Contains(new Coordinates(-3, 6, 0)));
			//test LocationBoundingBox::Equals(Object)
			Assert.AreEqual(true, box.Equals(new LocationBoundingBox(new Coordinates(-2, -4, 1), new Coordinates(3, 5, 7))));
			Assert.AreEqual(false, box.Equals(new LocationBoundingBox(new Coordinates(-2, -4, 1), new Coordinates(1, 3, 2))));
			//test operators
			Assert.AreEqual(new LocationBoundingBox(new Coordinates(-4, -8, 2), new Coordinates(6, 10, 14)), box * 2);
			//test static functions
			LocationBoundingBox box2 = LocationBoundingBox.FromPoints(new Coordinates[] {
				new Coordinates(-2, 0, 7),
				new Coordinates(0, -4, 1),
				new Coordinates(3, 5, 4)
			});
			Assert.AreEqual(box, box2);

			LocationBoundingBox[] boxes = new LocationBoundingBox[]{ box, box * 2 };
			LocationBoundingBox box3 = LocationBoundingBox.FromBoundingBoxes(boxes);
			Assert.AreEqual(new LocationBoundingBox(new Coordinates(-4, -8, 1), new Coordinates(6, 10, 14)), box3);
			//test intersects
			Assert.AreEqual(true, box.Intersects(new LocationBoundingBox(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10))));
			Assert.AreEqual(false, box.Intersects(new LocationBoundingBox(new Coordinates(10, 10, 10), new Coordinates(20, 20, 20))));
			Assert.AreEqual(false, box.Intersects(new LocationBoundingBox(new Coordinates(-20, -20, -20), new Coordinates(-10, -10, -10))));
			Assert.AreEqual(true, box.Intersects(new LocationBoundingBox(new Coordinates(0, 0, 0), new Coordinates(1, 1, 1))));
			//test tostring
			Assert.AreEqual("[LocationBoundingBox: Min=-2 -4 1, Max=3 5 7]", box.ToString());
		}
		/// <summary>
		/// Tests the location mesh.
		/// </summary>
		[Test]
		public void TestLocationMesh()
		{
			//constructor and initialization
			LocationMesh mesh = new LocationMesh();
			mesh.AddTriangle(new LocationTriangle(new Coordinates(0, 1, 0), new Coordinates(4, 3, 0), new Coordinates(4, 5, 0)));
			mesh.AddTriangle(new LocationTriangle(new Coordinates(0, 1, 0), new Coordinates(5, 3, 0), new Coordinates(-4, 5, 0)));
			//test LocationMesh::Contains(Coordinates)
			Assert.AreEqual(true, mesh.Contains(new Coordinates(3.9, 3.9, 0)));
			Assert.AreEqual(true, mesh.Contains(new Coordinates(0, 3, 0)));
			Assert.AreEqual(false, mesh.Contains(new Coordinates(0, -1, 0)));
			//test LocationMesh::GetBoundingBox()
			Assert.AreEqual(new LocationBoundingBox(new Coordinates(-4, 1, 0), new Coordinates(5, 5, 0)), mesh.GetBoundingBox());
			//test LocationMesh::GetCenter()
			Assert.AreEqual(new Coordinates(1.5, 3, 0), mesh.GetCenter());
			//test LocationMesh::GetTriangles()
			Assert.AreEqual(2, mesh.GetTriangles().Length);
		}
		/// <summary>
		/// Tests the fake location.
		/// </summary>
		[Test]
		public void TestFakeLocation()
		{
			FakeLocation location = new FakeLocation();
			Assert.AreEqual(new Coordinates(0, 0, 0), location.Coords);
			Assert.AreEqual("H", location.Building);
			Assert.AreEqual(strings.NoAddress, location.Address);
		}
		/// <summary>
		/// Tests the coordinates.
		/// </summary>
		[Test]
		public void TestCoordinates()
		{
			//test constructors
			Coordinates c1 = new Coordinates(1, 2, 3);
			Coordinates c2 = new Coordinates(2, 3, 4);
			Coordinates c3 = new Coordinates(1, 2, 3);
			//test equals
			Assert.AreEqual(true, c1.Equals(c3));
			Assert.AreEqual(false, c1.Equals(c2));
			//test operators
			Assert.AreEqual(new Coordinates(-1, -1, -1), c1 - c2);
			Assert.AreEqual(new Coordinates(3, 5, 7), c1 + c2);
			Assert.AreEqual(new Coordinates(2, 6, 12), c1 * c2);
			Assert.AreEqual(new Coordinates(0.5, 2.0 / 3.0, 0.75), c1 / c2);
			Assert.AreEqual(new Coordinates(3, 6, 9), c1 * 3);
			Assert.AreEqual(new Coordinates(1, 1.5, 2), c2 / 2);
			//test utility functions
			Assert.AreEqual(0, new Coordinates(0, 0, 0).Length);
			Assert.AreEqual(1, new Coordinates(1, 0, 0).Length);
			Assert.AreEqual("1 2 3", c1.ToString());
			//test static functions
			c1 = new Coordinates(4, -2, 3);
			c2 = new Coordinates(2, 5, 0);
			Assert.AreEqual(new Coordinates(2, -2, 0), Coordinates.Min(c1, c2));
			Assert.AreEqual(new Coordinates(4, 5, 3), Coordinates.Max(c1, c2));
		}
		/// <summary>
		/// Tests the building location collection.
		/// </summary>
		[Test]
		public void TestBuildingLocationCollection()
		{
			BuildingLocationCollection blc = new BuildingLocationCollection();
			string bld = blc.GetBuilding(new FakeLocation());
			Assert.AreEqual("Unknown", bld);
			Assert.AreEqual(new LocationBoundingBox(new Coordinates(45.493868, -73.581079, 0), new Coordinates(45.49869, -73.576094, 0)), blc.GetBoundingBox());
			Assert.NotNull(blc.GetMesh("H"));
			Assert.Null(blc.GetMesh("sparrow"));
			Assert.AreEqual(4, blc.GetBuildingCenters().Count);
			Assert.AreEqual(4, blc.GetBuildingNames().Length);

		}
	}
}