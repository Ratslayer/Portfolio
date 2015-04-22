using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;

namespace Mongoose
{
    public class PerformanceSettingsPage : BaseContentPage
    {
        public PerformanceSettingsPage () : base()
        {
        }
		/// <summary>
		/// Generates the page.
		/// </summary>
        public override void GeneratePage ()
        {
            base.GeneratePage();
            Title = strings.Performance;
      
            CustomSwitch gpsPerformanceSwitch;
            StackLayout gpsPerformanceStack = CreateSettingsSwitchCell(strings.PerformanceGPS, strings.PerformanceGPSDetails, Settings.GPSPerformance, out gpsPerformanceSwitch);
            gpsPerformanceSwitch.Toggled += (object sender, bool e) =>
            {
                Settings.GPSPerformance = e;
            };

            CustomSwitch bluetoothPerformanceSwitch;
			StackLayout bluetoothPerformanceStack;
			bluetoothPerformanceStack = CreateSettingsSwitchCell(strings.PerformanceBluetooth, strings.PerformanceBluetoothDetails, Settings.BluetoothPerformance, out bluetoothPerformanceSwitch);
            bluetoothPerformanceSwitch.Toggled += (object sender, bool e) =>
            {
                Settings.BluetoothPerformance = e;
            };

            Content = new ScrollView()
            {
                Content = Content = new StackLayout
                {
                    Children =
                    {
                        gpsPerformanceStack,
                        bluetoothPerformanceStack
                    }
                }
            };
        }
    }
}


