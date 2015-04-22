using Android.App;
using Android.Content;
using Android.OS;
using EstimoteSdk;
using Java.Util.Concurrent;
using JavaInteger = Java.Lang.Integer;
using Android.Widget;
using Android.Bluetooth;


namespace Mongoose.Droid
{
	[Activity(Label = "BeaconActivity")]

	public class BeaconActivity : Activity, BeaconManager.IServiceReadyCallback
	{
		const int REQUEST_ENABLE_BLUETOOTH = 123321;
		static readonly int NOTIFICATION_ID = 123321;
		BeaconManager _beaconManager;
		Region _region;
		static string ESTIMOTE_PROXIMITY_UUID = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			_beaconManager = new BeaconManager(this);
			// Default values are 5s of scanning and 25s of waiting time to save CPU cycles.
			// In order for this demo to be more responsive and immediate we lower down those values.
			_beaconManager.SetBackgroundScanPeriod(TimeUnit.Seconds.ToMillis(1), 0);
			_beaconManager.EnteredRegion += (sender, e) => SendNotification (MainActivity.GetString(Resource.String.BeaconEnter));
			_beaconManager.ExitedRegion += (sender, e) => SendNotification (MainActivity.GetString(Resource.String.BeaconExit));

		}
			
		protected override void OnResume()
		{
			base.OnResume();
			_beaconManager.Connect(this);
		}

		public void OnServiceReady()
		{
			//monitor the only working beacon atm
			_region = new Region ("region1", ESTIMOTE_PROXIMITY_UUID, new JavaInteger (32117), new JavaInteger (5758));
			if (_region != null) {
			_beaconManager.StartMonitoring (_region);
			}
		}

		protected override void OnDestroy()
		{
			// Make sure we disconnect from the Estimote.
			_beaconManager.Disconnect();
			base.OnDestroy();
		}

		public Region CreateRegion(int major, int minor)
		{
			return new Region ("Regionx", ESTIMOTE_PROXIMITY_UUID, new JavaInteger (major), new JavaInteger (minor));
		}

		public void SendNotification(string message)
		{
			Notification.Builder builder = new Notification.Builder (this)
				.SetContentTitle (MainActivity.GetString(Resource.String.BeaconTitle))
				.SetContentText (message)
				.SetAutoCancel (true)	
				.SetSmallIcon (Resource.Drawable.icon);
			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				// not sure about mainActivity
				GetSystemService (Context.NotificationService) as NotificationManager;

			// Publish the notification:
			notificationManager.Notify (NOTIFICATION_ID, notification);

		}
	}
}