using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.Generic;

namespace Mongoose
{
	public class ExplorePage : BaseContentPage, IEventListViewListener
	{
		ListView listView;
		StackLayout stackLayout;
		POICollections poiCollections = POICollections.Instance();
		List<Event> events = POICollections.Instance().EventsForSelectedLocation;
		Label locationPickerHeader;
		DropdownView locationDropdown;
		public ExplorePage (string building) : base(building)
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage();
			Title = strings.Explore;

			EventListViewAdapter viewAdapter = new EventListViewAdapter();

			viewAdapter.AddListener(this);
			viewAdapter.PopulateEventListForSelectedLocation(currentBuilding);

			// Locidex Dropdown
			locationPickerHeader = Theme.CreateHeaderLabel (strings.CurrentlyExploring);

			var locationKeys = new List<string> (FilterDefinitions.Locations.Keys);
			var locationValues = new List<string> (FilterDefinitions.Locations.Values);
			int locationIndex = locationValues.IndexOf (currentBuilding);
			locationDropdown = new DropdownView {
				ListItems = locationKeys,
				SelectedItemIndex = locationIndex,
				BackgroundColor = Theme.textAccentLightColor
			};

			listView = new ListView();
			listView.ItemTapped += async (sender, e) => {
				if (this.ActivateTapLock ()) {
					await Globals.AddPage (new EventPage ((Event)e.Item));
					this.ReleaseTapLock ();
				}
			};
			listView.ItemTemplate = new DataTemplate(typeof(EventCell));
			listView.ItemTemplate.SetBinding(TextCell.TextProperty, "title");

			stackLayout = new StackLayout();
			stackLayout.BackgroundColor = Theme.backgroundLightColor;
			stackLayout.Children.Add(locationPickerHeader);
			stackLayout.Children.Add(locationDropdown);

			locationDropdown.ItemSelected += async (sender, e) => {
				if (!currentBuilding.Equals(locationValues[e]))
				{
					if (this.ActivateTapLock()) {
						await Globals.AddPage(new ExplorePage(locationValues[e]),false);
						this.ReleaseTapLock ();
					}
					Navigation.RemovePage(this);
				}
			};

			Content = stackLayout;
		}

		public void OnEventListGenerated()
		{
			listView.BatchBegin();
			stackLayout.Children.Remove(listView);
			listView.ItemTemplate = new DataTemplate(typeof(EventCell));
			listView.ItemTemplate.SetBinding(TextCell.TextProperty, "title");
			listView.ItemsSource = events;
			stackLayout.Children.Add(listView);
			listView.BatchCommit();
		}
	}
}


