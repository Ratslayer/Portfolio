using System;
using NUnit.Framework;

namespace Mongoose
{
	[TestFixture]
	public class ArtTest
	{
		string title = "Library Art Display";
		string description = "New acquisitions at the Concordia Library";
		string url = "http://library.concordia.ca/about/news/#cat=exhibitions";
		string room = "LB-2";

		/// <summary>
		/// Verify that the binding works
		/// </summary>
		[Test]
		public void TestArtCreation()
		{
			Art art = new Art();
			art.title = title;
			art.description = description;
			art.url = url;
			art.room = room;

			StringAssert.AreEqualIgnoringCase(art.title, title);
			StringAssert.AreEqualIgnoringCase(art.description, description);
			StringAssert.AreEqualIgnoringCase(art.url, url);
			StringAssert.AreEqualIgnoringCase(art.room, room);
		}

		/// <summary>
		/// Verify that the binding with param works
		/// </summary>
		[Test]
		public void TestArtCreationWithParameters()
		{
			Art art = new Art(title, description, url, room, null);
			StringAssert.AreEqualIgnoringCase(art.title, title);
			StringAssert.AreEqualIgnoringCase(art.description, description);
			StringAssert.AreEqualIgnoringCase(art.url, url);
			StringAssert.AreEqualIgnoringCase(art.room, room);
		}
	}
}

