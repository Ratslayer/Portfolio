using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
	public class FavoritesSettingsPage : BaseContentPage
	{
		public FavoritesSettingsPage () : base()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage();
			Title = strings.Favorites;

			// Favorites Expiration Setting
			var favoritesRangeKeys = new List<string> (FilterDefinitions.FavoritesRange.Keys);
			var favoritesRangeValues = new List<string> (FilterDefinitions.FavoritesRange.Values);
			int favoritesRangeIndex = favoritesRangeValues.IndexOf (Settings.FavoritesExpiration.ToString());
            DropdownView favoritesRangeDropdown;
            StackLayout favoritesRangeStack = CreateSettingsDropDownCell(strings.FavoritesExpireDate, favoritesRangeKeys, favoritesRangeIndex, out favoritesRangeDropdown);
            favoritesRangeDropdown.ItemSelected += (sender, e) => {
                Settings.FavoritesExpiration = Int32.Parse(favoritesRangeValues[e]);
            };

			Content = new StackLayout { 
				Children = {
                    favoritesRangeStack
				},
				BackgroundColor = Theme.backgroundLightColor
			};
		}
	}
}


