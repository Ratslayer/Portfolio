using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
	public class BaseContentPage : ContentPage, ITapLock
	{
		TapLockStatus ITapLock.TapLockStatus { get; set; }
		/// <summary>
		/// The current building.
		/// </summary>
		protected string currentBuilding;
		/// <summary>
		/// Initializes a new instance of the <see cref="Mongoose.BaseContentPage"/> class.
		/// </summary>
		/// <param name="building">Building.</param>
		/// <param name="languageCanChange">If set to <c>true</c> language can change.</param>
		public BaseContentPage (string building = "", bool languageCanChange = true)
		{
			currentBuilding = building;
			if(languageCanChange)
			{
				MessagingCenter.Subscribe<LanguagesPage>(this, "Language changed.", sender => GeneratePage());
			}
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public virtual void GeneratePage () 
		{
			BackgroundColor = Theme.backgroundLightColor;
		}


		/******************************************************************/
		/*          General functions for creating page layouts			  */
		/******************************************************************/

		protected void AddPOIAttributeLabelToLayout (string attribute, string labelText, StackLayout pageLayout) {
			if (!String.IsNullOrEmpty (attribute)) {
				Label attrLabel = Theme.CreateLabel (labelText);
                Label itemLabel = Theme.CreateLabel(attribute);
                itemLabel.TextColor = Theme.textColor;
                var attrFrame = new Frame()
                {
                    Content = attrLabel,
                    Padding = new Thickness(0, 10, 0, 0)
                };
                var itemFrame = new Frame()
                {
                    Content = itemLabel,
                    Padding = new Thickness(0, 0, 0, 5)
                };
				pageLayout.Children.Add(attrFrame);
                pageLayout.Children.Add(itemFrame);
			}
		}

		// Overload for labels without label text
		protected void AddPOIAttributeLabelToLayout (string attribute, StackLayout pageLayout) {
			if (!String.IsNullOrEmpty (attribute)) {
				Label attrLabel = Theme.CreateLabel (attribute);
                attrLabel.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
				pageLayout.Children.Add(attrLabel);
			}
		}

		protected void AddPOIListLabelToLayout (List<string> values, string labelText, StackLayout pageLayout) {
			if (values.Any ()) {
				string valuesStr = BuildStringFromStringList (values);
				Label valuesLabel = Theme.CreateLabel (labelText);
                Label itemLabel = Theme.CreateLabel(valuesStr);
                itemLabel.TextColor = Theme.textColor;
				pageLayout.Children.Add(valuesLabel);
                pageLayout.Children.Add(itemLabel);
			}
		}
		/// <summary>
		/// Adds the URL button and handler to layout.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="title">Title.</param>
		/// <param name="pageLayout">Page layout.</param>
		protected void AddURLButtonAndHandlerToLayout (string url, string title, StackLayout pageLayout) {
			if (!String.IsNullOrEmpty (url)) {
				Button urlButton = Theme.CreateButton (strings.VisitWebsite, Theme.textColor, Theme.backgroundLightColor);
				pageLayout.Children.Add (urlButton);

				urlButton.Clicked += async (sender, e) => {
					var go = await DisplayAlert (title, strings.Visit + " " + "\"" + title + "\"?", strings.OK, strings.Cancel);
					if (go) {
						Device.OpenUri (new Uri (url));
					}
				};
			}
		}
		/// <summary>
		/// Builds the string from string list.
		/// </summary>
		/// <returns>The string from string list.</returns>
		/// <param name="list">List.</param>
		string BuildStringFromStringList (List<string> list)
		{
			string str = "";
			foreach (string item in list) 
			{
				string s = item;
				if(s.IndexOf(":") > 0)
				{
					s = s.Substring(s.IndexOf(":") + 1);
				}
				if(String.IsNullOrEmpty(str))
				{
					str += s;
				}
				else
				{
					str += ", " + s;
				}
			}
			return str;
		}
		/// <summary>
		/// Creates the settings switch cell.
		/// </summary>
		/// <returns>The settings switch cell.</returns>
		/// <param name="title">Title.</param>
		/// <param name="subtitle">Subtitle.</param>
		/// <param name="switchValue">If set to <c>true</c> switch value.</param>
		/// <param name="switchHook">Switch hook.</param>
        protected StackLayout CreateSettingsSwitchCell(string title, string subtitle, bool switchValue, out CustomSwitch switchHook)
        {
            Label notificationHeader = CreateHeader(title);

            var notificationSubtext = new Label
                {
                    Text = subtitle,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = Theme.textColor
                };

            var toggle = new CustomSwitch()
                {
                    IsToggled = switchValue,
                    HorizontalOptions = LayoutOptions.Start
                };
            switchHook = toggle;

            StackLayout settingLayout = CreateBaseSettingsLayout(notificationHeader, 3, StackOrientation.Vertical);
            settingLayout.Children.Add(notificationSubtext);

            StackLayout viewLayout = CreateSecondarySettingsLayout(settingLayout, 15, StackOrientation.Horizontal);
            viewLayout.Children.Add(toggle);

            StackLayout masterLayout = CreateMasterSettingLayout(viewLayout);

            return masterLayout;
        }
		/// <summary>
		/// Creates the settings sub cell.
		/// </summary>
		/// <returns>The settings sub cell.</returns>
		/// <param name="title">Title.</param>
		/// <param name="subtitle">Subtitle.</param>
        protected StackLayout CreateSettingsSubCell(string title, string subtitle)
        {   
            var notificationHeader = CreateHeader(title);

            var notificationSubtext = new Label
                {
                    Text = subtitle,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = Theme.textColor
                };

            StackLayout settingLayout = CreateBaseSettingsLayout(notificationHeader, 15, StackOrientation.Vertical);
            settingLayout.Children.Add(notificationSubtext);

            var masterLayout = CreateMasterSettingLayout(settingLayout);

            return masterLayout;

        }
		/// <summary>
		/// Creates the settings drop down cell.
		/// </summary>
		/// <returns>The settings drop down cell.</returns>
		/// <param name="title">Title.</param>
		/// <param name="dropdownValues">Dropdown values.</param>
		/// <param name="dropdownSelectIndex">Dropdown select index.</param>
		/// <param name="dropdownHook">Dropdown hook.</param>
        protected StackLayout CreateSettingsDropDownCell(string title, List<string> dropdownValues, int dropdownSelectIndex, out DropdownView dropdownHook)
        {
            var dropdown = new DropdownView {
                ListItems = dropdownValues,
                SelectedItemIndex = dropdownSelectIndex,
                BackgroundColor = Theme.textAccentLightColor,
            };
            dropdownHook = dropdown;

            Label notificationHeader = CreateHeader(title);

            StackLayout settingLayout = CreateBaseSettingsLayout(notificationHeader, 3, StackOrientation.Vertical);

            StackLayout viewLayout = CreateSecondarySettingsLayout(settingLayout, 15, StackOrientation.Vertical);
            viewLayout.Children.Add(dropdown);

            StackLayout masterLayout = CreateMasterSettingLayout(viewLayout);

            return masterLayout;
        }
		/// <summary>
		/// Creates the settings drop down cell.
		/// </summary>
		/// <returns>The settings drop down cell.</returns>
		/// <param name="title">Title.</param>
		/// <param name="dropdownValues">Dropdown values.</param>
		/// <param name="dropdownHook">Dropdown hook.</param>
        protected StackLayout CreateSettingsDropDownCell(string title, List<string> dropdownValues, out DropdownMultiSelectView dropdownHook)
        {
            var dropdown = new DropdownMultiSelectView {
                ListItems = dropdownValues,
                BackgroundColor = Theme.textAccentLightColor,
            };
            dropdownHook = dropdown;

            Label notificationHeader = CreateHeader(title);

            StackLayout settingLayout = CreateBaseSettingsLayout(notificationHeader, 3, StackOrientation.Vertical);

            StackLayout viewLayout = CreateSecondarySettingsLayout(settingLayout, 15, StackOrientation.Vertical);
            viewLayout.Children.Add(dropdown);

            StackLayout masterLayout = CreateMasterSettingLayout(viewLayout);

            return masterLayout;
        }


		/// <summary>
		/// Creates the settings text cell.
		/// </summary>
		/// <returns>The settings text cell.</returns>
		/// <param name="title">Title.</param>
		/// <param name="imageSourcePath">Image source path.</param>
		protected StackLayout CreateSettingsTextCell(string title, string imageSourcePath)
        {
			var image = new Image {
				Aspect = Aspect.AspectFit
			};
			image.Source = imageSourcePath;

            Label notificationHeader = CreateHeader(title);

            StackLayout settingLayout = CreateBaseSettingsLayout(notificationHeader, 3, StackOrientation.Vertical);

            StackLayout viewLayout = CreateSecondarySettingsLayout(settingLayout, 15, StackOrientation.Horizontal);
            viewLayout.Children.Add(image);

            StackLayout masterLayout = CreateMasterSettingLayout(viewLayout); 

            return masterLayout;
        }
		/// <summary>
		/// Creates the settings text cell.
		/// </summary>
		/// <returns>The settings text cell.</returns>
		/// <param name="title">Title.</param>
		protected StackLayout CreateSettingsTextCell(string title)
		{
            Label notificationHeader = CreateHeader(title);

            StackLayout settingLayout = CreateBaseSettingsLayout(notificationHeader, 15, StackOrientation.Vertical);

            StackLayout masterLayout = CreateMasterSettingLayout(settingLayout); 

			return masterLayout;
		}
		/// <summary>
		/// Creates the base settings layout.
		/// </summary>
		/// <returns>The base settings layout.</returns>
		/// <param name="stackedView">Stacked view.</param>
		/// <param name="padding">Padding.</param>
		/// <param name="orientation">Orientation.</param>
        StackLayout CreateBaseSettingsLayout(View stackedView, int padding, StackOrientation orientation)
        {

            StackLayout layout = CreateSecondarySettingsLayout(stackedView, padding, orientation);
            layout.HorizontalOptions = LayoutOptions.StartAndExpand;
            return layout;
        }
		/// <summary>
		/// Creates the secondary settings layout.
		/// </summary>
		/// <returns>The secondary settings layout.</returns>
		/// <param name="stackedView">Stacked view.</param>
		/// <param name="padding">Padding.</param>
		/// <param name="orientation">Orientation.</param>
        StackLayout CreateSecondarySettingsLayout(View stackedView, int padding, StackOrientation orientation)
        {
            return new StackLayout()
            {
                Padding = new Thickness(padding),
                Orientation = orientation,
                Children = { stackedView }
            };
        }
		/// <summary>
		/// Creates the master setting layout.
		/// </summary>
		/// <returns>The master setting layout.</returns>
		/// <param name="stackedView">Stacked view.</param>
        StackLayout CreateMasterSettingLayout(View stackedView)
        {
            return  new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { stackedView, CreateLine() }
            };
        }
		/// <summary>
		/// Creates the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <param name="title">Title.</param>
        Label CreateHeader(string title)
        {
            return new Label
            {
                Text = title,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Theme.textAccentDarkColor
            };
        }
		/// <summary>
		/// Creates the line.
		/// </summary>
		/// <returns>The line.</returns>
        protected BoxView CreateLine() {
            var line = new BoxView() {
                Color = Theme.textLightColor,
                HeightRequest = 1
            };

            return line;
        }
		/// <summary>
		/// Determines if is portrait the specified p.
		/// </summary>
		/// <returns><c>true</c> if is portrait the specified p; otherwise, <c>false</c>.</returns>
		/// <param name="p">P.</param>
        protected static bool IsPortrait(Page p) { return p.Width < p.Height; }
	}
}


