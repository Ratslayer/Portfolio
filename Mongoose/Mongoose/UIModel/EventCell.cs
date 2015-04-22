using System;
using Xamarin.Forms;

namespace Mongoose
{
	/// <summary>
	/// Event cell template. Contains the Event title, start date, and a "new" tag.
	/// </summary>
	public class EventCell : ViewCell
	{
		public EventCell()
		{
			var image = new Image
			{
				HorizontalOptions = LayoutOptions.Start
			};
			image.SetBinding(Image.SourceProperty, new Binding("image"));
			image.HeightRequest = image.WidthRequest = 50;

			var eventLayout = CreateEventLayout();

			var viewLayout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				Children = { eventLayout, image },
				Padding = new Thickness(20,20,20,20)
			};

			View = viewLayout;
		}

		static StackLayout CreateEventLayout()
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
			eventDetailLabel.SetBinding(Label.TextProperty, "FormattedDate");

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

