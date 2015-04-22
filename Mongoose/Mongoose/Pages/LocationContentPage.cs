using System;
using Xamarin.Forms;
namespace Mongoose
{
	public class LocationContentPage : BaseContentPage
	{
		public Page parent;
		protected ToolbarItem locationLabel;
		public LocationContentPage(string building):base(building, false)
		{
		}
		~LocationContentPage()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage()
		{
			base.GeneratePage();
			string building = strings.Building + ": " + currentBuilding;
			Command ShowListCommand = new Command (async (sender) => {
				// Need to localize menu options and "building" text 
				string[] buildings = Globals.buildingAddressCollection.GetBuildingNames();
				MarkCurrentBuilding(buildings, Globals.LastBuilding);
				string result = await DisplayActionSheet (strings.SelectBuilding, strings.Cancel, null, buildings);
				if(result!=null && result!=strings.Cancel)
				{
					UnmarkCurrentBuilding(ref result);
                    await Globals.AddPage(new EventList(result), false);
					Navigation.RemovePage(this);
				}
			});

			locationLabel = new ToolbarItem ()
			{
				Text = building,
				Command = ShowListCommand
			};
			ToolbarItems.Clear();
			ToolbarItems.Add(locationLabel);
		}
		private void UnmarkCurrentBuilding(ref string building)
		{
			string[] words = building.Split(new char[]{' '},3);
			if(words.Length==3)
			{
				building=words[0];
			}
		}
		private void MarkCurrentBuilding(string[] buildings, string building)
		{
			for(int i = 0; i < buildings.Length; i++)
			{
				if(buildings[i] == building)
				{
					buildings[i] += " - " + strings.Current;
					break;
				}
			}
		}
	}
}