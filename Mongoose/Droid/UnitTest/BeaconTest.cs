using System;
using NUnit.Framework;
using Mongoose.Droid;
using System.Collections;
namespace Mongoose.Droid
{
	[TestFixture ()]
	public class BeaconTest 
	{

		public BeaconActivity ba = new BeaconActivity();
		/// <summary>
		/// Tests the beacon major.
		/// </summary>
		[Test]
		public void TestBeaconMajor()
		{
			Assert.AreEqual(11,(int)ba.CreateRegion(11,12).Major);
		}
		/// <summary>
		/// Tests the beacon minor.
		/// </summary>
		[Test]
		public void TestBeaconMinor()
		{
			Assert.AreEqual(12,(int)ba.CreateRegion(11,12).Minor);
		}

	}
}
	