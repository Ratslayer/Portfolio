using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
	public class LandingPage : BaseContentPage
	{
        AbsoluteLayout bigIconLayout = new AbsoluteLayout();
        AbsoluteLayout smallIconLayout = new AbsoluteLayout();
        AbsoluteLayout landingPageLayout = new AbsoluteLayout();

        Grid eventLayout;
        Grid artLayout;
        Grid visualizeLayout;
        Grid favoriteLayout;
        Grid settingsLayout;
        Grid aboutLayout;

		public LandingPage () : base()
		{
            SizeChanged += (sender, e) => GeneratePage();
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage ();
            Title = strings.ArtEvent;

            // Big icons
            eventLayout = CreateBigIconButton("eventIMG_2.png", strings.Events);
            artLayout = CreateBigIconButton("artIMG.png", strings.Arts);
            visualizeLayout = CreateBigIconButton("cameraIMG.png", strings.Visualise);
            favoriteLayout = CreateBigIconButton("heartIMG.png", strings.Favorites);
            settingsLayout = CreateSmallIconButton("cogIMG.png", strings.Settings);
            aboutLayout = CreateSmallIconButton("aboutIMG.png", strings.About);

            var eventTap = new TapGestureRecognizer();
            eventTap.Tapped += async (sender, e) =>
            {
                eventLayout.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
                	await Globals.AddPage(new EventList(Globals.LastBuilding));
					this.ReleaseTapLock ();
				}
				eventLayout.BackgroundColor = Theme.backgroundLightColor;
            };
            eventLayout.GestureRecognizers.Add(eventTap);

            var artTap = new TapGestureRecognizer();
            artTap.Tapped += async (sender, e) =>
            {
                artLayout.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
                	await Globals.AddPage(new ArtsListPage());
					this.ReleaseTapLock ();
				}
                artLayout.BackgroundColor = Theme.backgroundLightColor;
            };
            artLayout.GestureRecognizers.Add(artTap);

            var visualizeTap = new TapGestureRecognizer();
            visualizeTap.Tapped += async (sender, e) =>
            {
                visualizeLayout.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
					await Globals.AddPage(new ARPage());
					this.ReleaseTapLock ();
				}
                visualizeLayout.BackgroundColor = Theme.backgroundLightColor;
            };
            visualizeLayout.GestureRecognizers.Add(visualizeTap);

            var favoriteTap = new TapGestureRecognizer();
            favoriteTap.Tapped += async (sender, e) =>
            {
                favoriteLayout.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
					await Globals.AddPage(new FavoritesPage());
					this.ReleaseTapLock ();
				}
                favoriteLayout.BackgroundColor = Theme.backgroundLightColor;
            };
            favoriteLayout.GestureRecognizers.Add(favoriteTap);

            var settingsTap = new TapGestureRecognizer();
            settingsTap.Tapped += async (sender, e) =>
            {
                settingsLayout.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
					await Globals.AddPage(new SettingsPage());
					this.ReleaseTapLock ();
				}
                settingsLayout.BackgroundColor = Theme.backgroundLightColor;
            };
            settingsLayout.GestureRecognizers.Add(settingsTap);

            var aboutTap = new TapGestureRecognizer();
            aboutTap.Tapped += async (sender, e) =>
            {
                aboutLayout.BackgroundColor = Theme.textLightColor;
				if (this.ActivateTapLock ()) {
                	await Globals.AddPage(new AboutPage());
					this.ReleaseTapLock ();
				}
                aboutLayout.BackgroundColor = Theme.backgroundLightColor;
            };
            aboutLayout.GestureRecognizers.Add(aboutTap);

            AdjustMasterLayout();

            Content = landingPageLayout;

		}

        Grid CreateBigIconButton(string imagePath, string text)
        {
            var layout = new Grid(){
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(0.40, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            var image = new Image()
                { 
                    Source = imagePath 
                };
            layout.Children.Add(image, 0, 0);

            var label = new Label()
                { 
                    Text = text, 
                    TextColor = Theme.textColor,
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    HorizontalOptions = LayoutOptions.Center, 
                    VerticalOptions = LayoutOptions.Center 
                };
            layout.Children.Add(label, 0, 1);

            return layout;
        }

        Grid CreateSmallIconButton(string imagePath, string text)
        {
            var layout = new Grid(){
                RowDefinitions = {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) }
                }
            };

            var image = new Image()
                { 
                    Source = imagePath 
                };
            layout.Children.Add(image, 0, 0);

            var label = new Label()
                { 
                    Text = text, 
                    TextColor = Theme.textColor,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    HorizontalOptions = LayoutOptions.Start, 
                    VerticalOptions = LayoutOptions.Center 
                };
            layout.Children.Add(label, 1, 0);

            return layout;
        }

        void AdjustMasterLayout()
        {
            Thickness bigIconPadding;
            Thickness bigIconSectionPadding;
            float defaultBigIconHeight = 0.5f;
            float defaultBigIconWidth = 0.5f;
            float bigIconSize = 0.8f;
            Thickness smallIconPadding;
            Thickness smallIconSectionPadding;
            float defaultSmallIconHeight = 0.8f;
            float defaultSmallIconWidth = 0.5f;
            float smallIconSize = 0.2f;

            if (IsPortrait(this))
            {
                bigIconPadding = new Thickness(10, 50, 10, 50);
                bigIconSectionPadding = new Thickness(25, 25, 25, 15);
                smallIconPadding = new Thickness(20, 5, 10, 5);
                smallIconSectionPadding = new Thickness(25, 10, 15, 10);
            }
            else
            {
                bigIconPadding = new Thickness(50, 5, 50, 5);
                bigIconSectionPadding = new Thickness(20, 10, 20, 10);
                smallIconPadding = new Thickness(80, 5, 70, 5);
                smallIconSectionPadding = new Thickness(30, 5, 20, 2);
            }

            bigIconLayout = new AbsoluteLayout();
            smallIconLayout = new AbsoluteLayout();
            landingPageLayout = new AbsoluteLayout();

            // Big Icons
            eventLayout.Padding = bigIconPadding;
            AbsoluteLayout.SetLayoutFlags(eventLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(eventLayout, new Rectangle(0f, 0f, defaultBigIconWidth, defaultBigIconHeight));
            bigIconLayout.Children.Add(eventLayout);

            artLayout.Padding = bigIconPadding;
            AbsoluteLayout.SetLayoutFlags(artLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(artLayout, new Rectangle(1f, 0f, defaultBigIconWidth, defaultBigIconHeight));
            bigIconLayout.Children.Add(artLayout);

            visualizeLayout.Padding = bigIconPadding;
            AbsoluteLayout.SetLayoutFlags(visualizeLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(visualizeLayout, new Rectangle(0f, 1f, defaultBigIconWidth, defaultBigIconHeight));
            bigIconLayout.Children.Add(visualizeLayout);

            favoriteLayout.Padding = bigIconPadding;
            AbsoluteLayout.SetLayoutFlags(favoriteLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(favoriteLayout, new Rectangle(1f, 1f, defaultBigIconWidth, defaultBigIconHeight));
            bigIconLayout.Children.Add(favoriteLayout);

            // Small icons
            settingsLayout.Padding = smallIconPadding;
            AbsoluteLayout.SetLayoutFlags(settingsLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(settingsLayout, new Rectangle(0f, 0.5f, defaultSmallIconWidth, defaultSmallIconHeight));
            smallIconLayout.Children.Add(settingsLayout);

            aboutLayout.Padding = smallIconPadding;
            AbsoluteLayout.SetLayoutFlags(aboutLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(aboutLayout, new Rectangle(1f, 0.5f, defaultSmallIconWidth, defaultSmallIconHeight));
            smallIconLayout.Children.Add(aboutLayout);

            var smallIconStack = new StackLayout()
                {
                    Children = { CreateLine(), smallIconLayout }
                };

            // Master Layout
            bigIconLayout.Padding = bigIconSectionPadding;
            AbsoluteLayout.SetLayoutFlags(bigIconLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(bigIconLayout, new Rectangle(0.5f, 0f, 1f, bigIconSize));
            landingPageLayout.Children.Add(bigIconLayout);

            smallIconStack.Padding = smallIconSectionPadding;
            AbsoluteLayout.SetLayoutFlags(smallIconStack, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(smallIconStack, new Rectangle(0.5f, 1f, 1f, smallIconSize));
            landingPageLayout.Children.Add(smallIconStack);
        }
	}
}
	