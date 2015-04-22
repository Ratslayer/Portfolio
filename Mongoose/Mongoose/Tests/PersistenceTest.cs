using System;
using NUnit.Framework;
using System.Collections.Generic;


namespace Mongoose
{
	[TestFixture]
	public class PersistenceTest
	{

		POICollections collections;
		Event a;
		Event b;
		Event c;
		EventListViewAdapter elva;
		List<Event> testEvent;

		/// <summary>
		/// Tests the object creation.
		/// </summary>
		[SetUp]
		public void TestObjectCreation()
		{
			collections = POICollections.Instance();
			elva = new EventListViewAdapter();
			testEvent = new List<Event>();

			//setting attributes for events
			a = new Event();
			a.title = "a";
			a.description = "a";
			a.startdate = "a";
			a.enddate = "a";

			b = new Event();
			b.title = "b";
			b.description = "b";
			b.startdate = "b";
			b.enddate = "b";

			c = new Event ();
			c.title = "c";
			c.description = "c";
			c.startdate = "c";
			c.enddate = "c";
		}


		/// <summary>
		/// Tests the persistence empty of the notified collection.
		/// </summary>
		[Test]
		public void TestPersistenceEmpty()
		{
			collections.notifiedEvents.Clear ();
			collections.toNotify.Clear ();
			testEvent.Clear ();
			testEvent.Add (a);
			testEvent.Add (b);
			elva.PopulateNotifyEvents (testEvent);
			Assert.AreEqual (collections.notifiedEvents.Count, collections.toNotify.Count);

		}

		/// <summary>
		/// Tests the persistence same of the notified collection if they have the same events
		/// </summary>
		[Test]
		public void TestPersistenceSame()
		{
			collections.notifiedEvents.Clear ();
			collections.toNotify.Clear ();
			testEvent.Clear ();
			testEvent.Add (a);
			testEvent.Add (b);
			collections.notifiedEvents.Add(a);
			collections.notifiedEvents.Add (b);
			elva.PopulateNotifyEvents (testEvent);
			Assert.AreEqual (0, collections.toNotify.Count);
		}

		/// <summary>
		/// Tests the persistence not empty of the notified collection 
		/// </summary>
		[Test]
		public void TestPersistenceNotEmpty()
		{
			collections.notifiedEvents.Clear ();
			collections.toNotify.Clear ();
			testEvent.Clear ();
			testEvent.Add (a);
			testEvent.Add (b);
			testEvent.Add (c);
			collections.notifiedEvents.Add(a);
			collections.notifiedEvents.Add (b);
			elva.PopulateNotifyEvents (testEvent);
			Assert.AreEqual (1, collections.toNotify.Count);
		}

		/// <summary>
		/// Tests the persistence object data add of the notified collections
		/// </summary>
		[Test]
		public void TestPersistenceObjectDataAdd()
		{
			collections.notifiedEvents.Clear ();
			collections.toNotify.Clear ();
			testEvent.Clear ();
			testEvent.Add (a);
			testEvent.Add (b);
			testEvent.Add (c);
			collections.notifiedEvents.Add(a);
			collections.notifiedEvents.Add (b);
			elva.PopulateNotifyEvents (testEvent);
			Assert.AreEqual (3, collections.notifiedEvents.Count);
		}

		/// <summary>
		/// Tests the persistence object data same of the notified collections
		/// </summary>
		[Test]
		public void TestPersistenceObjectDataSame()
		{
			collections.notifiedEvents.Clear ();
			collections.toNotify.Clear ();
			testEvent.Clear ();
			testEvent.Add (a);
			testEvent.Add (b);
			collections.notifiedEvents.Add(a);
			collections.notifiedEvents.Add (b);
			elva.PopulateNotifyEvents (testEvent);
			Assert.AreEqual (2, collections.notifiedEvents.Count);
		}

	

	}
}