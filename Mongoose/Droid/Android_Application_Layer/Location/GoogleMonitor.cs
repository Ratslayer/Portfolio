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
	public abstract class GoogleMonitor : Java.Lang.Object, Android.Gms.Location.ILocationListener, IGoogleApiClientConnectionCallbacks, IGoogleApiClientOnConnectionFailedListener
	{
		//google services
		public void OnConnectionFailed(Android.Gms.Common.ConnectionResult result)
		{
			Console.Out.Write("Failed to connect to Google Services!");
		}
		public void OnConnectionSuspended(int cause)
		{
			Console.Out.Write("Connection to Google Services suspended!");
		}
		public Coordinates lastLocationDiff;
		public void OnConnected(Bundle connectionHint)
		{
			lastLocationDiff = new Coordinates(0,0,0);
			UpdateLocation();
		}

		private Android.Content.Context _context;
		private IGoogleApiClient _client;

		public GoogleMonitor (Android.Content.Context context)
		{
			_context = context;
			GoogleApiClientBuilder builder = new GoogleApiClientBuilder(context);
			_client = builder.AddApi(LocationServices.Api)
				.AddConnectionCallbacks(this)
				.AddOnConnectionFailedListener(this)
				.Build();
			_client.Connect();
		}
		public string LastAddress;
		/*public async void GetAddress(ILocation location)
		{
			string result = MainActivity.GetString(Resource.String.Error);
			if (location == null)
			{
				result = MainActivity.GetString(Resource.String.ErrorLocationNull);
			}

			Geocoder geocoder = new Geocoder(_context);
			try
			{
				IList<Address> addressList = await geocoder.GetFromLocationAsync(location.Coords.latitude, location.Coords.longitude, 10);

				Address address = addressList.FirstOrDefault();
				if (address != null)
				{
					StringBuilder deviceAddress = new StringBuilder();
					for (int i = 0; i < address.MaxAddressLineIndex; i++)
					{
						deviceAddress.Append(address.GetAddressLine(i))
							.AppendLine(",");
					}
					result = deviceAddress.ToString();
				}
				else
				{
					result = MainActivity.GetString(Resource.String.ErrorNoAdressReceived);
				}
			}
			catch(Java.IO.IOException e)
			{
				result = MainActivity.GetString(Resource.String.NoAddress) + e.Message;
			}
			LastAddress = result;
		}*/

		public void UpdateLocation (int numUpdates=1, double interval=10.0, AccuracyBatterySettings settings = AccuracyBatterySettings.HighAccuracy_HighBattery)		{
			LocationRequest request = new LocationRequest();
			request.SetNumUpdates(numUpdates);
			long longInterval = (long)(interval * 1000.0);
			request.SetInterval(longInterval);
			request.SetFastestInterval(longInterval);
			request.SetPriority(GetPriority(settings));
			LocationServices.FusedLocationApi.RequestLocationUpdates(_client, request, this);
		}
		private int GetPriority(AccuracyBatterySettings settings)
		{
			int result;
			if(settings == AccuracyBatterySettings.LowAccuracy_LowBattery)
			{
				result = LocationRequest.PriorityLowPower;
			} 
			else if(settings == AccuracyBatterySettings.AverageAccuracy_AverageBattery)
			{
				result = LocationRequest.PriorityBalancedPowerAccuracy;
			} 
			else
			{
				result = LocationRequest.PriorityHighAccuracy;
			}
			return result;
		}
		public abstract void OnLocationChanged(Location location);
	}
}