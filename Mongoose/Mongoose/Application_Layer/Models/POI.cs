using System;
using Newtonsoft.Json;

/* Point of interest class  
   
 */
namespace Mongoose
{
	public class POI
	{
		public string title { get; set; }
		public string description { get; set; }
		public string campus { get; set; }
		public string building { get; set; }
		public Coordinates coords{ get; set; }

		public POI()
		{
		}

		public POI(string names, string details, Coordinates coords)
		{
			title = names;
			description = details;
			this.coords = coords;
		}
	}
}

