using System;
using System.Collections.Generic;
using System.Linq;
namespace Mongoose
{
	public static class POICollectionsHelper
	{
		private delegate bool IntFunc(int n);
		private static List<Event> FilterByTimeRange(List<Event> events, DateTime startTime, double endTimeRange)
		{
			List<Event> result = new List<Event>();
			foreach(Event e in events)
			{
				DateTime dateTimeEventStart = e.GetBeginTime();
				TimeSpan timeUntilEvent = dateTimeEventStart.Subtract(startTime);
				if (timeUntilEvent.Days == 0 && timeUntilEvent.Hours <= endTimeRange
					|| (DateTime.Compare(dateTimeEventStart, startTime.AddHours(endTimeRange)) < 0) && (DateTime.Compare(dateTimeEventStart, startTime) > 0)
					|| (DateTime.Compare(dateTimeEventStart, startTime) < 0) && (DateTime.Compare(e.GetEndTime(), startTime) > 0))
				{
					result.Add(e);
				}
			}
			return result;
		}
		/// <summary>
		/// Gets the event title occurences.
		/// </summary>
		/// <returns>The event title occurences.</returns>
		/// <param name="events">Events.</param>
		private static Dictionary<string,int> GetEventTitleOccurences(List<Event> events)
		{
			Dictionary<string,int> result = new Dictionary<string,int>();
			foreach(Event e in events)
			{
				if(e.GetLengthInHours() < 24)
				{
					if(result.ContainsKey(e.title))
					{
						if(result[e.title] != 0)
						{
							result[e.title]++;
						}
					}
					else
					{
						result[e.title] = 1;
					}
				}
				else
				{
					result[e.title] = 0;
				}
			}
			return result;
		}
		/// <summary>
		/// Filters the onetime.
		/// </summary>
		/// <returns>The onetime.</returns>
		/// <param name="events">Events.</param>
		private static List<Event> FilterOnetime(List<Event> events)
		{
			List<Event> result = FilterByOccurence(events, (int n) => n == 1);
			return result;
		}
		/// <summary>
		/// Filters the ongoing.
		/// </summary>
		/// <returns>The ongoing.</returns>
		/// <param name="events">Events.</param>
		private static List<Event> FilterOngoing(List<Event> events)
		{
			List<Event> result = FilterByOccurence(events, (int n) => n != 1);
			return result;
		}
		/// <summary>
		/// Filters the by occurence.
		/// </summary>
		/// <returns>The by occurence.</returns>
		/// <param name="events">Events.</param>
		/// <param name="occurenceProcessor">Occurence processor.</param>
		private static List<Event> FilterByOccurence(List<Event> events, IntFunc occurenceProcessor)
		{
			List<Event> result = new List<Event>();
			Dictionary<string,int> eventOccurences = GetEventTitleOccurences(events);
			foreach(Event e in events)
			{
				Event duplicate = result.Find((Event ev) => ev.title == e.title);
				if(duplicate == null
				   && occurenceProcessor(eventOccurences[e.title]))
				{
					result.Add(e);
				}
			}
			return result;
		}
		/// <summary>
		/// Filters the events by time range.
		/// </summary>
		/// <returns>The events by the time range filter</returns>
		/// <param name="events">A list of events</param>
		/// <param name="startTime">Start time</param>
		/// <param name="endTimeRange">End time range</param>
		public static List<Event> GetOneTimeEvents(List<Event> events, DateTime startTime, double endTimeRange)
		{
			List<Event> result = FilterOnetime(events);
			result = FilterByTimeRange(result, startTime, endTimeRange);
			return result;
		}

		/// <summary>
		/// Filters the ongoing events.
		/// </summary>
		/// <param name="events">Events.</param>
		/// <param name="startTime">Start time.</param>
		/// <param name="endTimeRange">End time range.</param>
		public static List<Event> FilterOngoingEvents(List<Event> events, DateTime startTime, double endTimeRange)
		{
			List<Event> result = FilterOngoing(events);
			result = FilterByTimeRange(result, startTime, endTimeRange);
			return result;
		}
	}
}

