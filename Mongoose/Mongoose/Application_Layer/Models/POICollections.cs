using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mongoose
{
	public class POICollections
	{
		private static POICollections instance = new POICollections();
		public List<Event> notifiedEvents;
		private List<Event> eventsForCurrentLocation;
		private List<Event> eventsForSelectedLocation;
		private List<Event> ongoingEventsForCurrentLocation;
		public List<Favorite> eventFavorites;
		public List<Art> artForCurrentLocation;
		public List<Event> toNotify;

		public List<Event> EventsForCurrentLocation
		{
			get	{ return eventsForCurrentLocation;	}
			set
			{
				this.eventsForCurrentLocation.AddRange(value);
			}
		}

		public List<Event> EventsForSelectedLocation
		{
			get	{ return eventsForSelectedLocation;	}
			set
			{
				this.eventsForSelectedLocation.AddRange(value);
			}
		}

		public List<Event> OngoingEventsForCurrentLocation
		{
			get { return ongoingEventsForCurrentLocation; }
			set
			{
				this.ongoingEventsForCurrentLocation.AddRange(value);
			}
		}

		/// <summary>
		///	Private constructor for singleton implementation class.
		/// </summary>
		private POICollections()
		{
			notifiedEvents = new List<Event>();
			eventsForCurrentLocation = new List<Event>();
			ongoingEventsForCurrentLocation = new List<Event>();
			eventsForSelectedLocation = new List<Event>();
			eventFavorites = new List<Favorite>();
			artForCurrentLocation = new List<Art>();
			toNotify = new List<Event>();
		}
		/// <summary>
		/// return this instance.
		/// </summary>
		public static POICollections Instance()
		{
			return instance;
		}

        /// <summary>
        /// Warpper to added favorites to the favorite collection
        /// </summary>
        /// <param name="favorite">favorite to be added</param>
        public void AddToFavorites(Favorite favorite)
        {
            eventFavorites.Add(favorite);
        }

        /// <summary>
        /// Warpper to remove favorites from the favorite collection
        /// </summary>
        /// <param name="favorite">favorite to be added</param>
        public void RemoveFromFavorites(Favorite favorite)
        {
            eventFavorites.Remove(favorite);
        }

        /// <summary>
        /// Warpper to clear all favorites from the favorite collection
        /// </summary>
        public void ClearAllFavorites()
        {
            lock (instance.eventFavorites)
            {
                eventFavorites.Clear();
            }
        }

		/// <summary>
		/// Check the date added if it exceeds the date to clear, it will remove the favorites
		/// </summary>
		/// <param name="cutOffDate">date to be removed</param>
		public void ClearOldFavorites (DateTime cutOffDate)
		{
			lock (instance.eventFavorites) {
				int favoriteCount = eventFavorites.Count;
				for (int i = 0; i < favoriteCount; i++) {
					if (eventFavorites [i].DateAdded.CompareTo (cutOffDate) < 0) {
						eventFavorites.RemoveAt (i);
						favoriteCount--;
					}
				}
			}
		}
	}
}