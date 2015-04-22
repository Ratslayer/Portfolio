using System;
using System.Collections.Generic;
namespace Mongoose
{
	public class LocationMesh
	{
		/// <summary>
		/// Triangles that compose the mesh.
		/// </summary>
		private List<LocationTriangle> _triangles;
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LocationMesh()
		{
			_triangles = new List<LocationTriangle>();
		}
		/// <summary>
		/// Adds the triangle to the mesh
		/// </summary>
		/// <param name="triangle">Triangle.</param>
		public void AddTriangle(LocationTriangle triangle)
		{
			_triangles.Add(triangle);
		}
		/// <summary>
		/// Returns true if any triangle in the mesh contains given point.
		/// </summary>
		/// <param name="coordinates">Point.</param>
		public bool Contains(Coordinates coordinates)
		{
			bool result = false;
			//if any triangle contains the coordinates, store the result and break.
			foreach(LocationTriangle triangle in _triangles)
			{
				if(triangle.Contains(coordinates))
				{
					result = true;
					break;
				}
			}
			return result;
		}
		/// <summary>
		/// Get the triangles that compose the mesh in an array.
		/// </summary>
		/// <returns>The triangles.</returns>
		public LocationTriangle[] GetTriangles()
		{
			return _triangles.ToArray();
		}
		/// <summary>
		/// Gets the bounding box that encloses the mesh.
		/// </summary>
		/// <returns>The bounding box.</returns>
		public LocationBoundingBox GetBoundingBox()
		{
			//fetch all the points from the triangles
			Coordinates[] points = GetCoords();
			//compute bounding box
			LocationBoundingBox result = LocationBoundingBox.FromPoints(points);
			return result;
		}
		/// <summary>
		/// Gets the center of the mesh.
		/// </summary>
		/// <returns>The center.</returns>
		public Coordinates GetCenter()
		{
			Coordinates[] points = GetCoords();
			//get the center of mass
			Coordinates result = new Coordinates(0, 0, 0);
			foreach(Coordinates point in points)
			{
				result += point;
			}
			//normalize the center
			result /= points.Length;
			return result;
		}
		/// <summary>
		/// Get all points constituting the mesh.
		/// </summary>
		/// <returns>The coords.</returns>
		private Coordinates[] GetCoords()
		{
			List<Coordinates> coordsList = new List<Coordinates>();
			//for every point in a triangle, add the point to the list
			foreach(LocationTriangle triangle in _triangles)
			{
				Coordinates[] triangleCoords = triangle.GetPoints();
				foreach(Coordinates coords in triangleCoords)
				{
					coordsList.Add(coords);
				}
			}
			return coordsList.ToArray();
		}
	}
}

