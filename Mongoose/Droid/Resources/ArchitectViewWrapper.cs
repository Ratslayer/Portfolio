using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Wikitude.Architect;
using System.Json;
using System.Collections.Generic;
using Android.Locations;
using System.Linq;
using Android.Content;

namespace Mongoose.Droid
{
	public class ArchitectViewWrapper : ArchitectView, ILocationChangeListener
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Mongoose.Droid.ArchitectViewWrapper"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public ArchitectViewWrapper (Context context) : base(context)
		{
			Globals.RegisterLocationListener(this);
		}
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		public override void OnDestroy()
		{
			base.OnDestroy();
			Globals.UnregisterLocationListener(this);
		}
		#region ILocationChangeListener implementation
		/// <summary>
		/// Raises the location change event.
		/// </summary>
		/// <param name="newLocation">New location.</param>
		public void OnLocationChange (ILocation newLocation)
		{
			SetLocation (newLocation.Coords.latitude, newLocation.Coords.longitude, 100, 1000);
		}

		#endregion
	}
}

