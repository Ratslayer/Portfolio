using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace Mongoose.Droid
{
	public class NotificationTracker : Activity, IBuildingChangeListener
	{
		public double current_time { get; set;}
		public double notification_time { get; set; }
		public Activity activity;
		NotificationCreator ne;
		private POICollections collections;

		public NotificationTracker()
		{
			current_time = DateTime.Now.TimeOfDay.TotalSeconds;
			notification_time = current_time + 21600;
		}

		public NotificationTracker (Activity act)
		{
			//maybe run it in the background, since when the person stop the app it resets
			activity = act;
			current_time = DateTime.Now.TimeOfDay.TotalSeconds;
			notification_time = current_time + 21600;
			Globals.RegisterLocationListener(this);
			ne = new NotificationCreator (activity);
		}

		/// <summary>
		/// Sets the notification time range.
		/// </summary>
		/// <param name="hours">Hours from the settings change</param>
		public void SetNotificationTimeRange(double hours)
		{
			System.Diagnostics.Debug.WriteLine ("This is the life before: " + hours);
			if(hours > 0)
			{
				notification_time = notification_time - 21600 + (hours * 3600);
			}
			else
			{
				notification_time = notification_time - 21600;
			}
			System.Diagnostics.Debug.WriteLine ("This is the life after: " + current_time);
			System.Diagnostics.Debug.WriteLine ("This is the life after: " + notification_time);
		}

		/// <summary>
		/// Implementation of the OnBuildingChange from the Location Listeniner, also
		/// this function will tell the rules of when to notify by checking the settings from the settings page
		/// </summary>
		/// <param name="newLocation">New location.</param>
		#region ILocationMonitorListener implementation
		public void OnBuildingChange (ILocation newLocation)
		{
			collections = POICollections.Instance ();
			current_time = DateTime.Now.TimeOfDay.TotalSeconds;
			SetNotificationTimeRange (Settings.TimeRangeFilter); 
			if (current_time > notification_time && Settings.NotifyEvents) 
			{
				if(Settings.NotifyDetails)
				{
					ne.NotificationEventList(collections.EventsForCurrentLocation);
				}
				else
				{
					ne.NotificationEvent(collections.EventsForCurrentLocation.Count);
				}
				notification_time = current_time + 21600;
			} 
			else if (collections.toNotify.Count != 0 && Settings.NotifyEvents)
			{
				if(Settings.NotifyDetails)
				{
					ne.NotificationEventList(collections.toNotify);
				}
				else
				{
					ne.NotificationEvent(collections.toNotify.Count);
				}
				collections.toNotify.Clear ();
			}
		}
		#endregion
	}
}

