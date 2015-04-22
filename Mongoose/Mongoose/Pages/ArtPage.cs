using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
	public class ArtPage : BaseContentPage
	{
		public ArtPage () : base()
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Mongoose.ArtPage"/> class.
		/// </summary>
		/// <param name="item">Item.</param>
		public ArtPage (Art item)
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
		protected void GeneratePage (Art item)
		{
			base.GeneratePage();
			Title = strings.ArtInfoPageTitle;

			var artPageLayout = new StackLayout 
			{ 
				BackgroundColor = Theme.backgroundLightColor,
				Spacing = 10
			};

			// Add labels and buttons from art item to layout
			AddPOIAttributeLabelToLayout (item.title, artPageLayout);
			AddPOIAttributeLabelToLayout (item.description, strings.Description, artPageLayout);
			AddPOIAttributeLabelToLayout (item.room, strings.Room, artPageLayout);
			AddURLButtonAndHandlerToLayout (item.url, item.title, artPageLayout);

            Content = new ScrollView {
                Content = artPageLayout,
                Padding = new Thickness(20, 20, 20, 20)
            };
		}
	}
}

