using System.Threading.Tasks;
using Xamarin.Forms;
namespace Mongoose
{
	/// <summary>
	/// This class contains all the global variable that will be used within our program
	/// </summary>
	public static class Globals
	{
		private static ILocationMonitor _locationMonitor;
		private static INavigation _navigation;
		public static BuildingLocationCollection buildingAddressCollection;
		public static LocationUpdateManager locationManager;
		public static ArtCollection arts;
		public static void SetNavigation(INavigation navigation)
		{
			_navigation = navigation;
		}
		public static async Task AddPage(BaseContentPage page, bool animated=true)
		{
			page.GeneratePage();
			await _navigation.PushAsync (page, animated);
		}
		public static async Task PopPage()
		{
			await _navigation.PopAsync();
		}
		public static void RemovePage(BaseContentPage page)
		{
			_navigation.RemovePage(page);
		}
		public static void Initialize(ILocationMonitor monitor)
		{
			_locationMonitor = monitor;
			buildingAddressCollection = new BuildingLocationCollection();
			arts = new ArtCollection();
			locationManager = new LocationUpdateManager(Globals.buildingAddressCollection, _locationMonitor);
            locationManager.BatterySavingEnabled = Settings.GPSPerformance;

		}
		public static ILocation LastLocation
		{
			get
			{
				return _locationMonitor.LastLocation;
			}
		}
		public static string LastBuilding
		{
			get
			{
				string result = "";
				if(_locationMonitor != null)
				{
					result = _locationMonitor.LastLocation.Building;
				}
				return result;
			}
		}
		public static void RegisterLocationListener(ILocationChangeListener listener)
		{
			if(_locationMonitor != null)
			{
				_locationMonitor.AddListener(listener);
			}
		}
		public static void RegisterLocationListener(IBuildingChangeListener listener)
		{
			if(_locationMonitor != null)
			{
				_locationMonitor.AddListener(listener);
			}
		}
		public static void UnregisterLocationListener(ILocationChangeListener listener)
		{
			if(_locationMonitor != null)
			{
				_locationMonitor.RemoveListener(listener);
			}
		}
		public static void UnregisterLocationListener(IBuildingChangeListener listener)
		{
			if(_locationMonitor != null)
			{
				_locationMonitor.RemoveListener(listener);
			}
		}
		public static void UpdateLocation(int numUpdates = 1)
		{
			_locationMonitor.UpdateLocation(numUpdates);
		}
	}
}