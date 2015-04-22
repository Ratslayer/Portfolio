using System;

using Xamarin.Forms;

namespace Mongoose
{
	public class AboutPage : BaseContentPage
	{
		public AboutPage () : base()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage ();
			Title = strings.About;
		
			StackLayout ApplicationButtonStack = CreateSettingsSubCell(strings.Application, "Application version: 1.0\nRelease date: April 2, 2015");
			var applicationTap = new TapGestureRecognizer();
			ApplicationButtonStack.GestureRecognizers.Add(applicationTap);	

			StackLayout DebugButtonStack = CreateSettingsTextCell(strings.DebugBeacon);
			var debugTap = new TapGestureRecognizer();
			debugTap.Tapped += async (sender, e) => 
			{
				DebugButtonStack.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
					await Globals.AddPage(new DebugPage());
					this.ReleaseTapLock ();
				}
				DebugButtonStack.BackgroundColor = Theme.backgroundLightColor;
			};
			DebugButtonStack.GestureRecognizers.Add(debugTap);

			StackLayout DebugLocationButtonStack = CreateSettingsTextCell(strings.DebugLocation);
			var debugLocationTap = new TapGestureRecognizer();
			debugLocationTap.Tapped += async (sender, e) => 
			{
				DebugLocationButtonStack.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
					await Globals.AddPage(new LocationPage());
					this.ReleaseTapLock ();
				}
				DebugLocationButtonStack.BackgroundColor = Theme.backgroundLightColor;
			};
			DebugLocationButtonStack.GestureRecognizers.Add(debugLocationTap);
			var stackLayout = new StackLayout
			{
				Children =
				{
					ApplicationButtonStack,
					DebugButtonStack,
					DebugLocationButtonStack,
				},
				BackgroundColor = Theme.backgroundLightColor
			};

			Content = stackLayout;

		}
	}
}


