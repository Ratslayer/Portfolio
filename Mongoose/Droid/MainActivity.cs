using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.Res;
using EstimoteSdk;
using Java.Util.Concurrent;
using JavaInteger = Java.Lang.Integer;
using Android.Bluetooth;
using Android.Gms.Common.Apis;
using System.Collections;

namespace Mongoose.Droid
{
	[Activity (Label = "ArtEvent Guide", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]	
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected static MainActivity instance;
		const int REQUEST_ENABLE_BT = 0;  
		NotificationCreator ne;

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			instance = this;
			Mongoose.ScreenWidth = (int)Resources.DisplayMetrics.WidthPixels; // real pixels
			Mongoose.ScreenHeight = (int)Resources.DisplayMetrics.HeightPixels; // real pixels	
			global::Xamarin.Forms.Forms.Init (this, bundle);
			//Location specific code
			Globals.Initialize(new LocationMonitorDroid (this));
			//end location
			LoadApplication (new Mongoose ());
			ActionBar.SetIcon(Resource.Drawable.golden_book_64);
			Android.Util.Log.Warn("Mongoose", "Mongoose Loaded!");

			new NotificationTracker (instance);
			ne = new NotificationCreator (instance);
			Intent applicationIntent = new Intent (this, typeof(BackgroundService));
			if (!IsServiceRunning(new BackgroundService().Class.Name))
			{
				this.StartService (applicationIntent);
			}
		}

		/// <summary>
		/// Determines whether this instance is service running the specified thisService.
		/// </summary>
		/// <returns><c>true</c> if this instance is service running the specified thisService; otherwise, <c>false</c>.</returns>
		/// <param name="thisService">This service.</param>
		public bool IsServiceRunning(String thisService)
		{
			ActivityManager service = (ActivityManager)GetSystemService (Context.ActivityService);
			foreach (var s in service.GetRunningServices(JavaInteger.MaxValue)) 
			{
				if (s.Service.ClassName.Equals(thisService))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Runs the alert dialog.
		/// </summary>
		public void RunAlertDialog()
		{
			string[] items = {"Do not show again"};
			//This is the prompt
			RunOnUiThread (() => 
			{
				AlertDialog.Builder builder;
				builder = new AlertDialog.Builder (this, 3);
				builder.SetTitle (GetString(Resource.String.AlertDialogTitle));
				builder.SetMessage (GetString(Resource.String.AlertDialog));
				builder.SetCancelable (false);
				builder.SetMultiChoiceItems(items,null,delegate {
					
				});
				builder.SetPositiveButton ("OK", (senderAlert, args) => {
				} );
				builder.Show ();
			});
		}
		/// <summary>
		/// Gets the resources.
		/// </summary>
		/// <returns>The resources.</returns>
		public static Resources GetResources() 
		{
			return instance.BaseContext.Resources;
		}
		/// <param name="resId">Resource id for the string</param>
		/// <summary>
		/// Return a localized string from the application's package's
		///  default string table.
		/// </summary>
		/// <returns>To be added.</returns>
		/// <param name="resID">Res I.</param>
		public static new string GetString(int resID)
		{
			Resources res = GetResources();
			CheckLocale (res);

			return res.GetString (resID);
		}
		/// <summary>
		/// Checks the locale.
		/// </summary>
		/// <param name="res">Res.</param>
		static void CheckLocale (Resources res)
		{
			Configuration conf = res.Configuration;
			if (!conf.Locale.Language.Equals (Settings.Language)) {
				conf.Locale = new Java.Util.Locale (Settings.Language);
				res.UpdateConfiguration (conf, null);
			}
		}

	}
}


