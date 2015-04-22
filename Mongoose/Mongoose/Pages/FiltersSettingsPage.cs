using System;
using System.Globalization;
using Xamarin.Forms;
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Mongoose
{
	public class FiltersSettingPage : BaseContentPage
	{
		public FiltersSettingPage () : base()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage();
			Title = strings.Filters;

			// Category Filter
            var categoryKeys = new List<string> (FilterDefinitions.Dictionaries["category"].Keys);
            var categoryValues = new List<string> (FilterDefinitions.Dictionaries["category"].Values);
            int categoryIndex = categoryValues.IndexOf (Settings.Filters["category"]);
            DropdownView categoryDropdown;
            StackLayout categoryStack = CreateSettingsDropDownCell(strings.FiltersSettingsCategoryPrompt, categoryKeys, categoryIndex, out categoryDropdown);
            categoryDropdown.ItemSelected += (sender, e) => SetFilter("category", categoryValues[e]);

			// Audience Filter
            var audienceKeys = new List<string> (FilterDefinitions.Dictionaries["audience"].Keys);
            var audienceValues = new List<string> (FilterDefinitions.Dictionaries["audience"].Values);
            int audienceIndex = audienceValues.IndexOf (Settings.Filters["audience"]);
            DropdownView audienceDropdown;
            StackLayout audienceStack = CreateSettingsDropDownCell(strings.FiltersSettingsAudiencePrompt, audienceKeys, audienceIndex, out audienceDropdown);
            audienceDropdown.ItemSelected += (sender, e) => SetFilter("audience", audienceValues[e]);

            // Unit Filter
            var unitKeys = new List<string> (FilterDefinitions.Dictionaries["unit"].Keys);
            var untiValues = new List<string> (FilterDefinitions.Dictionaries["unit"].Values);
            int unitIndex = untiValues.IndexOf (Settings.Filters["unit"]);
            DropdownView unitDropdown;
            StackLayout unitStack = CreateSettingsDropDownCell(strings.FiltersSettingsUnitPrompt, unitKeys, unitIndex, out unitDropdown);
            unitDropdown.ItemSelected += (sender, e) => SetFilter("unit", untiValues[e]);

            //TimeRange Filter
            var timeRangeKeys = new List<string> (FilterDefinitions.TimeRangeFilter.Keys);
            var timeRangeValues = new List<string> (FilterDefinitions.TimeRangeFilter.Values);
            int timeRangeIndex = timeRangeValues.IndexOf (Settings.TimeRangeFilter.ToString());
            DropdownView timeRangeDropdown;
            StackLayout timeRangeStack = CreateSettingsDropDownCell(strings.TimeRangeFilterHeader, timeRangeKeys, timeRangeIndex, out timeRangeDropdown);
            timeRangeDropdown.ItemSelected += (sender, e) => {
                Settings.TimeRangeFilter = Int32.Parse(timeRangeValues[e]);
            };

			// Reset Filters
            StackLayout resetFiltersStack = CreateSettingsTextCell(strings.FiltersResetToDefault, "warning_icon.png");
            var resetFiltersTap = new TapGestureRecognizer();
            resetFiltersTap.Tapped += async (sender, e) => 
                {
                    resetFiltersStack.BackgroundColor = Theme.textLightColor;
                    var allow = await DisplayAlert (strings.Warning, strings.FiltersResetWarning, strings.OK, strings.Cancel);
                    if (allow) {
                        Settings.ResetFiltersToDefault();
                        GeneratePage();
                    }
                    resetFiltersStack.BackgroundColor = Theme.backgroundLightColor;
                };
            resetFiltersStack.GestureRecognizers.Add(resetFiltersTap);

            Content = new ScrollView
            {
                Content = new StackLayout
                { 
                    Children =
                    {
                        categoryStack,
                        audienceStack,
                        unitStack,
                        timeRangeStack,
                        resetFiltersStack
                    },
                    BackgroundColor = Theme.backgroundLightColor
                }
            };
		}

        /// <summary>
        /// Sets the filter.
        /// </summary>
        /// <param name="paramType">Parameter type.</param>
        /// <param name="param">Parameter.</param>
        public void SetFilter(string paramType, string param)
        {
            Dictionary<string,string> filters = Settings.Filters;
            if (filters.ContainsKey(paramType))
            {
                filters.Remove(paramType);
            }
            filters.Add(paramType, param);
            Settings.Filters = filters;
        }
	}
}


