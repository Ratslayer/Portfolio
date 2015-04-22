using System;
using System.Collections.Generic;
namespace Mongoose
{
	public class ArtCollection
	{
		/// <summary>
		/// Accessor for the Arts collection.
		/// </summary>
		/// <value>The arts.</value>
		public List<Art> Arts
		{
			get
			{
				return _arts;
			}
		}
		private List<Art> _arts;
		/// <summary>
		/// Initializes a new instance of the <see cref="Mongoose.ArtCollection"/> class and add the three static arts
		/// </summary>
		public ArtCollection()
		{
			_arts = new List<Art>();
			_arts.Add(new Art(strings.ArtDisplayTitle, strings.ArtDisplayInformation, "http://library.concordia.ca/about/news/#cat=exhibitions", "LB-2", new Coordinates(45.49664111,-73.57788719, 0)));
			_arts.Add(new Art(strings.BookDisplayTitle, strings.BookDisplayInformation, "http://library.concordia.ca/about/news/acquisitions/", "LB-2", new Coordinates(45.49682912, -73.57824795, 0)));
			_arts.Add(new Art(strings.CourseReservesRoomTitle, strings.CourseReservesRoomInformation, "http://clues.concordia.ca/search/r", "LB-2", new Coordinates(45.49680374, -73.57820101, 0)));
		}
		/// <summary>
		/// Returns an Art object whose title matches the key.
		/// </summary>
		/// <param name="title">Title.</param>
		public Art this[string title]
		{
			get
			{
				Art result = null;
				foreach(Art art in _arts)
				{
					if(art.title == title)
					{
						result = art;
						break;
					}
				}
				return result;
			}
		}
	}
}

