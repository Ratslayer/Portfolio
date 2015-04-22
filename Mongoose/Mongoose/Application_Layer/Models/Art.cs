using System;
using System.Collections;

namespace Mongoose
{
	public class Art : POI
	{
		public string url { get; set; }
		public string room { get; set; }
		public Art()
			: base()
		{
		}

		public Art(string title, string description, string link, string position, Coordinates coords) : base(title, description, coords)
		{
			url = link;
			room = position;
		}

	}
}

