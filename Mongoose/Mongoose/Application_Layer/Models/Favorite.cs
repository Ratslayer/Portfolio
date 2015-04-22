using System;

namespace Mongoose
{
	public class Favorite
	{
		Event favoriteEvent;
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <value>The event that was favorited.</value>
		public Event Event 
		{
			get 
			{
				return favoriteEvent; 
			}
			set 
			{
				favoriteEvent = value; 
			}
		}

		DateTime dateAdded;
		/// <summary>
		/// Gets the date added.
		/// </summary>
		/// <value>The date added.</value>
		public DateTime DateAdded 
		{
			get 
			{
				return dateAdded;
			}
		}


		public Favorite () : this(new Event()) 
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Mongoose.Favorite"/> class and set the favorite event
		/// </summary>
		/// <param name="_favoriteEvent">Favorite event.</param>
		public Favorite(Event _favoriteEvent)
		{
			favoriteEvent = _favoriteEvent;
            dateAdded = ParseEndDate(favoriteEvent);
		}

        /// <summary>
        /// Parse the endDate parameter into a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="_favoriteEvent">Favorite event.</param>
        DateTime ParseEndDate(Event _favoriteEvent)
        {
            string endDate = String.IsNullOrEmpty(_favoriteEvent.enddate) ? DateTime.Now.Date.ToString() : _favoriteEvent.enddate;
            return DateTime.Parse(endDate);
        }
	}
}

