using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;

using Xamarin.Forms;

namespace Mongoose.Droid
{
	/// <summary>
	/// Location for the android part 
	/// </summary>
	public class LocationDroid : ILocation
	{
		public Coordinates Coords
		{
			get
			{
				Coordinates result;
				if(_location != null)
				{
					result = new Coordinates(_location.Latitude, _location.Longitude, _location.Altitude);
				} 
				else
				{
					result = new Coordinates(0, 0, 0);
				}
				return result;
			}
		}

		private LocationMonitorDroid _monitor;
		private Location _location;
		public LocationDroid(Location location, LocationMonitorDroid monitor)
		{
			_location = location;
			_monitor = monitor;
		}
		public string Address 
		{
			get 
			{
				string result = _monitor.LastAddress;
				if(result == null)
				{
					result = MainActivity.GetString(Resource.String.NoAddress);
				}
				return result;
			}
		}
		public string Building
		{
			get
			{
				string result = Globals.buildingAddressCollection.GetBuilding(this);
				return result;
			}
		}
		new public string ToString ()
		{
			string result = string.Format ("B: {0}; {1} | {2}", Building, Coords, _monitor.lastLocationDiff);
			return result;
		}
	}
}