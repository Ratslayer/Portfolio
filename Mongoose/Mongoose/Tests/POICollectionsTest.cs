using System;
using NUnit.Framework;
using System.Collections.Generic;
namespace Mongoose
{
	[TestFixture]
	public class POICollectionsTest
	{
		/// <summary>
		/// POIs the collections instance test.
		/// </summary>
		[Test]
		public void InstanceTest()
		{
			POICollections pois = POICollections.Instance();
			Assert.NotNull(pois);
		}
		/// <summary>
		/// POIs the collections events for selected location.
		/// </summary>
		[Test]
		public void EventsForSelectedLocation()
		{
			POICollections pois = POICollections.Instance();
			pois.EventsForCurrentLocation.Clear();
			List<Event> events = new List<Event>();
			events.Add(new Event());
			pois.EventsForCurrentLocation = events;
			Assert.AreEqual(1, events.Count);
		}
		/// <summary>
		/// POIs the collections clear favorites.
		/// </summary>
		[Test]
		public void ClearFavorites()
		{
			POICollections pois = POICollections.Instance();
			Favorite favorite = new Favorite();
			pois.eventFavorites.Add(favorite);
			pois.ClearOldFavorites(favorite.DateAdded.AddDays(1));
			Assert.AreEqual(0, pois.eventFavorites.Count);
		}
	}
}

