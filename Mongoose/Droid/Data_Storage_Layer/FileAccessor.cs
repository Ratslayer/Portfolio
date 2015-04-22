using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency (typeof (Mongoose.Droid.FileAccessor))]
namespace Mongoose.Droid
{
	public class FileAccessor : Java.Lang.Object, IFileAccessor
	{
		public Dictionary<string,string> fileNamePaths;

		public FileAccessor()
		{
			fileNamePaths = new Dictionary<string,string>();
		}
		/// <summary>
		///  Serializes POIs into a specified file in JSON format
		/// </summary>
		/// <returns>The and save objects.</returns>
		/// <param name="pointsOfInterest">Points of interest.</param>
		/// <param name="filename">Filename.</param>
		public async Task SerializeAndSaveObjects(List<Event> pointsOfInterest, string filename)
		{
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(documentsPath, filename);
			string json = string.Empty;
			using (StreamWriter sw = new StreamWriter(File.Create(filePath)))
			{
				json = JsonConvert.SerializeObject(pointsOfInterest);
				await sw.WriteLineAsync(json);
			}
		}
		/// <summary>
		/// Loads the serialized POIs and populates POI objects with the data
		/// </summary>
		/// <returns>The serialized objects.</returns>
		/// <param name="filename">Filename.</param>
		public async Task<string> LoadSerializedObjects(string filename)
		{
			System.Diagnostics.Debug.WriteLine("From file");
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(documentsPath, filename);
			StreamReader sr = new StreamReader(filePath);
			string json = await sr.ReadToEndAsync();
			System.Diagnostics.Debug.WriteLine(json);
			return json;
		}
	}
}
