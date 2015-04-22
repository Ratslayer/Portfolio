using System;

namespace Mongoose
{
	public interface ILocation
	{
		/// <summary>
		/// Gets the coordinates of the location.
		/// </summary>
		/// <value>The coords.</value>
		Coordinates Coords
		{
			get;
		}
		/// <summary>
		/// Gets the address associated with the Latitude/Longitude. 
		/// Take note that Address computation is separate from coordinates computation and quite costly, which means that this function is very likely to use async functionality.
		/// </summary>
		/// <value>The address.</value>
		string Address 
		{
			get;
		}
		/// <summary>
		/// Gets the building associated with the address
		/// </summary>
		/// <value>The building.</value>
		string Building
		{
			get;
		}
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		/// <filterpriority>2</filterpriority>
		string ToString();
	}
}