using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture()]
	public class TimeFilterRangeTest
	{
		POICollections collections = POICollections.Instance();
		string year = DateTime.Today.Year.ToString();
		string month = DateTime.Today.Month.ToString();
		Event test1;
		Event test2;
		Event test3;
		Event test4;
		List<Event> oneTimeEvents;
		List<Event> ongoingEvents;

		/// <summary>
		/// Initial setup conditions.
		/// </summary>
		[TestFixtureSetUp]
		public void Init()
		{
			oneTimeEvents = collections.EventsForCurrentLocation;
			oneTimeEvents.Clear();
			ongoingEvents = collections.OngoingEventsForCurrentLocation;
			ongoingEvents.Clear();
			test1 = new Event();
			test2 = new Event();
			test3 = new Event();
			test4 = new Event();
			test1.title = "Test 1";
			test2.title = "Test 1";
			test3.title = "Test 3";
			test4.title = "Test 4";
			ParseDates(3, 0, 0, year, month, test1);
			ParseDates(3, 1, 1, year, month, test2);
			ParseDates(6, 1, 3, year, month, test3);
			ParseDates(1, 0, 0, year, month, test4);
		}
		/// <summary>
		/// Tests the time range filter.
		/// </summary>
		[Test]
		public void TestTimeRangeFilter()
		{
			oneTimeEvents.Add(test1);
			oneTimeEvents.Add(test2);
			oneTimeEvents.Add(test3);
			oneTimeEvents.Add(test4);
			Assert.AreEqual(4, oneTimeEvents.Count);
			List<Event> result = POICollectionsHelper.GetOneTimeEvents(oneTimeEvents, DateTime.Now, 120);
			Assert.AreEqual(1, result.Count);
		}

		/// <summary>
		/// Tests the ongoing time filter.
		/// </summary>
		[Test]
		public void TestOngoingTimeFilter()
		{
			ongoingEvents = new List<Event>();
			ongoingEvents.Add(test1);
			ongoingEvents.Add(test2);
			ongoingEvents.Add(test3);
			ongoingEvents.Add(test4);
			Assert.AreEqual(4, ongoingEvents.Count);
			List<Event> result = POICollectionsHelper.FilterOngoingEvents(ongoingEvents, DateTime.Now, 120);
			Assert.AreEqual(2, result.Count);
		}
		/// <summary>
		/// Parses the dates.
		/// </summary>
		/// <param name="endHour">End hour.</param>
		/// <param name="startDay">Start day.</param>
		/// <param name="endDay">End day.</param>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		/// <param name="test1">Test1.</param>
        static void ParseDates(int endHour, int startDay, int endDay, string year, string month, Event test1)
        {
            test1.starttime = DateTime.Now.Hour.ToString() + ":" + ((DateTime.Now.Minute < 10) ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString());
            test1.endtime = DateTime.Now.AddHours(endHour).Hour.ToString() + ":" + ((DateTime.Now.Minute < 10) ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString());
            string event1StartDay = DateTime.Today.AddDays(startDay).Day.ToString();
            test1.startdate = year + "/" + month + "/" + event1StartDay;
            string event1EndDay = DateTime.Today.AddDays(endDay).Day.ToString();
            test1.enddate = year + "/" + month + "/" + event1EndDay;
        }
	}
}

