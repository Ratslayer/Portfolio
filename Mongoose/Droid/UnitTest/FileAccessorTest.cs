using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Mongoose.Droid
{
	[TestFixture()]
	public class FileAccessorTest
	{
		/// <summary>
		/// Tests the full json data.
		/// </summary>
		[Test]
		public void TestFullJsonData()
		{
			EventListViewAdapter viewAdapter = new EventListViewAdapter();
			var assembly = typeof(FileAccessorTest).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("Mongoose.Droid.TestJSONEventData.txt");
			string json = "";
			using (StreamReader sr = new StreamReader(stream))
			{
				json = sr.ReadToEnd ();
			}

			List<Event> events;
			events = new List<Event>();
			events = viewAdapter.ParseIntoObject(json);

			Event event1 = events.Find(e => e.title.Equals("Test Event 1"));

			Assert.True(events.Count == 2);
			Assert.True(event1.description.Equals("First Event test object."));
			Assert.True(event1.audiences.Count == 1);
			Assert.True(event1.audiences[0].Equals("event-audiences:all"));
			Assert.True(event1.units.Count == 3);
		}
		/// <summary>
		/// Tests the save and load dependency service.
		/// </summary>
		[Test]
		public async void TestSaveAndLoadDependencyService()
		{
			EventListViewAdapter viewAdapter = new EventListViewAdapter();

			// Serialize object to file
			List<Event> savedEvents;
			savedEvents = new List<Event>();
			string testEvent = "[{\"title\":\"Test Title\",\"description\":\"Test description\"," +
				"\"startdate\":\"2015/2/10\",\"starttime\":\"12:00\",\"enddate\":\"2015/2/10\"," +
				"\"endtime\":\"13:15\",\"allday\":\"False\",\"campus\":\"SGW\",\"building\":\"H\"," +
				"\"venue\":\"\",\"room\":\"760\",\"offlocation\":\"\",\"offcivic\":\"\",\"offlink\":\"\"," +
				"\"category\":[],\"audiences\":[],\"units\":[\"units:main\"],\"url\":\"www.example.com\"}]";
			savedEvents = viewAdapter.ParseIntoObject(testEvent);
			await Xamarin.Forms.DependencyService.Get<IFileAccessor>().SerializeAndSaveObjects(savedEvents, "test_dependency_service.json");

			List<Event> loadedEvents;
			loadedEvents = new List<Event>();
			string json = await Xamarin.Forms.DependencyService.Get<IFileAccessor>().LoadSerializedObjects("test_dependency_service.json");
			JArray jsonEvents = JArray.Parse(json);
			loadedEvents = (List<Event>)jsonEvents.ToObject(typeof(List<Event>));
			Assert.IsTrue((loadedEvents[0].title).Equals("Test Title"));
			Assert.IsTrue((loadedEvents[0].description).Equals("Test description"));
			Assert.IsTrue((loadedEvents[0].url).Equals("www.example.com"));
		}
	}
}

