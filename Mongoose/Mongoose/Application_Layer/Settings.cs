using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;
using System.Collections.Generic;
using System;

namespace Mongoose
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			  get
			  {
			    return CrossSettings.Current;
			  }
		}
	
		#region Setting Constants

		private const string languageKey = "language";
		private static readonly string languageDefault = "en-US";

		private const string notifyEventsKey = "notification-events";
		private static readonly bool notifyEventsDefault = true;

		private const string notifyArtsKey = "notification-arts";
		private static readonly bool notifyArtsDefault = true;

		private const string notifyDetailsKey = "notification-details";
		private static readonly bool notifyDetailsDefault = true;

		private static readonly Dictionary<string, string> filters = new Dictionary<string, string>() { 
			{"category", ""},
			{"audience", ""},
			{"unit", ""}
		};

		private const string timeRangeFilterKey = "time-range-filter";
        private static readonly int timeRangeFilterdefault = 720;

		private const string favoritesExpirationKey = "favorites-expiration";
		private static readonly int favoritesExpirationDefault = 30;

		private const string exploreLocationKey = "explore-location";
		private static readonly string exploreLocationDefault = "H";

        private const string gpsPerformanceKey = "gps-performance";
        private static readonly bool gpsPerformanceDefault = false;

        private const string bluetoothPerformanceKey = "bluetooth-performance";
        private static readonly bool bluetoothPerformanceDefault = false;

		#endregion

		/// <summary>
		/// Gets and set for the language setting
		/// </summary>
		/// <value>The language.</value>
		public static string Language
		{
				get { return AppSettings.GetValueOrDefault(languageKey, languageDefault); }
				set	{ AppSettings.AddOrUpdateValue(languageKey, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Mongoose.Settings"/> notify events.
		/// </summary>
		/// <value><c>true</c> if notify events; otherwise, <c>false</c>.</value>
		public static bool NotifyEvents
		{
			get { return AppSettings.GetValueOrDefault(notifyEventsKey, notifyEventsDefault); }
			set	{ AppSettings.AddOrUpdateValue(notifyEventsKey, value); }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Mongoose.Settings"/> notify arts.
		/// </summary>
		/// <value><c>true</c> if notify arts; otherwise, <c>false</c>.</value>
		public static bool NotifyArts
		{
			get { return AppSettings.GetValueOrDefault(notifyArtsKey, notifyArtsDefault); }
			set	{ AppSettings.AddOrUpdateValue(notifyArtsKey, value); }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Mongoose.Settings"/> notify details.
		/// </summary>
		/// <value><c>true</c> if notify details; otherwise, <c>false</c>.</value>
		public static bool NotifyDetails
		{
			get { return AppSettings.GetValueOrDefault(notifyDetailsKey, notifyDetailsDefault); }
			set	{ AppSettings.AddOrUpdateValue(notifyDetailsKey, value); }
		}
		/// <summary>
		/// Gets or sets the time range filter.
		/// </summary>
		/// <value>The time range filter.</value>
		public static int TimeRangeFilter
		{
			get { return AppSettings.GetValueOrDefault(timeRangeFilterKey, timeRangeFilterdefault); }
			set	{ AppSettings.AddOrUpdateValue(timeRangeFilterKey, value); }
		}
		/// <summary>
		/// Gets or sets the favorites expiration.
		/// </summary>
		/// <value>The favorites expiration.</value>
		public static int FavoritesExpiration
		{
			get { return AppSettings.GetValueOrDefault(favoritesExpirationKey, favoritesExpirationDefault); }
			set	{ AppSettings.AddOrUpdateValue(favoritesExpirationKey, value); }
		}
		/// <summary>
		/// Gets or sets the explore location.
		/// </summary>
		/// <value>The explore location.</value>
		public static string ExploreLocation
		{
			get { return AppSettings.GetValueOrDefault(exploreLocationKey, exploreLocationDefault); }
			set	{ AppSettings.AddOrUpdateValue(exploreLocationKey, value); }
		}
        /// <summary>
        /// Gets or sets the gps performance.
        /// </summary>
        /// <value>The GPS performance.</value>
        public static bool GPSPerformance
        {
            get { return AppSettings.GetValueOrDefault(gpsPerformanceKey, gpsPerformanceDefault); }
            set { AppSettings.AddOrUpdateValue(gpsPerformanceKey, value); }
        }
        /// <summary>
        /// Gets or sets the bluetooth performance.
        /// </summary>
        /// <value>The bluetooth performance.</value>
        public static bool BluetoothPerformance
        {
            get { return AppSettings.GetValueOrDefault(bluetoothPerformanceKey, bluetoothPerformanceDefault); }
            set { AppSettings.AddOrUpdateValue(bluetoothPerformanceKey, value); }
        }
		/// <summary>
		/// Gets or sets the filters.
		/// </summary>
		/// <value>The filters.</value>
		public static Dictionary<string,string> Filters
		{
			get 
			{
				string[] keys = new string[filters.Count];
				filters.Keys.CopyTo(keys, 0);
				Dictionary<string,string> savedFilters = new Dictionary<string, string>();
				foreach (string key in keys) {
					savedFilters[key] = AppSettings.GetValueOrDefault (key, filters[key]);
				}
				return savedFilters; 
			}
			set 
			{ 
				Dictionary<string,string> newFilters = value;
				foreach (string key in newFilters.Keys) {
					if(filters.ContainsKey(key)){
						AppSettings.AddOrUpdateValue(key, newFilters[key]);
					} else {
						throw new KeyNotFoundException("The selected filter is not a valid system filter.");
					}
				}
			}
		}
		/// <summary>
		/// Reset the filters to its default
		/// </summary>
		public static void ResetFiltersToDefault()
		{
			Filters = filters;
            AppSettings.AddOrUpdateValue(timeRangeFilterKey, timeRangeFilterdefault);
		}

		/// <summary>
		/// This will reset the settings to it's default value
		/// </summary>
		public static void ResetSettings()
		{
			ResetFiltersToDefault();
			AppSettings.AddOrUpdateValue(notifyEventsKey, notifyEventsDefault);
			AppSettings.AddOrUpdateValue(notifyArtsKey, notifyArtsDefault);
			AppSettings.AddOrUpdateValue(notifyDetailsKey, notifyDetailsDefault);
			AppSettings.AddOrUpdateValue(favoritesExpirationKey, favoritesExpirationDefault);
            AppSettings.AddOrUpdateValue(gpsPerformanceKey, gpsPerformanceDefault);
            AppSettings.AddOrUpdateValue(bluetoothPerformanceKey, bluetoothPerformanceDefault);
		}
	}
}