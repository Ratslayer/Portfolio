using System;

using Xamarin.Forms;

namespace Mongoose
{
    public class SettingsPage : BaseContentPage
    {
        public SettingsPage() : base()
        {
            SizeChanged += (sender, e) => GeneratePage();
        }

        CustomSwitch notifyEventsSwitch;
        CustomSwitch notifyArtsSwitch;
        CustomSwitch notifyDetailsSwitch;
		/// <summary>
		/// Generates the page.
		/// </summary>
        public override void GeneratePage()
        {
            base.GeneratePage();
            Title = strings.Settings;

            StackLayout languageButtonStack = CreateSettingsTextCell(strings.Language, "right_caret.png");
            var languageTap = new TapGestureRecognizer();
            languageTap.Tapped += async (sender, e) => 
                {
                    languageButtonStack.BackgroundColor = Theme.textLightColor;
					if (this.ActivateTapLock ()) {
			            await Globals.AddPage(new LanguagesPage());
						this.ReleaseTapLock ();
					}
                    languageButtonStack.BackgroundColor = Theme.backgroundLightColor;
                };
            languageButtonStack.GestureRecognizers.Add(languageTap);

			StackLayout notificationsButtonStack = CreateSettingsTextCell(strings.Notifications, "right_caret.png");
            var notificationsTap = new TapGestureRecognizer();
            notificationsTap.Tapped += async (sender, e) => 
                {
                    notificationsButtonStack.BackgroundColor = Theme.textLightColor;
					if (this.ActivateTapLock ()) {
	                    await Globals.AddPage(new NotificationsSettingsPage());
						this.ReleaseTapLock();
					}
                    notificationsButtonStack.BackgroundColor = Theme.backgroundLightColor;
                };
            notificationsButtonStack.GestureRecognizers.Add(notificationsTap);

			StackLayout filtersButtonStack = CreateSettingsTextCell(strings.Filters, "right_caret.png");
            var filtersTap = new TapGestureRecognizer();
            filtersTap.Tapped += async (sender, e) => 
                {
                    filtersButtonStack.BackgroundColor = Theme.textLightColor;
					if (this.ActivateTapLock ()) {
			            await Globals.AddPage(new FiltersSettingPage());
						this.ReleaseTapLock ();
					}
                    filtersButtonStack.BackgroundColor = Theme.backgroundLightColor;
                };
            filtersButtonStack.GestureRecognizers.Add(filtersTap);

			StackLayout favoritesButtonStack = CreateSettingsTextCell(strings.Favorites, "right_caret.png");
            var favoritesTap = new TapGestureRecognizer();
            favoritesTap.Tapped += async (sender, e) => 
                {
                    favoritesButtonStack.BackgroundColor = Theme.textLightColor;
					if (this.ActivateTapLock ()) {
                    	await Globals.AddPage(new FavoritesSettingsPage());
						this.ReleaseTapLock ();
					}
                    favoritesButtonStack.BackgroundColor = Theme.backgroundLightColor;
                };
            favoritesButtonStack.GestureRecognizers.Add(favoritesTap);

			StackLayout performanceButtonStack = CreateSettingsTextCell(strings.Performance, "right_caret.png");
            var performanceTap = new TapGestureRecognizer();
            performanceTap.Tapped += async (sender, e) => 
                {
                    performanceButtonStack.BackgroundColor = Theme.textLightColor;
					if (this.ActivateTapLock ()) {
                    	await Globals.AddPage(new PerformanceSettingsPage());
						this.ReleaseTapLock ();
					}
                    performanceButtonStack.BackgroundColor = Theme.backgroundLightColor;
                };
            performanceButtonStack.GestureRecognizers.Add(performanceTap);

            StackLayout resetButtonStack = CreateSettingsTextCell(strings.ResetSettings, "warning_icon.png");
            var resetTap = new TapGestureRecognizer();
            resetTap.Tapped += async (sender, e) => 
                {
                    resetButtonStack.BackgroundColor = Theme.textLightColor;
                    var allow = await DisplayAlert(strings.Warning, strings.ResetSettingsWarning, strings.OK, strings.Cancel);
                    if (allow)
                    {
                        Settings.ResetSettings();
                        GeneratePage();
                    }
                    resetButtonStack.BackgroundColor = Theme.backgroundLightColor;
                };
            resetButtonStack.GestureRecognizers.Add(resetTap);

            var scrollview = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        languageButtonStack,
                        notificationsButtonStack,
                        filtersButtonStack,
                        favoritesButtonStack,
                        performanceButtonStack,
                        resetButtonStack
                    },
                    BackgroundColor = Theme.backgroundLightColor
                }
            };

            // Notify Events
            Label notifyEventsHeader = Theme.CreateHeaderLabel(strings.NotifyEventsShort);
            notifyEventsSwitch = new CustomSwitch(){
                IsToggled = Settings.NotifyEvents
            };
            notifyEventsSwitch.Toggled += (sender, e) =>
            {
                Settings.NotifyEvents = e;
            };

            // Notify Arts
            Label notifyArtsHeader = Theme.CreateHeaderLabel(strings.NotifyArtsShort);
            notifyArtsSwitch = new CustomSwitch(){
                IsToggled = Settings.NotifyArts
            };
            notifyArtsSwitch.Toggled += (sender, e) =>
            {
                Settings.NotifyArts = e;
            };

            // Notify Details
            Label notifyDetailsHeader = Theme.CreateHeaderLabel(strings.NotifyDetailsShort);
            notifyDetailsSwitch = new CustomSwitch(){
                IsToggled = Settings.NotifyDetails
            };
            notifyDetailsSwitch.Toggled += (sender, e) =>
            {
                Settings.NotifyDetails = e;
            };

            var gridLayout = new Grid
            {
                BackgroundColor= Theme.backgroundLightColor,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength (0.15, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (0.15, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (0.15, GridUnitType.Star) }
                }
            };

            Thickness gridPadding;

            gridPadding = IsPortrait(this) ? new Thickness(10, 0, 10, 0) : new Thickness(40, 0, 40, 0);

            Grid notifyEventsGrid = CreateCheckBoxCluster(notifyEventsHeader, notifyEventsSwitch);
            notifyEventsGrid.Padding = gridPadding;
            Grid notifyArtsGrid = CreateCheckBoxCluster(notifyArtsHeader, notifyArtsSwitch);
            notifyArtsGrid.Padding = gridPadding;
            Grid notifyDetailsGrid = CreateCheckBoxCluster(notifyDetailsHeader, notifyDetailsSwitch);
            notifyDetailsGrid.Padding = gridPadding;

            gridLayout.Children.Add(notifyEventsGrid, 0, 0);
            gridLayout.Children.Add(notifyArtsGrid, 1, 0);
            gridLayout.Children.Add(notifyDetailsGrid, 2, 0);

                gridLayout.Padding = new Thickness(10, 15, 10, 5);

            var absoluteLayout = new AbsoluteLayout
            { 
                BackgroundColor = Theme.backgroundLightColor,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutFlags(scrollview, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(scrollview, new Rectangle(0.5f, 0f, 1f, 0.8));
            absoluteLayout.Children.Add(scrollview);
            AbsoluteLayout.SetLayoutFlags(gridLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(gridLayout, new Rectangle(0.5f, 1f, 1f, 0.2));
            absoluteLayout.Children.Add(gridLayout);

            Content = absoluteLayout;
        }

        protected override void OnAppearing()
        {
            notifyEventsSwitch.IsToggled = Settings.NotifyEvents;
            notifyArtsSwitch.IsToggled = Settings.NotifyArts;
            notifyDetailsSwitch.IsToggled = Settings.NotifyDetails;
        }

        Grid CreateCheckBoxCluster(Label text, CustomSwitch toggle)
        {
            var layout = new Grid(){
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(0.325, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.675, GridUnitType.Star) }
                }
            };
                   
            text.VerticalOptions = LayoutOptions.Center;
            text.HorizontalOptions = LayoutOptions.Start;
            layout.Children.Add(toggle, 0, 0);
            layout.Children.Add(text, 1, 0);

            return layout;
        }
    }
}


