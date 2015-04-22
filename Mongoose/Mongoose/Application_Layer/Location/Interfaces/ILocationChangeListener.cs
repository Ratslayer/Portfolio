using System;

namespace Mongoose
{
	/// <summary>
	/// Any class that implements this interface can call ILocationMonitor::AddListener(this) and will receive updates when the current location changes
	/// </summary>
	public interface ILocationChangeListener
	{
		/// <summary>
		/// Raises the location change event.
		/// </summary>
		/// <param name="newLocation">New location.</param>
		void OnLocationChange(ILocation newLocation);
	}
}

