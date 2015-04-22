using System;
using EstimoteSdk;
using Android.App;
using System.Collections.Generic;
using Android.Content;

namespace Mongoose.Droid
{
	public class BeaconsFinder:  Java.Lang.Object,BeaconManager.IRangingListener
	{
		Context context;
		public List<Beacon> ALL_FOUND_BEACONS{ get; set;}
		BeaconManager _beaconManager;

		public BeaconsFinder (Context context)
		{
			this.context = context;
			_beaconManager = new BeaconManager(context);
			_beaconManager.SetRangingListener (this);
		}
			
		#region IRangingListener implementation

		public void OnBeaconsDiscovered (Region region, IList<Beacon> beacons)
		{ 
			foreach(var b in beacons)
			{
 				ALL_FOUND_BEACONS = new List<Beacon>();
				ALL_FOUND_BEACONS.Add (b);
			}
		}

		#endregion

	}
}

