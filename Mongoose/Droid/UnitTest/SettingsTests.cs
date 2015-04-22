using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Mongoose.Droid
{
	[TestFixture]
	public class SettingsTests
	{
		/// <summary>
		/// Changes the language test.
		/// </summary>
		[Test]
		public void ChangeLanguageTest()
		{
			string locale = "fr-FR";
			Settings.Language = locale;
			Assert.That (Settings.Language.Equals (locale));
		}
		/// <summary>
		/// Tests the change filters.
		/// </summary>
		[Test]
		public void TestChangeFilters()
		{
			Settings.Filters = new Dictionary<string,string> () { {"category", "arts-culture"} };
			Assert.That (Settings.Filters["category"] == "arts-culture");
		}
		/// <summary>
		/// Tests the reset filters.
		/// </summary>
		[Test]
		public void TestResetFilters()
		{
			Settings.Filters = new Dictionary<string,string> () { {"category", "arts-culture"} };
			Settings.ResetFiltersToDefault ();
			Assert.That (Settings.Filters["category"] == "");
		}
	}
}

