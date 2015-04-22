
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

namespace Mongoose.Droid
{
	[Activity (Label = "FindAllBeacons")]			
	public class EX_FindAllBeacons : Activity, BeaconManager.IServiceReadyCallback, BeaconManager.IRangingListener
	{
		public static readonly Region ALL_ESTIMOTE_BEACONS_REGION = new Region("rid", null, null, null);
		BeaconManager _beaconManager;
		List<Beacon> beacon_list;
		ListView _listview;
		int count;
		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			count = 0;
			_beaconManager = new BeaconManager (this);
			SetContentView (Resource.Layout.main);
			beacon_list = new List<Beacon> ();
			_listview = (ListView)FindViewById (Resource.Id.id_list_view);
		}
		/// <summary>
		/// Listviews the item click.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void Listview_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			Intent intent = new Intent (BaseContext, typeof(BeaconActivity));
			//this will send a beacon instead of a count
			if (beacon_list != null && beacon_list.Count != 0)
			{
				intent.PutExtra ("Beacon_ID", beacon_list [e.Position]);
			}
			StartActivity (intent);
		}
		/// <summary>
		/// Connects to service.
		/// </summary>
		public void ConnectToService()
		{
			if (_beaconManager.HasBluetooth) 
			{
				_beaconManager.Connect (this);
				_beaconManager.SetRangingListener (this);
			}
		}
		/// <summary>
		/// Raises the service ready event.
		/// </summary>
		public void OnServiceReady ()
		{
			_beaconManager.StartRanging (ALL_ESTIMOTE_BEACONS_REGION);
		}
		/// <summary>
		/// Raises the beacons discovered event.
		/// </summary>
		/// <param name="region">Region.</param>
		/// <param name="beacons">Beacons.</param>
		public void OnBeaconsDiscovered (Region region, IList<Beacon> beacons)
		{
			
			foreach (var b in beacons)
			{
				bool exists = false;
				foreach (var a in beacon_list) 
				{
					if (a.Major == b.Major && a.Minor == b.Minor) 
					{
						exists = true;
					}
				}
				if (!exists) 
				{
					beacon_list.Add (b);
				}
			}
			if (beacon_list.Count != count)
			{
				count++;
				RunOnUiThread (() => {
					ArrayAdapter<Beacon> adapter = new ArrayAdapter<Beacon> (this, Android.Resource.Layout.SimpleListItem1, beacon_list);
					_listview.Adapter = adapter;
					_listview.ItemClick += Listview_ItemClick;
				});
			}
		}
		/// <summary>
		/// Called when you are no longer visible to the user.
		/// </summary>
		protected override void OnStop()
		{
			base.OnStop();
		}
		/// <summary>
		/// Perform any final cleanup before an activity is destroyed.
		/// </summary>
		protected override void OnDestroy()
		{
			_beaconManager.Disconnect();
			base.OnDestroy ();
		}
		/// <summary>
		/// Raises the start event.
		/// </summary>
		protected override void OnStart()
		{
			base.OnStart();
			ConnectToService ();
		}


	}
}

