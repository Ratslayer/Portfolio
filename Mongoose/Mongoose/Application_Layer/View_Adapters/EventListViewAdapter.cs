using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Mongoose
{

	public class EventListViewAdapter
	{
		private EventRESTAPIConnector restConnector;
		private POICollections collections;
		private List<IEventListViewListener> listeners;

		public POICollections Collections
		{
			get
			{
				return collections;
			}
		}

		public EventRESTAPIConnector EventRestAPIConnector
		{
			get
			{
				return restConnector;
			}
		}

		public EventListViewAdapter ()
		{
			listeners = new List<IEventListViewListener>();
			restConnector = new EventRESTAPIConnector();
			collections = POICollections.Instance();
		}
		public void AddListener(IEventListViewListener listener)
		{
			listeners.Add(listener);
		}
		/// <summary>
		/// Parses JSON data and converts it into Event objects
		/// </summary>
		/// <returns>The into object.</returns>
		/// <param name="jsonEvents">Json events.</param>
		public List<Event> ParseIntoObject(string jsonEvents)
		{
			List<Event> events = new List<Event>();
			try
			{
				events.AddRange(JsonConvert.DeserializeObject<List<Event>>(jsonEvents));
			}
			catch (JsonReaderException jre)
			{
				System.Diagnostics.Debug.WriteLine("JsonReaderException: " + jre.Message);
				return events;
			}
			return events;
		}

		/// <summary>
		/// Compare the list of current events shown to the list of notified events
		/// If the current events list has new events, it will populate the notified events list and also 
		/// populate a tonotify collection which will store the new events to notify to the user
		/// </summary>
		/// <param name="events">current event lists</param>
		public void PopulateNotifyEvents(List<Event> events)
		{
			bool exists = false;
			if (collections.notifiedEvents.Count != 0) 
			{
				foreach (Event e in events) 
				{
					foreach (Event e2 in collections.notifiedEvents)
					{
						if (e.EqualsObject (e2)) 
						{
							e.image = "";
							exists = true;
						}
					}
					if (!exists) 
					{
						e.image = ImageSource.FromResource("Mongoose.new_tag.png");
						collections.toNotify.Add(e);
					}
					exists = false;
				}
				//adding To notify events to notified events
				foreach (Event e3 in collections.toNotify)
				{
					collections.notifiedEvents.Add (e3);
				}
			} 
			else 
			{
				collections.notifiedEvents.AddRange(events);
				collections.toNotify.AddRange(events);
			}
		}
		private delegate List<Event> FilterDelegate(List<Event> events, DateTime time, double timeFilter);	
		/// <summary>
		/// Populates the event list based on data returned from the REST API
		/// </summary>
		/// <returns>The event list from RESTAP.</returns>
		public async Task PopulateOnetimeEventList(string building)
		{
			await PopulateEventListFromRESTAPI(collections.EventsForCurrentLocation, building, POICollectionsHelper.GetOneTimeEvents);

			await Xamarin.Forms.DependencyService.Get<IFileAccessor>().SerializeAndSaveObjects(collections.EventsForCurrentLocation, "events.json");
		}
		/// <summary>
		/// Populates the ongoing event list.
		/// </summary>
		/// <returns>The ongoing event list.</returns>
		/// <param name="building">Building.</param>
		public async Task PopulateOngoingEventList(string building)
		{
			await PopulateEventListFromRESTAPI(collections.OngoingEventsForCurrentLocation, building, POICollectionsHelper.FilterOngoingEvents);
		}
		/// <summary>
		/// Populates the event list from RESTAP.
		/// </summary>
		/// <returns>The event list from RESTAP.</returns>
		/// <param name="eventCollection">Event collection.</param>
		/// <param name="building">Building.</param>
		/// <param name="filterFunc">Filter func.</param>
		private async Task PopulateEventListFromRESTAPI(List<Event> eventCollection, string building, FilterDelegate filterFunc)
		{
			System.Diagnostics.Debug.WriteLine("From RESTAPI");
			double timeFilter = Settings.TimeRangeFilter;
			List<Event> events = await RetrieveEvents(building);
			eventCollection.Clear();
			events = filterFunc(events,DateTime.Now,timeFilter);
			eventCollection.AddRange(events);
			PopulateNotifyEvents(events);

			foreach(IEventListViewListener listener in listeners)
			{
				listener.OnEventListGenerated();
			}
		}
		/// <summary>
		/// Populates event list based on stored data (in case of crash, can load data straight from device instead of querying REST)
		/// </summary>
		/// <returns>The event list from file system.</returns>
		public async Task PopulateEventListFromFileSystem()
		{
			List<Event> eventList;
			string json = await Xamarin.Forms.DependencyService.Get<IFileAccessor>().LoadSerializedObjects("events.json");
			JArray jsonEvents = JArray.Parse(json);
			eventList = (List<Event>)jsonEvents.ToObject(typeof(List<Event>));
			eventList = ParseIntoObject(json);
			collections.EventsForCurrentLocation.Clear();
			collections.EventsForCurrentLocation.AddRange(eventList);
			foreach(IEventListViewListener listener in listeners)
			{
				listener.OnEventListGenerated();
			}
		}
		/// <summary>
		/// Popuplates selected location event list based on selected location
		/// </summary>
		/// <returns>The event list for selected location.</returns>
		/// <param name="building">Building.</param>
		public async Task PopulateEventListForSelectedLocation(string building)
		{
			double timeFilter = Settings.TimeRangeFilter;
			List<Event> events = await RetrieveEvents(building);
			collections.EventsForSelectedLocation.Clear();
			POICollectionsHelper.GetOneTimeEvents(events,DateTime.Now,timeFilter);
			collections.EventsForSelectedLocation = events;

			foreach(IEventListViewListener listener in listeners)
			{
				listener.OnEventListGenerated();
			}
		}
		/// <summary>
		/// Retrieves the events.
		/// </summary>
		/// <returns>The events.</returns>
		/// <param name="building">Building.</param>
		private async Task<List<Event>> RetrieveEvents(string building)
		{
			string urlRequest = restConnector.CreateURL(building);
			Task<string> responseTask = restConnector.CallRESTAPI(urlRequest);
			List<Event> events = ParseIntoObject(await responseTask);
			return events;
		}
	}
}

