using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Wikitude.Architect;
using System.Json;
using System.Collections.Generic;
using Android.Locations;
using System.Linq;

[assembly: ExportRenderer (typeof (Mongoose.ARView), typeof (Mongoose.Droid.ARViewRenderer))]
namespace Mongoose.Droid
{
	public class ARViewRenderer : ViewRenderer<ARView, ArchitectViewWrapper>, IContentPageListener, ArchitectView.IArchitectUrlListener
	{
		/// <summary>
		/// URLs the was invoked.
		/// </summary>
		/// <returns><c>true</c>, if was invoked was URLed, <c>false</c> otherwise.</returns>
		/// <param name="uriString">URI string.</param>
		public bool UrlWasInvoked(string uriString)
		{
			var invokedUri = Android.Net.Uri.Parse (uriString);
			if("markerselected".Equals(invokedUri.Host, StringComparison.InvariantCultureIgnoreCase))
			{
				string id = invokedUri.GetQueryParameter("id");
				string title = invokedUri.GetQueryParameter("title");
				if(id == "0")
				{
					LoadArt(title);
				}
				else
				{
					LoadBuilding(title);
				}
			}
			return false;
		}
		/// <summary>
		/// Loads the art.
		/// </summary>
		/// <param name="title">Title.</param>
		private void LoadArt(string title)
		{
			Art art = Globals.arts[title];
			if(art != null)
			{
				Globals.AddPage(new ArtPage(art));
			}
		}
		/// <summary>
		/// Loads the building. This is empty because it might need implementation later on
		/// </summary>
		/// <param name="title">Title.</param>
		private void LoadBuilding(string title)
		{
            _navigation.PushAsync(new EventList(title));
		}
		//This is the trial key, can be changed to license key
		private static string wikitudeKey = "hNDtMLLQeG4BoLuRtno9VCl1MNolyrnC/0Ch631JH/pYt5eRG0xnBFamS6wUbRpn47W5gXr4EAsgO9W54aG" +
			"epxkhNULgoKldHRoPdMV2k7Oy2LyIkDVlrTNsysU/d35+wWxuOtIxK8UW7jtuMo4wkTyFsoBcRjQaz2w7riEc6n1TYWx0ZWRfX0ce7Szq5iqJipLb4O" +
			"sghZ2QrKDO2RGCzpziwOL4YV+T5Lio5RgPZYdY/mLvN43bNfWLFycexSMCeIR/8FzSKMXQgnOCXK0pGlETwAtFVG0SNVc10/O7qGzK9FD6olt3zASZe" +
			"8nzUme4+IrMi5lAZGA/IrLFl3h7a1wXz2D8pil0BvpdKLAfbj+nb5CIeWipwKPm0Sh48VTT+nPtLw9W+nPanpojDcn1fOBfSb1GDaDKbKlyvlQSfWfo" +
			"0ChCStJ4FR9WjJYRvwJTlj6SbNL9IhuTszMowLcMKgnXr9BxaJmnKZl6sILlNc6cdmorjZC28mhRXMCMD+kNtenOXndxMAXilH5Xh5/BPygpu2dAm/f" +
			"nZtGHh8t+dAR8m8uF11kd9oYDQXGYS/fjMFPyCvzlfYkexnNM2A7DtUKcVMBiz36wU1JHoV6+9b910Sq15z9DCKAKvcPN5NKi9/GqpyLoxF0cps5Q8b" +
			"xtoFOa6yFXDF7K+x8SFmRPm+A=";
		private ArchitectViewWrapper _view;
		private INavigation _navigation; 
		public ARViewRenderer() 
		{
		}
		protected override void OnElementChanged(ElementChangedEventArgs<ARView> e)
		{
			base.OnElementChanged(e);
			if(e.OldElement != null)
			{
				e.OldElement.UnregisterListener();
			}
			e.NewElement.RegisterListener(this);
			_navigation = e.NewElement.Navigation;
		}	
		/// <summary>
		/// Gets the poi information.
		/// </summary>
		/// <returns>The poi information.</returns>
		/// <param name="userLocation">User location.</param>
		private JsonArray GetPoiInformation(ILocation userLocation)
		{
			if(userLocation == null)
			{
				return null;
			}

			var pois = new List<JsonObject> ();
			foreach(Art art in Globals.arts.Arts)
			{
				pois.Add(GetJsonObject(art, 0));
			}
			var buildings = Globals.buildingAddressCollection.GetBuildingCenters();
			foreach(POI building in buildings)
			{
				pois.Add(GetJsonObject(building, 1));
			}
			var vals = from p in pois select (JsonValue)p;

			JsonArray result = new JsonArray (vals);
			return result;
		}
		/// <summary>
		/// Gets the json object.
		/// </summary>
		/// <returns>The json object.</returns>
		/// <param name="poi">Point of interest.</param>
		/// <param name="id">Identifier.</param>
		private JsonObject GetJsonObject(POI poi, int id)
		{
			Dictionary<string, JsonValue> jsonPoi = new Dictionary<string, JsonValue> () 
			{
				{ "id", id },
				{ "name", poi.title },
				{ "description", poi.description },
				{ "latitude", poi.coords.latitude },
				{ "longitude", poi.coords.longitude },
				{ "altitude", poi.coords.altitude }
			};
			return new JsonObject(jsonPoi.ToList());
		}
		#region IContentPageListener implementation

		public void OnAppear()
		{
			_view = new ArchitectViewWrapper(Forms.Context);
			var config = new ArchitectView.ArchitectConfig(wikitudeKey);
			_view.OnCreate(config);
			SetNativeControl(_view);
			_view.RegisterUrlListener(this);
			_view.OnPostCreate();
			_view.OnResume();

			Globals.UpdateLocation(1);
			string world="";
			if (Settings.Language.Equals ("en-US")) 
			{
				world = "poi_js/index.html";
			} else 
			{
				world = "poi_js/index_fr.html";
			}
			_view.Load(world);
			JsonArray poiData = GetPoiInformation(new FakeLocation());
			string param = poiData.ToString();
			string lang = Settings.Language;
			var js = "World.loadPoisFromJsonData(" + param + ",'" + lang  +"');";
			_view.CallJavascript(js);
		}

		public void OnDisappear()
		{
			_view.OnPause();
			_view.OnDestroy();
		}

		#endregion
	}
}