using System;

namespace Mongoose
{
	public class LocationTriangle
	{
		/// <summary>
		/// The 3 points composing a triangle
		/// </summary>
		private Coordinates _c1, _c2, _c3;
		/// <summary>
		/// Parametrised constructor.
		/// </summary>
		/// <param name="c1">C1.</param>
		/// <param name="c2">C2.</param>
		/// <param name="c3">C3.</param>
		public LocationTriangle(Coordinates c1, Coordinates c2, Coordinates c3)
		{
			_c1 = c1;
			_c2 = c2;
			_c3 = c3;
		}
		//code used as inspiration http://stackoverflow.com/questions/2049582/how-to-determine-a-point-in-a-triangle
		//helper function that is used by the Contains function to determine whether a given point is inside a triangle
		private double Sign2D(Coordinates c1, Coordinates c2, Coordinates c3)
		{
			double result = (c1.latitude - c3.latitude) * (c2.longitude - c3.longitude) - (c2.latitude - c3.latitude) * (c1.longitude - c3.longitude);
			return result;
		}
		/// <summary>
		/// Returns true if triangle contains given point.
		/// </summary>
		/// <param name="p">Point.</param>
		public bool Contains(Coordinates p)
		{
			bool b1, b2, b3;
			b1 = IsNegativeSign2D(p, _c1, _c2);
			b2 = IsNegativeSign2D(p, _c2, _c3);
			b3 = IsNegativeSign2D(p, _c3, _c1);

			return ((b1 == b2) && (b2 == b3));
		}
		private bool IsNegativeSign2D(Coordinates c1, Coordinates c2, Coordinates c3)
		{
			return Sign2D(c1, c2, c3) <= 0.0;
		}
		/// <summary>
		/// Gets the 3 points that compose the triangle.
		/// </summary>
		/// <returns>The points.</returns>
		public Coordinates[] GetPoints()
		{
			Coordinates[] points = new Coordinates[3];
			points[0] = _c1;
			points[1] = _c2;
			points[2] = _c3;
			return points;
		}
	}
}

