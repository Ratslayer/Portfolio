using System;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture()]
	public class POITest
	{
		/// <summary>
		/// Tests the POI creation.
		/// </summary>
		[Test]
		public void TestPOICreation()
		{
			POI poi = new POI();
			poi.title = "POI Test";
			poi.description = "Testing";
			poi.campus = "EV";
			poi.building = "H";

			Assert.True(poi.title.Equals("POI Test"));
		}
		/// <summary>
		/// Tests the POI creation with fields.
		/// </summary>
		[Test]
		public void TestPOICreationWithFields()
		{
			Coordinates coords = new Coordinates(0, 0, 0);
			POI poi = new POI("POI Test", "This is a test for the other constructor", coords);
			Assert.True(poi.title.Equals("POI Test"));
			Assert.True(poi.description.Equals("This is a test for the other constructor"));
		}
	}
}

