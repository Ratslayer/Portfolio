using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Media;

namespace Mongoose.Droid
{
	//[Activity (Label = "NotificationEvent")]			
	public class NotificationCreator : Activity
	{
	
		List<String> events = new List<String>();
		Activity activity = new Activity();
		Intent intent = new Intent();
		public bool notifyEventDetails { get; set; }

		public NotificationCreator(Activity act)
		{
			activity = act;
			notifyEventDetails = true;
		}

		public NotificationCreator(Intent inten)
		{
			intent = inten;
		}
			
		/// <summary>
		/// Notifications the event list in details
		/// </summary>
		/// <param name="eventlist">List of the events happening</param>
		public void NotificationEventList(List<Event> eventlist)
		{
			Notification.Builder builder = new Notification.Builder (activity)
				.SetContentTitle (MainActivity.GetString (Resource.String.NewEvents))
				.SetContentText (MainActivity.GetString (Resource.String.ViewMore))
				.SetAutoCancel (true)
				.SetVibrate (new long[]{ 1000, 1000 })
				.SetDefaults (NotificationDefaults.Sound)
				.SetSmallIcon (Resource.Drawable.book_white_64);
			// Instantiate the Inbox style:
			Notification.InboxStyle inboxStyle = new Notification.InboxStyle();

			// Set the title and text of the notification:
			builder.SetContentTitle (MainActivity.GetString(Resource.String.LocationEvents));
			builder.SetContentText (MainActivity.GetString(Resource.String.NewEvents));

			// Generate a message summary for the body of the notification:
			foreach (Event e in eventlist) 
			{
				inboxStyle.AddLine (e.title + " " + e.starttime + "-" + e.endtime + " " + e.building); 
			}
			inboxStyle.SetSummaryText (MainActivity.GetString(Resource.String.VisitForMore));

			// Plug this style into the builder:
			builder.SetStyle (inboxStyle);
			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				activity.GetSystemService (Context.NotificationService) as NotificationManager;

			// Publish the notification:
			const int notificationId = 0;
			notificationManager.Notify (notificationId, notification);
		}

		/// <summary>
		/// Notifications of the event using the notification builder for big text
		/// </summary>
		public void NotificationEventDetails()
		{
			GetPhoneOptions ();

			Notification.Builder builder = new Notification.Builder (activity)
				.SetContentTitle (MainActivity.GetString (Resource.String.NewEvents))
				.SetContentText (MainActivity.GetString (Resource.String.ViewMore))
				.SetAutoCancel (true)
				.SetVibrate (new long[]{ 1000, 1000 })
				.SetDefaults (NotificationDefaults.Sound)
				.SetSmallIcon (Resource.Drawable.book_white_64);
			// Instantiate the Big Text style:
			Notification.BigTextStyle textStyle = new Notification.BigTextStyle();

			// Fill it with text:
			string longTextMessage = MainActivity.GetString(Resource.String.LongTextMessage);
			//...
			textStyle.BigText (longTextMessage);

			// Set the summary text:
			textStyle.SetSummaryText (MainActivity.GetString(Resource.String.SummaryTextMessage));

			// Plug this style into the builder:
			builder.SetStyle (textStyle);

			// Create the notification and publish it ...
			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				activity.GetSystemService (Context.NotificationService) as NotificationManager;

			// Publish the notification:
			const int notificationId = 0;
			notificationManager.Notify (notificationId, notification);
		}
	
		/// <summary>
		/// Notification of events without details
		/// </summary>
		/// <param name="count">Count of the number of event in the list</param>
		public void NotificationEvent(int count)
		{
			Notification.Builder builder = new Notification.Builder (activity)
                .SetContentTitle (MainActivity.GetString (Resource.String.ConcordiaNotification))
				.SetContentText (MainActivity.GetString (Resource.String.ThereAre) + " " + count + " " + MainActivity.GetString (Resource.String.NewEvents))
				.SetAutoCancel (true)	
				.SetVibrate (new long[]{ 1000, 1000 })
				.SetDefaults (NotificationDefaults.Sound)
				.SetSmallIcon (Resource.Drawable.book_white_64);

			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				// not sure about mainActivity
				activity.GetSystemService (Context.NotificationService) as NotificationManager;

			// Publish the notification:
			const int notificationId = 0;
			notificationManager.Notify (notificationId, notification);
		}

		private RingerMode GetPhoneOptions()
		{
			AudioManager audio = (AudioManager)activity.GetSystemService (Context.AudioService);
			return audio.RingerMode;
		}
	}


}

