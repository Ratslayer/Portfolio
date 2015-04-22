using System;
using NUnit.Framework;
using Mongoose.Droid;
namespace Mongoose.Droid
{
	[TestFixture]
	public class ARTest
	{
		/// <summary>
		/// Tests the AR view renderer.
		/// </summary>
		[Test]
		public void TestARViewRenderer()
		{
			ARViewRenderer renderer = new ARViewRenderer();
			renderer.OnAppear();
			renderer.OnDisappear();
			Assert.AreNotSame(null, renderer);
		}
	}
}

