using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Mongoose
{
	public class NotificationsSettingsPage : BaseContentPage
	{
		public NotificationsSettingsPage () : base()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage();
			Title = strings.Notifications;

            CustomSwitch notifyEventsSwitch;
            StackLayout notifyEventsStack = CreateSettingsSwitchCell(strings.NotifyEvents, strings.NotifyEventsDetails, Settings.NotifyEvents, out notifyEventsSwitch);
            notifyEventsSwitch.Toggled += (object sender, bool e) =>
            {
                Settings.NotifyEvents = e;
            };

            CustomSwitch notifyArtsSwitch;
            StackLayout notifyArtsStack = CreateSettingsSwitchCell(strings.NotifyArts, strings.NotifyArtsDetails, Settings.NotifyArts, out notifyArtsSwitch);
            notifyArtsSwitch.Toggled += (object sender, bool e) =>
                {
                    Settings.NotifyArts = e;
                };

            CustomSwitch notifyDetailsSwitch;
            StackLayout notifyDetailsStack = CreateSettingsSwitchCell(strings.NotifyDetails, strings.NotifyDetailsDetails, Settings.NotifyDetails, out notifyDetailsSwitch);
            notifyDetailsSwitch.Toggled += (object sender, bool e) =>
                {
                    Settings.NotifyDetails = e;
                };

            Content = new ScrollView()
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        notifyEventsStack,
                        notifyArtsStack,
                        notifyDetailsStack
                    }
                }
            };
		}
	}
}


