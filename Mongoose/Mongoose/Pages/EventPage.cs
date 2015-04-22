using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Mongoose
{
	public class EventPage : BaseContentPage
	{
		List<Favorite> favorites = POICollections.Instance().eventFavorites;

		Button favoriteAddButton = Theme.CreateButton (strings.AddToFavorites, Theme.textColor, Theme.backgroundLightColor);
		Button favoriteRemoveButton = Theme.CreateButton (strings.RemoveFromFavoritesTitle, Theme.textColor, Theme.backgroundLightColor);

		public EventPage (Event item)
		{
			MessagingCenter.Subscribe<LanguagesPage> (this, "Language changed.", sender => GeneratePage (item));
			if(item != null)
			{
				GeneratePage(item);
			}
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		/// <param name="item">Item.</param>
		protected void GeneratePage (Event item) 
		{
			base.GeneratePage();
			Title = strings.EventInfoPageTitle;
				
			var eventItemLayout = new StackLayout 
			{
				BackgroundColor = Theme.backgroundLightColor
			};

            string title = item.title;
            string date = BuildDateString(item);
            string time = BuildTimeString(item);
            string location = BuildLocationString(item);

			// Add labels and buttons from event to layout
			AddPOIAttributeLabelToLayout (title, eventItemLayout);
            AddPOIAttributeLabelToLayout (date, strings.Date, eventItemLayout);
			AddPOIAttributeLabelToLayout (time, strings.Time, eventItemLayout);
            AddPOIAttributeLabelToLayout (location, strings.Location, eventItemLayout);
            AddPOIAttributeLabelToLayout(item.description, strings.Description, eventItemLayout);
			AddURLButtonAndHandlerToLayout (item.url, item.title, eventItemLayout);			 
			eventItemLayout.Children.Add(GetAddOrRemoveFavoritesButton(item));

            Content = new ScrollView {
                Content = eventItemLayout,
                Padding = new Thickness(20, 20, 20, 20)
            };

			// Adds Event to Favorites
			favoriteAddButton.Clicked += (sender, e) =>
			{
				eventItemLayout.Children.Remove(GetAddOrRemoveFavoritesButton(item));
				lock(POICollections.Instance().eventFavorites)
				{
					favorites.Add(new Favorite(item));
				}
				eventItemLayout.Children.Add(GetAddOrRemoveFavoritesButton(item));
			};

			//Remove Event from Favorites, confirmation prompt must pass
			favoriteRemoveButton.Clicked += async (sender, e) => 
			{
				if (IsAlreadyInFavorites(item)) 
				{
					var confirmedToBeRemoved = await DisplayAlert(strings.RemoveFromFavoritesTitle, strings.RemoveFromFavoritesBody, strings.OK, strings.Cancel);
					if(confirmedToBeRemoved)
					{
						lock(favorites)
						{
							foreach (Favorite favorite in favorites) 
							{	
								Event favoritedEvent = favorite.Event;
							
								if (favoritedEvent.EqualsObject(item))
								{
									eventItemLayout.Children.Remove(GetAddOrRemoveFavoritesButton(item));
									favorites.Remove(favorite);
									eventItemLayout.Children.Add(GetAddOrRemoveFavoritesButton(item));
									break;
								} 
							}
						}
					}
				} 
				else 
				{
					await DisplayAlert(strings.RemoveFromFavoritesFailureTitle,strings.RemoveFromFavoritesFailureBody, strings.OK);
				}
			};

		}

        string BuildDateString(Event item)
        {
            return !String.IsNullOrEmpty(item.startdate) ? DateTime.Parse(item.startdate).ToString("MMM. d, yyyy") : "";
        }

        string BuildTimeString(Event item)
        {
            var time = new StringBuilder();
            if (!String.IsNullOrEmpty(item.starttime))
            {
                time.Append(item.starttime);
            }
            if (!String.IsNullOrEmpty(item.endtime))
            {
                if (!String.IsNullOrEmpty(time.ToString()))
                {
                    time.Append(" - ");
                }
                time.Append(item.endtime);
            }
            return time.ToString();
        }

        string BuildLocationString(Event item)
        {
            var location = new StringBuilder();
            if (!String.IsNullOrEmpty(item.building))
            {
                location.Append(item.building + " Building");
            }
            if (!String.IsNullOrEmpty(item.offcivic))
            {
                var temp = new StringBuilder();
                temp.Append(item.offcivic);
                if (!String.IsNullOrEmpty(location.ToString()))
                {
                    temp.Insert(0, " (");
                    temp.Append(")");
                }
                location.Append(temp);
            }
            if (!String.IsNullOrEmpty(item.room))
            {
                if (!String.IsNullOrEmpty(location.ToString()))
                {
                    location.Append(", ");
                }
                location.Append(" Room " + item.room);
            }
            if (!String.IsNullOrEmpty(item.campus))
            {
                if (!String.IsNullOrEmpty(location.ToString()))
                {
                    location.Append(", ");
                }
                location.Append(item.campus);
            }
			string result = location.ToString();
			result = result.Replace("<br />", null);
			result = result.Replace("<br>", null);
			return result;
        }

		bool IsAlreadyInFavorites (Event e) 
		{
			lock (POICollections.Instance().eventFavorites)
			{
				foreach (Favorite favorite in favorites)
				{	
					Event favoritedEvent = favorite.Event;
					if (favoritedEvent.EqualsObject (e))
					{
						return true;
					} 
				}
			}
			return false;
		}

		Button GetAddOrRemoveFavoritesButton (Event e) 
		{
			if(IsAlreadyInFavorites(e))
			{
				return favoriteRemoveButton;
			}
			return favoriteAddButton;
		}
	}
}

