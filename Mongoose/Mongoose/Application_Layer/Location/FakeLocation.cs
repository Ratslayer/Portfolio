using System;

namespace Mongoose
{
	/// <summary>
	/// An implementation of ILocation interface that is used for testing.
	/// Returns zero coordinates and defaults to building H.
	/// </summary>
	public class FakeLocation : ILocation
	{
		/// <summary>
		/// Returns the point of origin as current location.
		/// </summary>
		/// <value>The coords.</value>
		public Coordinates Coords
		{
			get
			{
				return new Coordinates(0, 0, 0);
			}
		}
		/// <summary>
		/// Returns no address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get
			{
				return strings.NoAddress;
			}
		}
		/// <summary>
		/// Gets the building associated with the address. In this case, defaults to H.
		/// </summary>
		/// <value>The building.</value>
		public string Building
		{
			get
			{
				return "H";
			}
		}
	}
}

