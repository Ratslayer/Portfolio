using System;

namespace Mongoose
{
	public interface ITapLock
	{
		TapLockStatus TapLockStatus {get; set;}
	}

	public struct TapLockStatus {
		public bool locked;
	}

	public static class TapLockHelper
	{
		public static bool ActivateTapLock (this ITapLock obj) 
		{
			// If locked is true, return false
			// If locked is false, set locked to true and return true
			bool result = false;
			if(!obj.TapLockStatus.locked)
			{
				obj.TapLockStatus = new TapLockStatus(){ locked = true };
				result = true;
			} 
			return result;
		}

		public static void ReleaseTapLock (this ITapLock obj) 
		{
			var status = obj.TapLockStatus;
			// To release tap lock, set locked to false
			status.locked = false;
			obj.TapLockStatus = status;
		}
	}
}

