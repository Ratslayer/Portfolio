using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture]
	public class FavoriteTest
	{
		/// <summary>
		/// Tests the favorite.
		/// </summary>
		[Test]
		public void TestFavorite()
		{
			Event e = new Event();

			Favorite favorite = new Favorite();
			Assert.NotNull(favorite);

			favorite = new Favorite(e);
			Assert.NotNull(favorite);

			Assert.AreEqual(e, favorite.Event);
		}
	}
}

