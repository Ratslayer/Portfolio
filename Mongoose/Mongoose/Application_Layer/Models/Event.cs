using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Mongoose
{
	public class Event : POI
	{
		public string startdate { get; set; }
		public string starttime { get; set; }
		public string enddate { get; set; }
		public string endtime { get; set; }
		public string allday { get; set; }
		public string venue { get; set; }
		public string room { get; set; }
		public string offlocation { get; set; }
		public string offcivic { get; set; }
		public string offlink { get; set; }
		public List<string> category { get; set; }
		public List<string> audiences { get; set; }
		public List<string> units { get; set; }
		public string url { get; set; }
		public ImageSource image { get; set; }
		private string formattedDate { get; set; }

		public string FormattedDate
		{
			get
			{
				return FormatDate();
			}
			set
			{
				formattedDate = value;
			}
		}

		public Event()
			: base()
		{
			category = new List<string>();
			audiences = new List<string>();
			units = new List<string>();
			image = ImageSource.FromResource("Mongoose.new_tag.png");
		}

		/// <summary>
		/// Equalses the object could have overriden the EqualsMethod instead.
		/// </summary>
		/// <returns><c>true</c>, if object was equalsed, <c>false</c> otherwise.</returns>
		/// <param name="event1">Event1.</param>
		public bool EqualsObject(Event event1)
		{
			if (this.title.Equals (event1.title) && this.description.Equals (event1.description) && this.startdate.Equals (event1.startdate) && this.enddate.Equals (event1.enddate))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public double GetLengthInHours()
		{
			DateTime start = GetBeginTime();
			DateTime end = GetEndTime();
			double result = end.Subtract(start).TotalHours;
			return result;
		}
		public DateTime GetBeginTime()
		{
			return GetTime(startdate, starttime);
		}
		public DateTime GetEndTime()
		{
			return GetTime(enddate, endtime);
		}
		private DateTime GetTime(string date, string time)
		{
			string strTime = date + " " + time;
			DateTime dateTime = DateTime.Parse(strTime);
			return dateTime;
		}
		private string FormatDate()
		{
			DateTime eventDate = DateTime.Parse(startdate);
			string eventDay = eventDate.Day.ToString();
			string eventMonth = eventDate.Month.ToString();
			string eventYear = eventDate.Year.ToString();
			string eventDateFormatted = eventDay + "/" + eventMonth + "/" + eventYear;

			string day = DateTime.Now.Day.ToString();
			string month = DateTime.Now.Month.ToString();
			string year = DateTime.Now.Year.ToString();
			string todayFormatted = day + "/" + month + "/" + year;

			if (eventDateFormatted.Equals(todayFormatted))
			{
                return strings.TodayAt + starttime;
			}
			else
			{
                return eventDate.ToString("dddd, MMMM d") + strings.At + starttime;
			}
		}
	}
}

