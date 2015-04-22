using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Mongoose
{
	public class ArtsListPage : BaseContentPage
	{
		public ArtsListPage () : base()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage();
			Title = strings.Arts;

			ListView listView = new ListView ();

			listView.ItemsSource = Globals.arts.Arts;
            listView.ItemTemplate = new DataTemplate(typeof(ArtCell));
            listView.HasUnevenRows = true;

			// Navigates To Art Page After Selecting Specific Art Item
			listView.ItemTapped += async (sender, e) => 
			{
				if (this.ActivateTapLock ()) {
					await  Globals.AddPage (new ArtPage ((Art)e.Item));
					this.ReleaseTapLock ();
				}
				((ListView)sender).SelectedItem = null;	// Deselect the row
			};
                    
			Content = listView;
		}

	}
}
