
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

namespace Mongoose.Droid
{
	[BroadcastReceiver]
	public class FavoritesReceiver : BroadcastReceiver
	{
		/// <param name="context">The Context in which the receiver is running.</param>
		/// <param name="intent">The Intent being received.</param>
		/// <summary>
		/// This method is called when the BroadcastReceiver is receiving an Intent
		///  broadcast.
		/// </summary>
		public override void OnReceive (Context context, Intent intent)
		{
			POICollections.Instance().ClearOldFavorites(
				DateTime.Now.AddDays(
					-(Settings.FavoritesExpiration)
				)
			);
		}
	}
}

