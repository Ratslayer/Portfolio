using System;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class TapLockHelperTest : ITapLock
	{
		#region ITapLock implementation

		TapLockStatus ITapLock.TapLockStatus { get; set; }
		#endregion

		/// <summary>
		/// Tests the activate tap lock.
		/// </summary>
		[Test]
		public void TestActivateTapLock()
		{
			this.ActivateTapLock ();
			Assert.IsFalse (this.ActivateTapLock ());
		}

		[Test]
		public void TestReleaseTapLock()
		{
			this.ReleaseTapLock ();
			Assert.IsTrue (this.ActivateTapLock ());
		}
	}
}