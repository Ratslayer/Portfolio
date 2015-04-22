using System;

namespace Mongoose
{
	/// <summary>
	/// This class is responsible for requesting a location update at specific intervals.
	/// Those intervals vary, based on how close you are to the university campus, as to save battery power.
	/// </summary>
	public class LocationUpdateManager : ILocationChangeListener
	{
		/// <summary>
		/// The factor by which campus bounding box is multiplied to receive the bounding box that determines whether you are near or far.
		/// </summary>
		private double _nearFactor = 3;
		/// <summary>
		/// The location update interval when you are at the campus.
		/// </summary>
		private double _atInterval = 10;
		/// <summary>
		/// The location update interval, when you are near the campus.
		/// </summary>
		private double _nearInterval = 30;
		/// <summary>
		/// The location update interval, when you are far away from the campus.
		/// </summary>
		private double _farInterval = 60;
		/// <summary>
		/// The global building collection reference. 
		/// </summary>
		private BuildingLocationCollection _buildings;
		/// <summary>
		/// The global location monitor reference.
		/// </summary>
		private ILocationMonitor _monitor;
		/// <summary>
		/// is battery saving enabled?
		/// </summary>
		private bool _batterySavingEnabled;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Mongoose.LocationUpdateManager"/> battery saving enabled.
		/// </summary>
		/// <value><c>true</c> if battery saving enabled; otherwise, <c>false</c>.</value>
		public bool BatterySavingEnabled
		{
			get
			{
				return _batterySavingEnabled;
			}
			set
			{
				_batterySavingEnabled = value;
			}
		}
		/// <summary>
		/// parametrised constructor
		/// </summary>
		/// <param name="buildings">Building collection reference.</param>
		/// <param name="monitor">Location monitor reference.</param>
		public LocationUpdateManager(BuildingLocationCollection buildings, ILocationMonitor monitor)
		{
			_buildings = buildings;
			_monitor = monitor;
			_monitor.AddListener(this);
		}
		#region ILocationChangeListener implementation
		/// <summary>
		/// Raises the location change event.
		/// Fetches the state associated with the new location and asks for another update, but with new parameters.
		/// </summary>
		/// <param name="newLocation">New location.</param>
		public void OnLocationChange(ILocation newLocation)
		{
			CampusProximity proximity;
			AccuracyBatterySettings settings;
			double interval;
			//fetch the settings and the interval
			//proximity is only for debugging
			GetState(newLocation, out proximity, out settings, out interval);
			//request location updatewith given intervals and settings
			_monitor.UpdateLocation(1, interval, settings);
		}
		#endregion
		/// <summary>
		/// Gets the state associated with an ILocation. 
		/// Returns a CampusProximity enum, AccuracyBatterySettings enum and a time interval.
		/// </summary>
		/// <param name="location">Location.</param>
		/// <param name="proximity">Proximity.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="interval">Interval.</param>
		public void GetState(ILocation location, out CampusProximity proximity, out AccuracyBatterySettings settings, out double interval)
		{
			proximity 	= GetCampusArea(location);
			settings 	= GetSettings(proximity);
			interval 	= GetInterval(proximity);
		}
		/// <summary>
		/// Get the campus proximity enum associated with the location.
		/// </summary>
		/// <returns>The campus proximity enum.</returns>
		/// <param name="location">Location.</param>
		private CampusProximity GetCampusArea(ILocation location)
		{
			CampusProximity result;
			LocationBoundingBox campusBox = _buildings.GetBoundingBox();
			//if the battery savings are disabled or the location is inside the campus bounding box
			if(!_batterySavingEnabled || campusBox.Contains(location.Coords))
			{
				result = CampusProximity.At;
			} 
			//if the location is inside the scaled near bounding box.
			else if((campusBox * _nearFactor).Contains(location.Coords))
			{
				result = CampusProximity.Near;
			}
			else
			{
				result = CampusProximity.Far;
			}
			return result;
		}
		/// <summary>
		/// Gets the battery settings associated with the proximity enum.
		/// </summary>
		/// <returns>The settings.</returns>
		/// <param name="proximity">Proximity.</param>
		private AccuracyBatterySettings GetSettings(CampusProximity proximity)
		{
			AccuracyBatterySettings result;
			switch(proximity)
			{
				case(CampusProximity.Near):
					result = AccuracyBatterySettings.AverageAccuracy_AverageBattery;
					break;
				case(CampusProximity.Far):
					result = AccuracyBatterySettings.LowAccuracy_LowBattery;
					break;
				default:
					result = AccuracyBatterySettings.HighAccuracy_HighBattery;
					break;
			}
			return result;
		}
		/// <summary>
		/// Gets the update interval associated with the proximity.
		/// </summary>
		/// <returns>The interval.</returns>
		/// <param name="proximity">Proximity.</param>
		private double GetInterval(CampusProximity proximity)
		{
			double result;
			switch(proximity)
			{
				case(CampusProximity.Near):
					result = _nearInterval;
					break;
				case(CampusProximity.Far):
					result = _farInterval;
					break;
				default:
					result = _atInterval;
					break;
			}
			return result;
		}
	}
}