using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Wikitude.Architect;
using System.Json;
using System.Collections.Generic;
using Android.Locations;
using System.Linq;

[assembly: ExportRenderer (typeof (Mongoose.LocationView), typeof (Mongoose.Droid.LocationViewRenderer))]
namespace Mongoose.Droid
{
	public class LocationViewRenderer : ViewRenderer<LocationView, LocationViewDroid>, IContentPageListener
	{
		private LocationViewDroid _view;
		private INavigation _navigation;
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<LocationView> e)
		{
			base.OnElementChanged(e);
			if(e.OldElement != null)
			{
				e.OldElement.UnregisterListener();
			}
			e.NewElement.RegisterListener(this);
			_navigation = e.NewElement.Navigation;
		}	
		#region IContentPageListener implementation
		/// <summary>
		/// Raises the appear event.
		/// </summary>
		public void OnAppear()
		{
			_view = new LocationViewDroid(Forms.Context);
			Globals.RegisterLocationListener(_view);
			SetNativeControl(_view);
		}
		/// <summary>
		/// Raises the disappear event.
		/// </summary>
		public void OnDisappear()
		{
			Globals.UnregisterLocationListener(_view);
		}

		#endregion
	}
}