using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mongoose
{
	public class Mongoose : Application
	{

		public static bool buttonClicked = false;

		/// <summary>
		/// Definition of the screen height and width so we can define it in other screen with different dimensions
		/// </summary>
		public static int ScreenWidth;
		public static int ScreenHeight;
		public Mongoose ()
		{
			strings.Culture = new CultureInfo (Settings.Language);
			var navPage = new NavigationPage();
			navPage.BarBackgroundColor = Theme.backgroundDarkColor;
			navPage.BarTextColor = Theme.textAccentLightColor;

			Globals.SetNavigation(navPage.Navigation);
			Globals.AddPage(new LandingPage());

			// The root page of your application
			MainPage = navPage;
		}



		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

