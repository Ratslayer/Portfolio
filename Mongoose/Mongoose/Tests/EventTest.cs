using System;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class EventTest
	{
		Event ev;
		Event ev2;
		/// <summary>
		/// Setups the parameter.
		/// </summary>
		[SetUp]
		public void SetupParam()
		{
			ev = new Event ();
			ev.title = "a";
			ev.startdate = "a";
			ev.enddate = "a";
			ev.description = "a";

			ev2 = new Event ();
			ev2.title = "b";
			ev2.startdate = "b";
			ev2.enddate = "b";
			ev2.description = "b";
		}

		/// <summary>
		/// Compares the object equality test.
		/// </summary>
		[Test]
		public void CompareObjectEqualityTest()
		{
			Assert.True(ev.EqualsObject (ev));
		}

		/// <summary>
		/// Compares the not object equality test.
		/// </summary>
		[Test]
		public void CompareNotObjectEqualityTest()
		{
			Assert.False(ev.EqualsObject (ev2));
		}
	}
}

