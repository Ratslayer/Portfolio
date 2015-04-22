using System;

namespace Mongoose
{
	/// <summary>
	/// This is the class that corresponds to a geolocation on earth.
	/// It operates as a vector, containing many vector functions and overloads most operators.
	/// However, the X and Y components are swapped in the constructor, due to the fact that the international system
	/// writes the latitude before longitude, i.e. North before West.
	/// </summary>
	public class Coordinates
	{
		/// <summary>
		/// Delegate that is an operation on 2 doubles.
		/// It is used to reduce duplication of code, since most vector operations execute same transform for every single dimension.
		/// </summary>
		private delegate double DoubleFunc(double d1, double d2);
		/// <summary>
		/// The latitude (North-South).
		/// </summary>
		public double latitude;
		/// <summary>
		/// The longitude (East-West).
		/// </summary>
		public double longitude;
		/// <summary>
		/// The altitude.
		/// </summary>
		public double altitude;
		//overlaoded operators
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public static Coordinates operator -(Coordinates left, Coordinates right)
		{
			return CoordTransform(left, right, Minus);
		}
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public static Coordinates operator +(Coordinates left, Coordinates right)
		{
			return CoordTransform(left, right, Plus);
		}
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public static Coordinates operator *(Coordinates left, Coordinates right)
		{
			return CoordTransform(left, right, Mult);
		}
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public static Coordinates operator /(Coordinates left, Coordinates right)
		{
			return CoordTransform(left, right, Div);
		}
		/// <param name="coordinates">Coordinates.</param>
		/// <param name="scalar">Scalar.</param>
		public static Coordinates operator *(Coordinates coordinates, double scalar)
		{
			return ScalarTransform(coordinates, scalar, Mult);
		}
		/// <param name="coordinates">Coordinates.</param>
		/// <param name="scalar">Scalar.</param>
		public static Coordinates operator /(Coordinates coordinates, double scalar)
		{
			return ScalarTransform(coordinates, scalar, Div);
		}
		/// <summary>
		/// Helper function that performs a scalar DoubleFunc operation on every Coordinate component.
		/// </summary>
		/// <returns>The transformed coordinate.</returns>
		/// <param name="c">Coordinate to be transformed.</param>
		/// <param name="scalar">Scalar value.</param>
		/// <param name="func">Operation delegate to be performed.</param>
		private static Coordinates ScalarTransform(Coordinates c, double scalar, DoubleFunc func)
		{
			double lat = func(c.latitude, scalar);
			double lon = func(c.longitude, scalar);
			double alt = func(c.altitude, scalar);
			return new Coordinates(lat, lon, alt);
		}
		/// <summary>
		/// Helper function that performs a DoubleFunc operation on every component using both coordinates objects.
		/// </summary>
		/// <returns>The result Coordinates object.</returns>
		/// <param name="c1">Coordinate 1.</param>
		/// <param name="c2">Coordinate 2.</param>
		/// <param name="func">Operation delegate to be performed.</param>
		private static Coordinates CoordTransform(Coordinates c1, Coordinates c2, DoubleFunc func)
		{
			double lat = func(c1.latitude, c2.latitude);
			double lon = func(c1.longitude, c2.longitude);
			double alt = func(c1.altitude, c2.altitude);
			return new Coordinates(lat, lon, alt);
		}
		//functions satisfying DoubleFunc delegates signature
		/// <summary>
		/// Function used for the -(Coordinates, Coordinates) operator.
		/// </summary>
		/// <param name="d1">Left component.</param>
		/// <param name="d2">Right component.</param>
		private static double Minus(double d1, double d2)
		{
			return d1 - d2;
		}
		/// <summary>
		/// Function used for the +(Coordinates, Coordinates) operator.
		/// </summary>
		/// <param name="d1">Left component.</param>
		/// <param name="d2">Right component.</param>
		private static double Plus(double d1, double d2)
		{
			return d1 + d2;
		}
		/// <summary>
		///Function used for the *(Coordinates, Coordinates) and *(Coordinates, double) operators.
		/// </summary>
		/// <param name="d1">Left component.</param>
		/// <param name="d2">Right component.</param>
		private static double Mult(double d1, double d2)
		{
			return d1 * d2;
		}
		/// <summary>
		/// Function used for the /(Coordinates, Coordinates) and /(Coordinates, double) operators.
		/// </summary>
		/// <param name="d1">Left component.</param>
		/// <param name="d2">Right component.</param>
		private static double Div(double d1, double d2)
		{
			return d1 / d2;
		}
		/// <summary>
		/// Returns by-component-minimum between 2 coordinates.
		/// </summary>
		/// <param name="c1">Coordinates 1.</param>
		/// <param name="c2">Coordinates 2.</param>
		public static Coordinates Min(Coordinates c1, Coordinates c2)
		{
			//get the minimum using Math.Min as a DoubleFunc delegate
			Coordinates result = CoordTransform(c1, c2, Math.Min);
			return result;
		}
		/// <summary>
		/// Returns by-component-maximum between 2 coordinates.
		/// </summary>
		/// <param name="c1">Coordinates 1.</param>
		/// <param name="c2">Coordinates 2.</param>
		public static Coordinates Max(Coordinates c1, Coordinates c2)
		{
			//get the maximum using Math.Max as a DoubleFunc delegate
			Coordinates result = CoordTransform(c1, c2, Math.Max);
			return result;
		}
		/// <summary>
		/// Clamp c between min and max.
		/// </summary>
		/// <param name="c">Coordinates to be clamped.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static Coordinates Clamp(Coordinates c, Coordinates min, Coordinates max)
		{
			Coordinates result = Min(Max(c, min), max);
			return result;
		}
		/// <summary>
		/// Parametrised constructor. Note that latitude and longitude are inversed, due to the fact that the international system writes north coords before west coords.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		/// <param name="altitude">Altitude.</param>
		public Coordinates(double latitude = 0, double longitude = 0, double altitude = 0)
		{
			this.latitude = latitude;
			this.longitude = longitude;
			this.altitude = altitude;
		}
		/// <summary>
		/// Gets the euclidian length of the coordinates vector.
		/// </summary>
		/// <value>The length.</value>
		public double Length
		{
			get
			{
				double sqrResult = latitude * latitude + longitude * longitude + altitude * altitude;
				double result = Math.Sqrt(sqrResult);
				return result;
			}
		}
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			string result = latitude.ToString() + " " + longitude.ToString() + " " + altitude.ToString();
			return result;
		}
		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <returns>true if the coordinate components are equal to each other.</returns>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Mongoose.Coordinates"/>.</param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			bool result = false;
			Coordinates other = (Coordinates)obj;
			if(other != null)
			{
				result = latitude == other.latitude
				&& longitude == other.longitude
				&& altitude == other.altitude;
			}
			return result;
		}
	}
}

