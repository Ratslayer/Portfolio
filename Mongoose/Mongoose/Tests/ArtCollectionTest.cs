using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class ArtCollectionTest
	{
		/// <summary>
		/// Tests the collection to see if it is being bound properly
		/// </summary>
		[Test]
		public void TestCollection()
		{
			ArtCollection arts = new ArtCollection();
			Assert.NotNull(arts);
			Art art = arts[strings.ArtDisplayTitle];
			Assert.AreEqual(strings.ArtDisplayTitle, art.title);
			List<Art> artsList = arts.Arts;
			Assert.NotNull(artsList);
		}
	}
}

