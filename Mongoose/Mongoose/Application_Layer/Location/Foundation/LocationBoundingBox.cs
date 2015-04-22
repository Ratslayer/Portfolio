using System;

namespace Mongoose
{
	/// <summary>
	/// This is the class that represents an Axis-Aligned Bounding Box in the geo-location coordinates system.
	/// </summary>
	public class LocationBoundingBox
	{
		/// <summary>
		/// Gets the bottom left corner of the box.
		/// </summary>
		/// <value>The minimum.</value>
		public Coordinates Min
		{
			get
			{
				return _min;
			}
		}
		/// <summary>
		/// Gets the top right corner of the box.
		/// </summary>
		/// <value>The max.</value>
		public Coordinates Max
		{
			get
			{
				return _max;
			}
		}
		/// <summary>
		/// Bottom left corner.
		/// </summary>
		private readonly Coordinates _min;
		/// <summary>
		/// Top right corner.
		/// </summary>
		private readonly Coordinates _max;
		/// <summary>
		/// The center of the bounding box.
		/// </summary>
		private readonly Coordinates _center;
		/// <summary>
		/// The dimensions of the bounding box.
		/// </summary>
		private readonly Coordinates _dimensions;
		/// <summary>
		/// Parametrised contructor.
		/// </summary>
		/// <param name="min">Bottom left.</param>
		/// <param name="max">Top right.</param>
		public LocationBoundingBox(Coordinates min, Coordinates max)
		{
			_min = min;
			_max = max;
			_center = (_min + _max) * 0.5;
			_dimensions = _max - _min;
		}
		/// scaling operator for the box
		/// <param name="box">Box.</param>
		/// <param name="scalar">Scalar.</param>
		public static LocationBoundingBox operator *(LocationBoundingBox box, double scalar)
		{
			LocationBoundingBox result = new LocationBoundingBox(box.Min * scalar, box.Max * scalar);
			return result;
		}
		/// <summary>
		/// Create a bounding box that encloses all the points in the array.
		/// </summary>
		/// <returns>The points.</returns>
		/// <param name="points">Points.</param>
		public static LocationBoundingBox FromPoints(Coordinates[] points)
		{
			LocationBoundingBox result;
			if(points.Length > 0)
			{
				//store starting min/max values
				Coordinates min = points[0], max = points[0];
				//for each point in points, update the min and max with smallest min and max of the point
				foreach(Coordinates point in points)
				{
					min = Coordinates.Min(min, point);
					max = Coordinates.Max(max, point);
				}
				result = new LocationBoundingBox(min, max);
			}
			//if the array is empty, create a zero bounding box
			else
			{
				result = new LocationBoundingBox(new Coordinates(), new Coordinates());
			}
			return result;
		}
		/// <summary>
		/// Merges the bounding boxes from the array into 1 bounding box that encloses all of them.
		/// </summary>
		/// <returns>The bounding box enclosing the boxes.</returns>
		/// <param name="boxes">Bounding boxes array.</param>
		public static LocationBoundingBox FromBoundingBoxes(LocationBoundingBox[] boxes)
		{
			LocationBoundingBox result;
			if(boxes.Length > 0)
			{
				//store starting min/max values
				Coordinates min = boxes[0].Min, max = boxes[0].Max;
				//for each box in boxes, update the min and max with smallest min and biggest max of the boxes
				foreach(LocationBoundingBox box in boxes)
				{
					min = Coordinates.Min(min, box.Min);
					max = Coordinates.Max(max, box.Max);
				}
				result = new LocationBoundingBox(min, max);
			}
			//if empty array, create a zero bounding box
			else
			{
				result = new LocationBoundingBox(new Coordinates(), new Coordinates());
			}
			return result;
		}
		/// <summary>
		/// Determines if value is between the specified value min and max.
		/// </summary>
		/// <returns><c>true</c> if value is between the specified value min max; otherwise, <c>false</c>.</returns>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		private static bool IsBetween(double value, double min, double max)
		{
			return value >= min && value <= max;
		}
		/// <summary>
		/// Returns whether this box ontains the specified point.
		/// Does not take altitude into account.
		/// </summary>
		/// <param name="point">Point.</param>
		public bool Contains(Coordinates point)
		{
			bool result = IsBetween(point.latitude, _min.latitude, _max.latitude)
			              && IsBetween(point.longitude, _min.longitude, _max.longitude);
			return result;
		}
		/// <summary>
		/// helper function that determines whether the boxes intersect ina  given plane.
		/// </summary>
		/// <param name="distance">Distance between their centers on the plane.</param>
		/// <param name="size1">Half size of the bounding box 1's projection unto that plane.</param>
		/// <param name="size2">Half size of the bounding box 2's projection unto that plane.</param>
		private static bool Intersects(double distance, double halfSize1, double halfSize2)
		{
			bool result = Math.Abs(distance) <= (halfSize1 + halfSize2) * 0.5;
			return result;
		}
		/// <summary>
		/// Determines whether this box intersects given box.
		/// </summary>
		/// <param name="box">Other box.</param>
		public bool Intersects(LocationBoundingBox box)
		{
			Coordinates distance = box._center - _center;
			//if they intersect on both x and y planes, then they intersect.
			bool result = Intersects(distance.latitude, _dimensions.latitude, box._dimensions.latitude)
			              && Intersects(distance.longitude, _dimensions.longitude, box._dimensions.longitude);
			return result;
		}
		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <returns>true if the mins and maxes match in both objects; otherwise, false.</returns>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Mongoose.LocationBoundingBox"/>.</param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			bool result = false;
			if(obj is LocationBoundingBox)
			{
				LocationBoundingBox box = (LocationBoundingBox)obj;
				result = box.Min.Equals(_min)
					&& box.Max.Equals(_max);
			}
			return result;
		}
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("[LocationBoundingBox: Min={0}, Max={1}]", Min, Max);
		}
	}
}

