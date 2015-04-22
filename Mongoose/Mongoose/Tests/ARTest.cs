using System;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class ARTest
	{
		/// <summary>
		/// Test the AR functionability
		/// </summary>
		[Test]
		public void TestARView()
		{
			BaseView view = new BaseView();
			view.RegisterListener(null);
			view.UnregisterListener();
			view.OnAppear();
			view.OnDisappear();
			Assert.AreNotSame(null, view);
		}
	}
}

