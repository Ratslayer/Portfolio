using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Mongoose.Droid
{
	public class FilterRetrievalTest
	{
		/// <summary>
		/// Tests the filter retrieval from settings.
		/// </summary>
		[Test]
		public void TestFilterRetrievalFromSettings()
		{
			EventRESTAPIConnector restConnector = new EventRESTAPIConnector();
			List<string> filters = restConnector.GetFiltersFromSettings();
			Assert.True(filters.Count == 3);
		}
		/// <summary>
		/// Tests the URL creation.
		/// </summary>
		[Test]
		public void TestURLCreation()
		{
			EventRESTAPIConnector restConnector = new EventRESTAPIConnector();
			string url = "http://www-qa.concordia.ca/etc/designs/concordia/resources/events.json?building=&category=&audience=&unit=&";
			string createUrl = restConnector.CreateURL("");
			Assert.True(createUrl.Contains(url));
		}
	}
}

