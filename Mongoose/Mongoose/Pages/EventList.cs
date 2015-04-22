using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
    public class EventList : LocationContentPage, IEventListViewListener
    {
        protected ToolbarItem filterLabel;

        protected delegate void AdapterFunc(EventListViewAdapter adapter,string building);

        ListView listView;
        List<Event> _events;
        Label defaultEventListLabel;
        ActivityIndicator loadingWheel;
        string _title = strings.Events;
        AdapterFunc _filter;

        string currentTab = "";
        Grid tabs;

        public EventList(string building)
            : base(building)
        {
            PushTabButton(strings.Onetime, PopulateOnetimeEvents, POICollections.Instance().EventsForCurrentLocation);
            SizeChanged += (sender, e) => GeneratePage();
        }
		/// <summary>
		/// Generates the page.
		/// </summary>
        public override void GeneratePage()
        {
            base.GeneratePage();

            tabs = CreateTabButtonGrid();

            filterLabel = new ToolbarItem()
            {
                Text = strings.Filters,
                Command = new Command(PushFilterPage)
            };
            ToolbarItems.Add(filterLabel);

            Title = _title;
            var viewAdapter = new EventListViewAdapter();
            viewAdapter.AddListener(this);
            _filter(viewAdapter, currentBuilding);

            if (defaultEventListLabel == null)
            {
                defaultEventListLabel = CreateEventListMessage(strings.NoLocationForEventList);
            }

            if (loadingWheel == null)
            {
                loadingWheel = new ActivityIndicator();
                loadingWheel.HorizontalOptions = LayoutOptions.CenterAndExpand; 
                loadingWheel.VerticalOptions = LayoutOptions.CenterAndExpand; 
                loadingWheel.Color = Theme.textAccentLightColor;
                loadingWheel.IsRunning = true;
            }

            Content = CreateStackLayout(loadingWheel);
        }

        public void OnEventListGenerated()
        {
            if (Content != null)
            {
                Content.BatchBegin();
                if (_events.Count == 0)
                {
                    defaultEventListLabel = CreateEventListMessage(strings.NoEvents);
                    Content = CreateStackLayout(defaultEventListLabel);
                }
                else
                {
                    listView = CreateEventListListView(_events);
                    Content = CreateStackLayout(listView);
                }
                Content.BatchCommit();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GeneratePage();
        }

        StackLayout CreateStackLayout(View view)
        {

            return new StackLayout()
            {
                Children =
                {
                    tabs,
                    view
                }
            };
        }

        Grid CreateTabButtonGrid()
        {
            var grid = new Grid()
            { 
                BackgroundColor = Theme.backgroundLightColor,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) }
                }
            };

            StackLayout onetimeStack = CreateTabStackLayout(strings.Onetime);
            var onetimeTap = new TapGestureRecognizer();
            onetimeTap.Tapped += (sender, e) =>
            {
                PushTabButton(strings.Onetime, PopulateOnetimeEvents, POICollections.Instance().EventsForCurrentLocation);
                GeneratePage();
            };
            onetimeStack.GestureRecognizers.Add(onetimeTap);

            StackLayout ongoingStack = CreateTabStackLayout(strings.Ongoing);
            var ongoingTap = new TapGestureRecognizer();
            ongoingTap.Tapped += (sender, e) =>
            {
                PushTabButton(strings.Ongoing, PopulateOngoingEvents, POICollections.Instance().OngoingEventsForCurrentLocation);
                GeneratePage();
            };
            ongoingStack.GestureRecognizers.Add(ongoingTap);

            grid.Children.Add(onetimeStack, 0, 0);
            grid.Children.Add(ongoingStack, 1, 0);

            if (IsPortrait(this))
            {
                grid.Padding = new Thickness(0,0,0,-6);
            }
            else
            {
                grid.Padding = new Thickness(0,0,0,22);
            }

            return grid;
        }

        StackLayout CreateTabStackLayout(string text)
        {
            return new StackLayout()
            {
                Children =
                {
                    new Frame()
                    {
                        Content = new Label() { Text = text, HorizontalOptions = LayoutOptions.CenterAndExpand },
                        Padding = new Thickness(0, 20, 0, 20)
                    },
                    CreateTabBoxView(text)
                }
            };
        }

        BoxView CreateTabBoxView(string text)
        {
            var box = new BoxView() { HeightRequest = 3 };
            if (currentTab.Equals(text))
            {
                box.BackgroundColor = Theme.backgroundDarkColor;
            }
            else
            {
                box.BackgroundColor = Theme.backgroundLightColor;
            }
            return box;
        }

        async void PushFilterPage()
        {
            BaseContentPage filters = new FiltersSettingPage();
            filters.Disappearing += (object sender, EventArgs e) => this.OnAppearing();
            await Globals.AddPage(filters);
        }

        void PushTabButton(string title, AdapterFunc filter, List<Event> events)
        {
            _filter = filter;
            _events = events;
            currentTab = title;
        }

        static void PopulateOnetimeEvents(EventListViewAdapter adapter, string building)
        {
            adapter.PopulateOnetimeEventList(building);
        }
        static void PopulateOngoingEvents(EventListViewAdapter adapter, string building)
        {
            adapter.PopulateOngoingEventList(building);
        }

        ListView CreateEventListListView(List<Event> events)
        {
            ListView lv = new ListView();
            lv.ItemTapped += async (sender, e) =>
                {
                    await Globals.AddPage(new EventPage((Event)e.Item));
                    ((ListView)sender).SelectedItem = null; // Deselect the row
                };

            lv.ItemTemplate = new DataTemplate(typeof(EventCell));
            lv.HasUnevenRows = true;
            lv.ItemsSource = events;

            return lv;
        }

        Label  CreateEventListMessage(string text)
        {
            Label label = Theme.CreateLabel(text);
            label.VerticalOptions = LayoutOptions.CenterAndExpand;
            label.HorizontalOptions = LayoutOptions.CenterAndExpand;
            label.XAlign = TextAlignment.Center;
            label.YAlign = TextAlignment.Center;
            return label;
        }

    }
}