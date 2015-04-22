using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Mongoose
{
	[TestFixture]
	public class FilterTest
	{
		Dictionary<string, Dictionary<string, string>> dictionaries;
		/// <summary>
		/// Tests the filter definitions.
		/// </summary>
		[SetUp]
		public void TestFilterDefinitions()
		{
			dictionaries = FilterDefinitions.Dictionaries;
		}
		/// <summary>
		/// Tests the dictionaries.
		/// </summary>
		[Test]
		public void TestDictionaries(){
			foreach (string key1 in dictionaries.Keys) {
				Assert.IsNotNull (key1);
				foreach (string key2 in dictionaries[key1].Keys) {
					Assert.IsNotNull (key2);
					Assert.IsNotNull (dictionaries[key1][key2]);
				}
			}
		}
	}
}

