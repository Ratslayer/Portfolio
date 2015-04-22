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
using Android.Gms.Common.Apis;
using Android.Gms.Location;
namespace Mongoose.Droid
{
	/// <summary>
	/// Location monitor droid and its functionability implemented
	/// </summary>
	public class LocationMonitorDroid : GoogleMonitor, ILocationMonitor
	{
		readonly List<ILocationChangeListener> _locationListeners;
		readonly List<IBuildingChangeListener> _buildingListeners;
		private ILocation _lastLocation;
		public ILocation LastLocation
		{
			get
			{
				return _lastLocation;
			}
		}
		public LocationMonitorDroid (Android.Content.Context context)
			:base (context)
		{
			_locationListeners = new List<ILocationChangeListener> ();
			_buildingListeners = new List<IBuildingChangeListener> ();
			_lastLocation = new FakeLocation();
		}
		public override void OnLocationChanged(Location location)
		{
			LocationDroid newLocation = new LocationDroid (location, this);
			lastLocationDiff = newLocation.Coords - _lastLocation.Coords;
			foreach(ILocationChangeListener listener in _locationListeners)
			{
				listener.OnLocationChange(newLocation);
			}

			if(newLocation.Building != _lastLocation.Building)
			{
				foreach(IBuildingChangeListener listener in _buildingListeners)
				{
					listener.OnBuildingChange(newLocation);
				}
			}

			_lastLocation = newLocation;
		}
		public void AddListener(ILocationChangeListener listener)
		{
			_locationListeners.Add (listener);
		}
		public void AddListener(IBuildingChangeListener listener)
		{
			_buildingListeners.Add(listener);
		}
		public void RemoveListener(ILocationChangeListener listener)
		{
			_locationListeners.Remove (listener);
		}
		public void RemoveListener(IBuildingChangeListener listener)
		{
			_buildingListeners.Remove(listener);
		}
	}
}