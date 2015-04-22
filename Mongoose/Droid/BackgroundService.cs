using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EstimoteSdk;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Concurrent;
using JavaInteger = Java.Lang.Integer;
using Android.Support.V4.App;

namespace Mongoose.Droid
{
	[Service]
	public class BackgroundService : Service, BeaconManager.IServiceReadyCallback
	{
		public static readonly Region ALL_ESTIMOTE_BEACONS_REGION = new Region("rid", null, null, null);
		BeaconManager _beaconManager;
		Region _regionA, _regionB, _regionC;
		//Set the initial distance to around 5 meters ( this is when the beacon will be notified )
		int distance_A = 5;
		int distance_B = 5;
		int distance_C = 5;
		//The notification ID for the beacon
		const int _NOTIFICATIONID = 2;
		const string ESTIMOTE_PROXIMITY_UUID = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
		const int REQUEST_ENABLE_BT = 0;   
	
		#region implemented abstract members of Service

		public override IBinder OnBind (Intent intent)
		{
			throw new NotImplementedException ();
		}

		#endregion

		public override void OnCreate()
		{
			base.OnCreate();
			Toast.MakeText(this, "Service Created", ToastLength.Long).Show();
			_beaconManager = new BeaconManager(this);
			if (_beaconManager.HasBluetooth)
			{
				_beaconManager.Connect (this);
			}

			//Set the scan period to different number depending on the option if it's on and off
			// Default values are 5s of scanning and 25 to wait, but we can change it to something if we regard saving battery
			try
			{
				if (Settings.BluetoothPerformance) {
					_beaconManager.SetBackgroundScanPeriod (TimeUnit.Seconds.ToMillis (5), 25);
				} 
				else 
				{
					_beaconManager.SetBackgroundScanPeriod(TimeUnit.Seconds.ToMillis(1), 1);
				}
			}
			catch(NullReferenceException e)
			{
				_beaconManager.SetBackgroundScanPeriod (TimeUnit.Seconds.ToMillis (5), 25);
			}

			//This will range the beacon to verify how far you are from the beacon
			_beaconManager.Ranging += BeaconManagerRanging;
			//This will send a notification when the person is approaching a beacon and exiting a beacon
			_beaconManager.EnteredRegion += (sender, e) => SendNotification (e.Region.Major, e.Region.Minor, true);
			_beaconManager.ExitedRegion += (sender, e) => SendNotification (e.Region.Major, e.Region.Minor, false);

		}

		/// <summary>
		/// Raises the start command event.
		/// </summary>
		/// <param name="intent">Intent.</param>
		/// <param name="flags">Flags.</param>
		/// <param name="startId">Start identifier.</param>
		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			Toast.MakeText(this, "Service Started", ToastLength.Long).Show();
			//Favorites
			SetFavoritesAlarm (this);

			return StartCommandResult.Sticky;
		}

		/// <summary>
		/// Called by the system to notify a Service that it is no longer used and is being removed.
		/// </summary>
		public override void OnDestroy() 
		{
			Toast.MakeText(this, "My Service GOT DESTROYED", ToastLength.Long).Show();
			_beaconManager.Disconnect ();
			base.OnDestroy ();
		}

		/// <summary>
		/// This will monitor each beacon, and also range the beacon if it's coming close
		/// </summary>
		public void OnServiceReady ()
		{
			//Refactor the code to a list, and foreach
			_regionC = new Region ("regionC", ESTIMOTE_PROXIMITY_UUID, new JavaInteger (32117), new JavaInteger (5758));
			_regionB = new Region ("regionC", ESTIMOTE_PROXIMITY_UUID, new JavaInteger (65162), new JavaInteger (35479));
			_regionA = new Region ("regionC", ESTIMOTE_PROXIMITY_UUID, new JavaInteger (52791), new JavaInteger (33936));
			if (_regionA != null && _regionB != null && _regionC != null) 
			{
				_beaconManager.StartMonitoring (_regionA);
				_beaconManager.StartRanging (_regionA);
				_beaconManager.StartMonitoring (_regionB);
				_beaconManager.StartRanging (_regionB);
				_beaconManager.StartMonitoring (_regionC);
				_beaconManager.StartRanging (_regionC);
				_beaconManager.StartMonitoring (ALL_ESTIMOTE_BEACONS_REGION);
			}
		}
			

