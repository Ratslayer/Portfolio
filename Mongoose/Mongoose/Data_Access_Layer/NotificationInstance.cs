using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Mongoose
{
	public class NotificationInstance
	{
		public List<Event> eventTemp;
		public HashSet<Event> hashTemp;
		private List<Event> eventsToNotify;
		private POICollections collections;
		public bool hasNew;

		public NotificationInstance ()
		{
			collections = POICollections.Instance();
			eventsToNotify = new List<Event>();
			hasNew = false;
		}

		public bool HasNew 
		{
			get 
			{
				return hasNew;
			}
			set
			{
				HasNew = value;
			}
		}

		public List<Event> CompareList()
		{
				return eventsToNotify;
		}

		public Collection<Event> DeepCopy(Collection<Event> CollectionA, Collection<Event> CollectionB)
		{
			foreach (Event e in CollectionA) 
			{
				CollectionB.Add (e);
			}
			return CollectionB;
		}
	}
}