using System;

namespace Mongoose
{
	/// <summary>
	/// Any class that implements this listener can call ILocationMonitor::AddListener(this)
	/// and receive notifications when the building, in which the user is located, changes.
	/// </summary>
	public interface IBuildingChangeListener
	{
		/// <summary>
		/// Raises the building change event.
		/// </summary>
		/// <param name="newLocation">New location.</param>
		void OnBuildingChange(ILocation newLocation);
	}
}

