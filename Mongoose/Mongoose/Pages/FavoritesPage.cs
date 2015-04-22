using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
	public class FavoritesPage : BaseContentPage
	{
		Label defaultEmptyFavoritesLabel = Theme.CreateHeaderLabel (strings.NoFavorites);
		Button removeAllFavorites = Theme.CreateButton (strings.RemoveAllFavoritesTitle, Theme.textColor, Theme.backgroundLightColor);

		StackLayout favoriteListLayout = new StackLayout 
		{
			Children = {}, BackgroundColor = Theme.backgroundLightColor
		};	
		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing () 
		{ 		
			List<Favorite> favorites = POICollections.Instance().eventFavorites;
			favoriteListLayout.Children.Clear ();

			if (favorites.Count == 0) 
			{
				defaultEmptyFavoritesLabel.XAlign = TextAlignment.Center;
				defaultEmptyFavoritesLabel.YAlign = TextAlignment.Center;
				Content = defaultEmptyFavoritesLabel;
			} 
			else 
			{
				ListView listView = new ListView ();
				listView.ItemsSource = favorites;

                listView.ItemTemplate = new DataTemplate(typeof(FavoriteCell));
                listView.HasUnevenRows = true;

				listView.ItemTapped += async (sender, e) => 
				{
					Favorite favorite = (Favorite)e.Item;
					if (this.ActivateTapLock ()) {
						await Globals.AddPage (new EventPage (favorite.Event));
						this.ReleaseTapLock ();
					}
					((ListView)sender).SelectedItem = null;	// Deselect the row
				};

				favoriteListLayout.Children.Add (listView);
				favoriteListLayout.Children.Add (removeAllFavorites);
				Content = favoriteListLayout;
			}
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage()
		{
			base.GeneratePage();
			Title = strings.Favorites;

			removeAllFavorites.Clicked += async (sender, e) => 
			{
				var confirmedToClearAllFavorites = await DisplayAlert (strings.RemoveAllFavoritesTitle, strings.RemoveAllFavoritesBody, strings.OK, strings.Cancel);
				if (confirmedToClearAllFavorites) 
				{
                    POICollections.Instance().ClearAllFavorites();
					this.OnAppearing();
				}
			};
				
			Padding = new Thickness (0, 20, 0, 0);
		}
	}
}

