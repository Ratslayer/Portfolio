using System;
using System.Collections.Generic;

namespace Mongoose
{
	public static class FilterDefinitions
	{
		/// <summary>
		/// a dictionary of all the filters existing
		/// </summary>
		private static Dictionary<string, Dictionary<string, string>> dictionaries = new Dictionary<string, Dictionary<string, string>> {
			{ "category", CategoryFilter },
			{ "audience", AudienceFilter },
			{ "unit", UnitFilter },
			{ "notify-range", TimeRangeFilter},
			{ "favorites-range", FavoritesRange }
		};
		/// <summary>
		/// Gets the dictionaries.
		/// </summary>
		/// <value>The dictionaries.</value>
		public static Dictionary<string,Dictionary<string,string>> Dictionaries {
			get 
			{
				return dictionaries;
			}
		}

		/// <summary>
		/// The list of all possible category filters
		/// </summary>
		/// <value>The category filter.</value>
		public static Dictionary<string, string> CategoryFilter {
			get {
				return new Dictionary<string, string> () {
					{ strings.CategoryAll, "" },
					{ strings.CategoryArtsAndCulture, "arts-culture" },
					{ strings.CategoryConferencesAndLectures, "conferences-lectures" },
					{ strings.CategoryExaminations, "examinations" },
					{ strings.CategoryReligiousAndSpiritual, "religious-spiritual" },
					{ strings.CategorySocialEvents, "social-events" },
					{ strings.CategorySportsAndWellness, "sports-wellness" },
					{ strings.CategoryWorkshopsSeminars, "workshops-seminars"},
					{ strings.CategoryFundraisers, "fundraisers" },
					{ strings.CategoryInformationOrientation, "information-orientation" },
					{ strings.CategoryOtherEvents, "other-events" }
				};
			}
		}

		/// <summary>
		/// The list of all possible audience filter
		/// </summary>
		/// <value>The audience filter.</value>
		public static Dictionary<string, string> AudienceFilter {
			get {
				return new Dictionary<string, string> () {
					{ strings.AudienceAll, "" },
					{ strings.AudienceAlumni, "alumni" },
					{ strings.AudienceStaff, "staff" },
					{ strings.AudienceFaculty, "faculty" },
					{ strings.AudienceManagers, "managers" },
					{ strings.AudienceStudents, "students" },
					{ strings.AudienceParents, "parents" },
					{ strings.AudienceFutureStudents, "future-students"}
				};
			}
		}

		/// <summary>
		/// The list of all possible unit filter
		/// </summary>
		/// <value>The unit filter.</value>
		public static Dictionary<string, string> UnitFilter {
			get {
				return new Dictionary<string, string> () {
					{ strings.UnitAll, "" },
					{ strings.UnitENCS, "encs" },
					{ strings.UnitArtsAndScience, "artsci" },
					{ strings.UnitOffices, "offices" },
					{ strings.UnitMain, "main" },
					{ strings.UnitBusiness, "jmsb" },
					{ strings.UnitFineArts, "finearts" }
				};
			}
		}

		/// <summary>
		/// The list of all possible time range filter
		/// </summary>
		/// <value>The time range filter.</value>
		public static Dictionary<string, string> TimeRangeFilter {
			get {
				return new Dictionary<string, string> () {
					{ strings.NotifyHour, "1" },
					{ strings.NotifyThreeHours, "3"  },
					{ strings.NotifySixHours, "6" },
					{ strings.NotifyDay, "24" },
					{ strings.NotifySevenDays, "168" },
					{ strings.NotifyThirtyDays, "720" }
				};
			}
		}

		/// <summary>
		/// The range of the favorites
		/// </summary>
		/// <value>The favorites range.</value>
		public static Dictionary<string, string> FavoritesRange {
			get {
				return new Dictionary<string, string> () {
					{ strings.FavoritesClearDay, "1" },
					{ strings.FavoritesClearWeek, "7"  },
					{ strings.FavoritesClearTwoWeeks, "14" },
					{ strings.FavoritesClearMonth, "30" },
				};
			}
		}

		/// <summary>
		/// Gets the locations (missing some other locations not mapped yet such as loyola, and other buildings)
		/// </summary>
		/// <value>The locations.</value>
		public static Dictionary<string, string> Locations {
			get {
				return new Dictionary<string, string> () {
					{ "Hall", "H" },
					{ "EV", "EV" },
					{ "Library" , "LB" },
					{ "Faubourg" , "FB" }
				};
			}
		}
	}
}