		/// <summary>
		/// Sets the distance for each beacon
		/// </summary>
		/// <param name="major">Major.</param>
		/// <param name="meters">Meters.</param>
		void SetDistance(int major, int meters)
		{
			switch (major) 
			{
			case 32117:
				distance_A = meters;
				break;
			case 52791:
				distance_B = meters;
				break;
			case 65162:
				distance_C = meters;
				break;
			default:
				distance_A = 5;
				distance_B = 5;
				distance_C = 5;
				break;
			}
		}

		/// <summary>
		/// This will determine how close or how far from the beacon 
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void BeaconManagerRanging(object sender, BeaconManager.RangingEventArgs e)
		{
			foreach (var beacon in e.Beacons) 
			{
				var proximity = Utils.ComputeProximity (beacon);

				if (proximity == Utils.Proximity.Immediate) 
				{
					SetDistance (beacon.Major, 3);
				}
				if (proximity == Utils.Proximity.Near) 
				{
					SetDistance (beacon.Major, 1);
				}
			}
		}

		/// <summary>
		/// Sends a notification about entering a region of the beacon
		/// </summary>
		/// <param name = "major"></param>
		/// <param name = "minor"></param>
		/// <param name="isEntered">if set to true display message for entering, else display message exiting</param>
		public void SendNotification(JavaInteger major, JavaInteger minor, bool isEntered){
			if (Settings.NotifyArts) 
			{

				NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
					.SetAutoCancel (false) 
                    .SetContentTitle ("Beacon Notification") 
					.SetSmallIcon (Resource.Drawable.flagIMG)
					.SetContentText (BeaconIdentifier(major,isEntered));

				NotificationManager notificationManager = (NotificationManager)GetSystemService (NotificationService);
				notificationManager.Notify (_NOTIFICATIONID, builder.Build ());
			}
		}
			
		/// <summary>
		/// Identifies which beacons it belongs
		/// </summary>
		/// <returns>The string specific for each beacon</returns>
		/// <param name="major">Beacon region major</param>
		/// <param name = "isEntered"></param>
		public string BeaconIdentifier(JavaInteger major, bool isEntered)
		{
			if (major == null) 
			{
				major = (JavaInteger)99;
			}
            if (isEntered) {
                switch ((int)major) {
                    case 32117:
                        return "You're ~" + distance_A + "m from the Course Reserve beacon";
                    case 52791:
                        return "You're ~"+ distance_B + "m from the New Books beacon";
                    case 65162:
                        return "You're ~"+ distance_C +"m from the Art Display beacon";
                    default:
                        return "You've found an unknown beacon!";
                }
            } 
            else 
            {
                switch ((int)major)
                {
                    case 32117:
                        return "You're exiting the Course Reserve beacon ";
                    case 52791:
                        return "You're exiting the New Books beacon";
                    case 65162:
                        return "You're exiting the Art Display beacon";
                    default:
                        return "You're exiting the Unknown beacon";
                }
            }

		}

		/// <summary>
		/// Sets the favorites alarm.
		/// </summary>
		/// <param name="context">Context.</param>
		void SetFavoritesAlarm(Context context)
		{
			Intent favoritesClearer = new Intent (context, typeof(FavoritesReceiver));
			if (PendingIntent.GetBroadcast (this, 0, favoritesClearer, PendingIntentFlags.NoCreate) == null) 
			{
				PendingIntent recurringFavoritesClearer = PendingIntent.GetBroadcast (context, 0, favoritesClearer, PendingIntentFlags.CancelCurrent);
				AlarmManager alarms = (AlarmManager)GetSystemService (Context.AlarmService);
				alarms.SetRepeating (AlarmType.Rtc, 0, AlarmManager.IntervalHalfDay, recurringFavoritesClearer);
			}
		}


	}
}

