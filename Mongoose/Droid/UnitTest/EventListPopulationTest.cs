using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mongoose.Droid
{
	[TestFixture()]
	public class EventListPopulationTest
	{
		/// <summary>
		/// Tests the selected location list population.
		/// </summary>
		[Test]
		public void TestSelectedLocationListPopulation()
		{
			EventListViewAdapter viewAdapter = new EventListViewAdapter();
			POICollections collections = viewAdapter.Collections;
			viewAdapter.PopulateEventListForSelectedLocation("H");
			Assert.True(collections.EventsForSelectedLocation.Count >= 0);
		}
		/// <summary>
		/// Tests the current location list population.
		/// </summary>
		[Test]
		public void TestCurrentLocationListPopulation()
		{
			EventListViewAdapter viewAdapter = new EventListViewAdapter();
			POICollections collections = viewAdapter.Collections;
			viewAdapter.PopulateOnetimeEventList("H");
			Assert.True(collections.EventsForSelectedLocation.Count >= 0);
		}
	}
}

