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
	[IntentFilter (new[] {Intent.ActionBootCompleted})]
	public class BootCompletedReceiver : BroadcastReceiver
	{
		/// <param name="context">The Context in which the receiver is running.</param>
		/// <param name="intent">The Intent being received.</param>
		/// <summary>
		/// This method is called when the BroadcastReceiver is receiving an Intent
		///  broadcast.
		/// </summary>
		public override void OnReceive (Context context, Intent intent)
		{
			if (intent.Action == Intent.ActionBootCompleted) 
			{
				Toast.MakeText 
				(
					context, 
					"Broadcast receiver received Boot on Startup", 
					ToastLength.Long
				).Show ();
				Intent applicationIntent = new Intent (context, typeof(BackgroundService));
				context.StartService(applicationIntent);
			}
		}


	}
}

