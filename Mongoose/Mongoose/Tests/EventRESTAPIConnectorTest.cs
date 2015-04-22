using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
namespace Mongoose
{
	[TestFixture()]
	public class EventRESTAPIConnectorTest
	{
		/// <summary>
		/// Tests the connector creation.
		/// </summary>
		[Test]
		public void TestConnectorCreation()
		{
			EventRESTAPIConnector restConnector = new EventRESTAPIConnector();
			StringAssert.AreEqualIgnoringCase("", restConnector.Building);
			Assert.IsNotNull(restConnector.filters);
		}
		/// <summary>
		/// Tests the on location change.
		/// </summary>
		[Test]
		public void TestOnLocationChange()
		{
			EventRESTAPIConnector restConnector = new EventRESTAPIConnector();
			restConnector.OnBuildingChange(new FakeLocation());
			StringAssert.AreEqualIgnoringCase(restConnector.Building,new FakeLocation().Building);
		}
		/// <summary>
		/// Tests the on building change.
		/// </summary>
		[Test]
		public void TestOnBuildingChange()
		{
			EventRESTAPIConnector restConnector = new EventRESTAPIConnector();
			restConnector.OnBuildingChange(new FakeLocation());
			Assert.IsNotNull(restConnector);
		}
		/// <summary>
		/// Tests the event RESTAPI connector creation.
		/// </summary>
		[Test]
		public void TestEventRESTAPIConnectorCreation()
		{
			EventRESTAPIConnector restConnector = new EventRESTAPIConnector();
			Assert.True(restConnector.filters.Count == 0);
		}
		/// <summary>
		/// Tests the connection to the REST API
		/// </summary>
		[Test]
		public void TestSuccessfulConnectionToRESTAPI()
		{
			string url = "http://www-qa.concordia.ca/etc/designs/concordia/resources/events.json?campus=SGW&building=EV";
			EventRESTAPIConnector restConnector = new EventRESTAPIConnector();
			Task<string> responseTask = restConnector.CallRESTAPI(url);
			responseTask.Wait();
			string response = responseTask.Result;
			Assert.IsNotNullOrEmpty(response);
		}
	}
}

