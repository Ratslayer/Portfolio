using System;

namespace Mongoose
{
	public interface ILocationMonitor
	{
		/// <summary>
		/// Register the listener with the monitor so it can receive OnLocationChange calls.
		/// </summary>
		/// <param name="listener">Listener.</param>
		void AddListener(ILocationChangeListener listener);
		/// <summary>
		/// Register the listener with the monitor so it can receive OnBuildingChange calls.
		/// </summary>
		/// <param name="listener">Listener.</param>
		void AddListener(IBuildingChangeListener listener);
		/// <summary>
		/// Remove the listener, disabling OnLocationChange calls.
		/// </summary>
		/// <param name="listener">Listener.</param>
		void RemoveListener(ILocationChangeListener listener);
		/// <summary>
		/// Remove the listener with the monitor so it can receive OnBuildingChange calls.
		/// </summary>
		/// <param name="listener">Listener.</param>
		void RemoveListener(IBuildingChangeListener listener);
		/// <summary>
		/// Request a location update from GPS service.
		/// </summary>
		/// <param name="numUpdates">Number of updates requested.</param>
		/// <param name="interval">Time interval between updates in seconds.</param>
		/// <param name="settings">Accuracy/Battery settings which allow to save battery at expense of accuracy.</param>
		void UpdateLocation(int numUpdates = 1, double interval = 10.0, AccuracyBatterySettings settings = AccuracyBatterySettings.HighAccuracy_HighBattery);
		/// <summary>
		/// Gets the last location returned through UpdateLocation(). Cached value.
		/// </summary>
		/// <value>The last location.</value>
		ILocation LastLocation { get;}
	}
}

