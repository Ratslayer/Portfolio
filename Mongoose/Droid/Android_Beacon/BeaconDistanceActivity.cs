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
using Android.Bluetooth;

namespace Mongoose.Droid
{
	[Activity (Label = "BeaconDistanceActivity")]			
	public class BeaconDistanceActivity : Activity, BeaconManager.IServiceReadyCallback, BeaconManager.IRangingListener
	{
		public static readonly Region ALL_ESTIMOTE_BEACONS_REGION = new Region("rid", null, null, null);
		const string ESTIMOTE_PROXIMITY_UUID = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
		BeaconManager _beaconManager;
		BluetoothAdapter mBluetoothAdapter;
		ListView _listview;
		BeaconsFinder _beaconFinder;
		List<Beacon> ALL_BEACONS; 
		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			CreateManager ();
			SetContentView (Resource.Layout.main);
			_listview = (ListView)FindViewById (Resource.Id.id_list_view);
		}
		/// <summary>
		/// Raises the beacons discovered event.
		/// </summary>
		/// <param name="region">Region.</param>
		/// <param name="beacons">List of Beacons.</param>
		public void OnBeaconsDiscovered (Region region, IList<Beacon> beacons)
		{
			foreach (var b in beacons)
				ALL_BEACONS.Add (b);
			RunOnUiThread (() => {
				ArrayAdapter<Beacon> adapter = new ArrayAdapter<Beacon> (this, Android.Resource.Layout.SimpleListItem1, ALL_BEACONS);
				_listview.Adapter = adapter;

			});
		}
		/// <summary>
		/// Finds the beacon list.
		/// </summary>
		public void FindBeaconList()
		{
			String[] items = new String[] {"Item 1", "Item 2", "Item 3"};
			if (ALL_BEACONS != null) {
				ArrayAdapter<Beacon> adapter = new ArrayAdapter<Beacon> (this, Android.Resource.Layout.SimpleListItem1, ALL_BEACONS);
				_listview.Adapter = adapter;
			} else {
				ArrayAdapter<String> adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, items);
				_listview.Adapter = adapter;
			}
		}
		/// <summary>
		/// Creates the manager.
		/// </summary>
		public void CreateManager()
		{
			mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
			_beaconManager = new BeaconManager (this);
			if (_beaconManager.HasBluetooth) {
				mBluetoothAdapter.Enable ();
				_beaconManager.Connect (this);
				_beaconManager.SetRangingListener (this);
			} else {
				Toast.MakeText(this, "The bluetooth has not been enabled, please enable bluetooth", ToastLength.Long).Show();
			}
		}
		/// <summary>
		/// Raises the service ready event.
		/// </summary>
		public void OnServiceReady ()
		{
			_beaconManager.StartRanging (ALL_ESTIMOTE_BEACONS_REGION);
		}
	}
}

