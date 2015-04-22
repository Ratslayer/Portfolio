using System;
using Xamarin.Forms;

namespace Mongoose
{
	/// <summary>
	/// Art cell template. Contains the Art title and location.
	/// </summary>
	public class ArtCell : ViewCell
	{
		public ArtCell()
		{
			var artLayout = CreateArtLayout();

			var viewLayout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				Children = { artLayout },
				Padding = new Thickness(20,20,20,20)
			};

			View = viewLayout;
		}

		static StackLayout CreateArtLayout()
		{
			var eventTitleLabel = new Label
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = Theme.textAccentDarkColor,
			};
			eventTitleLabel.SetBinding(Label.TextProperty, "title");

			var eventDetailLabel = new Label
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = Theme.textColor,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
			};
			eventDetailLabel.SetBinding(Label.TextProperty, "room");

			var eventLayout = new StackLayout()
			{
				Padding = new Thickness(3,3,3,3),
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Orientation = StackOrientation.Vertical,
				Children = { eventTitleLabel, eventDetailLabel }
			};
			return eventLayout;
		}
	}
}

