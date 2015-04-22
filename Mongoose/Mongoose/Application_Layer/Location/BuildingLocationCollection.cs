using System;
using System.Collections;
using System.Collections.Generic;
namespace Mongoose
{
	/// <summary>
	/// This class is a collection of LocationMesh objects, each corresponding to a specific building.
	/// The values for the mesh points are fetched from google maps and hard-coded into the constructor.
	/// Eventually they might be ported to a text file, but for now this will suffice.
	/// </summary>
	public class BuildingLocationCollection
	{
		/// <summary>
		/// Buildings-mesh dictionary that uses building names as keys and TriangleMesh covering the building area as values.
		/// </summary>
		private Dictionary<string, LocationMesh> _buildings;
		/// <summary>
		/// Default constructor. Initialised the buildings collection and populates it with data.
		/// </summary>
		public BuildingLocationCollection()
		{
			_buildings = new Dictionary<string, LocationMesh>();
			//H building mesh
			AddTriangle("H", 45.497688, -73.577801, 45.498690, -73.579598, 45.497177, -73.581079);
			AddTriangle("H", 45.497688, -73.577801, 45.496705, -73.578745, 45.497177, -73.581079);
			AddTriangle("H", 45.496282, -73.578965, 45.496705, -73.578745, 45.497177, -73.581079);
			//LB building mesh
			AddTriangle("LB", 45.496645, -73.578777, 45.497722, -73.577768, 45.496897, -73.576094);
			AddTriangle("LB", 45.496645, -73.578777, 45.495835, -73.577129, 45.496897, -73.576094);
			//EV building mesh
			//1550 De Maisonneuve W.
			AddTriangle("EV", 45.495720, -73.579280, 45.496227, -73.578990, 45.495087, -73.577880);
			AddTriangle("EV", 45.495485, -73.577478, 45.496227, -73.578990, 45.495087, -73.577880);
			//1428 Mackay
			AddTriangle("EV", 45.495791, -73.578124, 45.495485, -73.577478, 45.496157, -73.577773);
			AddTriangle("EV", 45.495838, -73.577119, 45.495485, -73.577478, 45.496157, -73.577773);
			//MB building mesh
			AddTriangle("MB", 45.495645, -73.579339, 45.495020, -73.577946, 45.494572, -73.580510);
			AddTriangle("MB", 45.493868, -73.579292, 45.495020, -73.577946, 45.494572, -73.580510);
		}
		/// <summary>
		/// Adds the triangle to building mesh. If no mesh is present, creates one.
		/// </summary>
		/// <param name="building">Building name.</param>
		protected void AddTriangle(string building, double lat1, double long1, double lat2, double long2, double lat3, double long3)
		{
			//if this is a new building, then first create a mesh for it
			if(!_buildings.ContainsKey(building))
			{
				_buildings.Add(building, new LocationMesh());
			}
			//convert data to triangle, with altitude = 0 and add triangle to the mesh
			Coordinates p1, p2, p3;
			p1 = new Coordinates(lat1, long1, 0);
			p2 = new Coordinates(lat2, long2, 0);
			p3 = new Coordinates(lat3, long3, 0);
			_buildings[building].AddTriangle(new LocationTriangle(p1, p2, p3));
		}
		/// <summary>
		/// Gets the building, whose mesh contains the point. If none contain, will return "Unknown".
		/// </summary>
		/// <returns>The name of the building that contains the point</returns>
		/// <param name="coords">Coordinates of the geolocation</param>
		protected string GetBuildingFromCoords(Coordinates coords)
		{
			string building = strings.Unknown;
			//iterate through buildings and find one whose mesh contains the point
			foreach(KeyValuePair<string, LocationMesh> entry in _buildings)
			{
				if(entry.Value.Contains(coords))
				{
					building = entry.Key;
					break;
				}
			}
			return building;
		}
		public string[] GetBuildingNames()
		{
			List<string> result = new List<string>();
			foreach(KeyValuePair<string, LocationMesh> pair in _buildings)
			{
				result.Add(pair.Key);
			}
			return result.ToArray();
		}
		/// <summary>
		/// Public function that allows to fetch building by ILocation. 
		/// </summary>
		/// <returns>The name of the building that contains the geolocation.</returns>
		/// <param name="location">Geolocation</param>
		public string GetBuilding(ILocation location)
		{
			string result = GetBuildingFromCoords(location.Coords);
			return result;
		}
		/// <summary>
		/// Gets the bounding box of all the building meshes.
		/// </summary>
		/// <returns>The bounding box.</returns>
		public LocationBoundingBox GetBoundingBox()
		{
			//fetch the LocationMeshes from the _buildings collection
			Dictionary<string, LocationMesh>.ValueCollection meshes = _buildings.Values;
			//get an array of BoundingBoxes, corresponding to the bounding boxes of each mesh in meshes
			LocationBoundingBox[] boxes = new LocationBoundingBox[meshes.Count];
			int i = 0;
			foreach(LocationMesh mesh in meshes)
			{
				boxes[i] = mesh.GetBoundingBox();
				i++;
			}
			//merge the boxes together and return
			return LocationBoundingBox.FromBoundingBoxes(boxes);
		}
		/// <summary>
		/// Gets the mesh by building name.
		/// </summary>
		/// <returns>The mesh.</returns>
		/// <param name="buildingName">Building name.</param>
		public LocationMesh GetMesh(string buildingName)
		{
			LocationMesh result;
			_buildings.TryGetValue(buildingName, out result);
			return result;
		}
		/// <summary>
		/// Gets the POIs corresponding to centers of building meshes.
		/// </summary>
		/// <returns>The building centers array.</returns>
		public List<POI> GetBuildingCenters()
		{
			List<POI> result = new List<POI>();
			//get all building POIs
			foreach(KeyValuePair<string, LocationMesh> entry in _buildings)
			{
				POI building = new POI();
				//both building and title are the name of the building
				building.building = entry.Key;
				building.title = entry.Key;
				//get center of the mesh
				building.coords = entry.Value.GetCenter();
				result.Add(building);
			}
			return result;
		}
	}
}