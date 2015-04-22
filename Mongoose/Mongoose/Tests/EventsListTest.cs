using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;

namespace Mongoose
{
    [TestFixture]
    public class EventsListTests
    {
		/// <summary>
		/// Tests the object creation from json deserialization
		/// </summary>
        [Test]
        public void TestObjectCreationFromJsonDeserialization()
        {
            EventListViewAdapter viewAdapter = new EventListViewAdapter();

            string testJson = "[{\"title\":\"Test Title\",\"description\":\"Test description\"," +
                "\"startdate\":\"2015/2/10\",\"starttime\":\"12:00\",\"enddate\":\"2015/2/10\"," +
                "\"endtime\":\"13:15\",\"allday\":\"False\",\"campus\":\"SGW\",\"building\":\"H\"," +
                "\"venue\":\"\",\"room\":\"760\",\"offlocation\":\"\",\"offcivic\":\"\",\"offlink\":\"\"," +
                "\"category\":[],\"audiences\":[],\"units\":[\"units:main\"],\"url\":\"www.example.com\"}]";

            List<Event> events;
            events = new List<Event>();
            events = viewAdapter.ParseIntoObject(testJson);

            Event[] eventArray = events.ToArray();

            StringAssert.AreEqualIgnoringCase(eventArray[0].title, "Test Title");
            StringAssert.AreEqualIgnoringCase(eventArray[0].description, "Test description");
        }

		/// <summary>
		/// Tests the add listener.
		/// </summary>
        [Test]
        public void TestAddListener()
        {
            EventListViewAdapter viewAdapter = new EventListViewAdapter();
            viewAdapter.AddListener(null);
            Assert.AreNotSame(null, viewAdapter);
        }

		/// <summary>
		/// Tests the properties.
		/// </summary>
        [Test]
        public void TestProperties()
        {
            EventListViewAdapter viewAdapter = new EventListViewAdapter();
            POICollections collections = viewAdapter.Collections;

            Assert.True(collections.EventsForCurrentLocation.Count == 0);
            Assert.True(collections.EventsForSelectedLocation.Count == 0);
        }
    }
}
