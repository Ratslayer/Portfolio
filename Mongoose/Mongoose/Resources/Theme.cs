using System;
using Xamarin.Forms;

namespace Mongoose
{
	public static class Theme
	{
		static Color concordiaGold = Color.FromRgb (237, 204, 71);
		static Color concordiaMaroon = Color.FromRgb (173, 60, 54);
		static Color concordiaMaroonPale = Color.FromRgb (209, 98, 86);
		static Color darkGray = Color.FromRgb(79, 79, 79);
		static Color mediumGray = Color.FromRgb(181, 177, 177);
		static Color lightGray = Color.FromRgb(235, 235, 235);

		public static Color textColor = darkGray;
        public static Color textLightColor = lightGray;
		public static Color textAccentDarkColor = concordiaMaroon;
		public static Color textAccentLightColor = concordiaGold;
		public static Color backgroundLightColor = Color.White;
		public static Color backgroundDarkColor = concordiaMaroon;

		public static Button CreateButton(string text, Color textColor, Color backgroundColor){
			return new Button {
				Text = text,
				TextColor = textColor,
				BackgroundColor = backgroundColor,
				VerticalOptions = LayoutOptions.End
			};
		}

		public static Button CreateButton(string text, Color textColor, Color backgroundColor, String imageFile){
			return new Button {
				Text = text,
				TextColor = textColor,
				BackgroundColor = backgroundColor,
				Image = imageFile,
                VerticalOptions = LayoutOptions.CenterAndExpand
			};
		}

		public static Label CreateHeaderLabel(string headerText)
		{
			return new Label {
				Text = headerText,
				TextColor = textAccentDarkColor,
				HorizontalOptions = LayoutOptions.Center
			};
		}

		public static Label CreateLabel(string text)
		{
			return new Label {
				Text = text,
				TextColor = textAccentDarkColor,
				HorizontalOptions = LayoutOptions.Start
			};
		}
	}

	public class POICell : ImageCell
	{
		public POICell() : base()
		{
			TextColor = Theme.textAccentDarkColor;
		}
	}
}
