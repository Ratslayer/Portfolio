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
using EstimoteSdk;
using Android.Support.V4.App;

namespace Mongoose.Droid
{
	
	[Activity (Label = "BeaconActivity")]			

	public class BeaconActivity : Activity, ViewTreeObserver.IOnGlobalLayoutListener, BeaconManager.IServiceReadyCallback,BeaconManager.IRangingListener
	{
		static readonly string Tag = typeof(BeaconActivity).FullName;
		static readonly double RELATIVE_START_POS = 320.0 / 1110.0;
		static readonly double RELATIVE_STOP_POS = 885.0 / 1110.0;
		Beacon _beacon;
		View _dotView;
		Region _region;
		int _segmentLength = -1;
		View _sonar;
		int _startY = -1;
		BeaconManager _beaconManager;

		/// <summary>
		/// Raises the global layout event.
		/// </summary>
		public void OnGlobalLayout()
		{
			_sonar.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
			_startY = (int)(RELATIVE_START_POS * _sonar.MeasuredHeight);
			int stopY = (int)(RELATIVE_STOP_POS * _sonar.MeasuredHeight);
			_segmentLength = stopY - _startY;
			_dotView.Visibility = ViewStates.Visible;
			_dotView.TranslationY = ComputeDotPosY(_beacon);
		}
	
		/// <summary>
		/// Raises the start event.
		/// </summary>
		protected override void OnStart()
		{
			base.OnStart ();
		}
		/// <summary>
		/// Called when you are no longer visible to the user.
		/// </summary>
		protected override void OnStop()
		{
			_beaconManager.Disconnect();
			base.OnStop();
		}

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			Intent intent = Intent;
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			SetContentView(Resource.Layout.distanceBackground);
			_dotView = FindViewById(Resource.Id.dot);
			_sonar = FindViewById(Resource.Id.sonar);
			_beacon = (Beacon)intent.GetParcelableExtra("Beacon_ID");
			_sonar.ViewTreeObserver.AddOnGlobalLayoutListener(this);
			_beaconManager = new BeaconManager(this);
			_beaconManager.Connect(this);
			//found beacon in the other activity from the extra
			_beaconManager.SetRangingListener(this);
		}

		/// <summary>
		/// Beacons the manager ranging.
		/// </summary>
		/// <param name="sender">object sender.</param>
		/// <param name="e">E is the ranging event arguments</param>
		void BeaconManagerRanging(object sender, BeaconManager.RangingEventArgs e)
		{
			RunOnUiThread(() =>{
				if (_segmentLength == -1)
				{
					return;
				}
				_dotView.Animate().TranslationY(ComputeDotPosY(_beacon)).Start();
			});
		}

		/// <summary>
		/// Computes the dot position y.
		/// </summary>
		/// <returns>The dot position y.</returns>
		/// <param name="foundBeacon">Found beacon.</param>
		float ComputeDotPosY(Beacon foundBeacon)
		{
			// Put the dot at the end of the scale when it's further than 6m.
			double x = Utils.ComputeAccuracy(foundBeacon);
			double distance = Math.Min(x, 6.0);
			return _startY + (int)(_segmentLength * (distance / 6.0));
		}
		/// <summary>
		/// Raises the service ready event.
		/// </summary>
		public void OnServiceReady ()
		{
			CreateRegion (_beacon);
			_beaconManager.StartMonitoring (_region);
			_beaconManager.StartRanging (_region);
		}
		/// <summary>
		/// Creates the region.
		/// </summary>
		/// <param name="beacon">Beacon.</param>
		public void CreateRegion(Beacon beacon)
		{
			_region = new Region ("rid", beacon.ProximityUUID,(Java.Lang.Integer)beacon.Major,(Java.Lang.Integer)beacon.Minor);
		}
		/// <summary>
		/// Raises the beacons discovered event.
		/// </summary>
		/// <param name="region">Region of a beacon</param>
		/// <param name="beacons">list of beacons</param>
		public void OnBeaconsDiscovered (Region region, IList<Beacon> beacons)
		{
			RunOnUiThread(() =>{
				Beacon foundBeacon = null;
				foreach (var b in beacons) {
					if (b.MacAddress.Equals (_beacon.MacAddress)) {
						foundBeacon = b;
					}
				}
				if(foundBeacon != null)
				{
					if (_segmentLength == -1)
					{
						return;
					}
					_dotView.Animate().TranslationY(ComputeDotPosY(foundBeacon)).Start();
				}
			});
		}
	}
}

