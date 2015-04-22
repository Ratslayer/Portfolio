using System;
using NUnit.Framework;
using Mongoose.Droid;
using System.Collections;

namespace Mongoose.Droid
{
	[TestFixture ()]
	public class NotificationTest 
	{
		/// <summary>
		/// Tests the time change.
		/// </summary>
		[Test]
		public void TestTimeChange()
		{
			NotificationTracker nt = new NotificationTracker();
			nt.SetNotificationTimeRange (0);
			Assert.AreEqual((int)nt.current_time,(int)nt.notification_time);
		}
		/// <summary>
		/// Tests the time change is lower.
		/// </summary>
		[Test]
		public void TestTimeChangeLower()
		{
			NotificationTracker nt = new NotificationTracker();
			nt.SetNotificationTimeRange (-1);
			Assert.AreEqual((int)nt.current_time,(int)nt.notification_time);
		}
		/// <summary>
		/// Tests the time change is greater.
		/// </summary>
		[Test]
		public void TestTimeChangeGreater()
		{
			NotificationTracker nt = new NotificationTracker();
			nt.SetNotificationTimeRange (3);
			Assert.AreNotEqual ((int)nt.current_time,(int)nt.notification_time);
		}
		/// <summary>
		/// Tests the notification filters.
		/// </summary>
		[Test]
		public void TestNotificationFilters()
		{
			//This is just testing in the console 
			NotificationTracker nt = new NotificationTracker ();
			nt.SetNotificationTimeRange (Settings.TimeRangeFilter);
			Assert.IsTrue (Settings.TimeRangeFilter!=0);
			//end test
		}
			
	}
}
