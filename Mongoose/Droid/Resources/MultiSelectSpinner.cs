//MultiSelectSpinner.cs------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Java.Lang;
using Java.Util;
using Xamarin.Forms;

namespace Mongoose.Droid
{
	class MultiSelectSpinner : Spinner, IDialogInterfaceOnMultiChoiceClickListener
	{
		/// <summary>
		/// Array of items that are displayed on the spinner.
		/// </summary>
		string[] _items = null;
		/// <summary>
		/// Array of booleans, each corresponding an element from _items.
		/// Each boolean states whether this item has been selected or not.
		/// </summary>
		bool[] _selection = null;
		/// <summary>
		/// The proxy adapter.
		/// </summary>
		public ArrayAdapter<string> _proxyAdapter;

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiSelectSpinner.MultiSelectSpinner"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public MultiSelectSpinner(Context context)
			: base(context, SpinnerMode.Dropdown)
		{
			_proxyAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem);
			base.Adapter = _proxyAdapter;
			_proxyAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleListItem1);
			base.SetMinimumHeight(45);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiSelectSpinner.MultiSelectSpinner"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attrs">Attrs.</param>
		public MultiSelectSpinner(Context context, IAttributeSet attrs)
			: base(context, attrs, 1, SpinnerMode.Dropdown)
		{
			_proxyAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem);
			base.Adapter = _proxyAdapter;
			_proxyAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleListItem1);
			base.SetMinimumHeight(45);
		}

		/// <param name="dialog">The dialog where the selection was made.</param>
		/// <param name="which">The position of the item in the list that was clicked.</param>
		/// <param name="isChecked">True if the click checked the item, else false.</param>
		/// <summary>
		/// This method will be invoked when an item in the dialog is clicked.
		/// </summary>
		public void OnClick(IDialogInterface dialog, int which, bool isChecked)
		{
			AssertWithinSelectionBounds(which);
			if (_selection != null)
			{
				_selection[which] = isChecked;
				//if no element is selected, select first element
				_selection[0] = IsNothingSelected();
				//update selection
				UpdateAdapter();
				SetSelection(0);			
				//Add or remove from settings
				UpdateSettings(which);
			}
		}
		/// <summary>
		/// Asserts the that which is within _selection bounds.
		/// </summary>
		/// <param name="which">Which.</param>
		private void AssertWithinSelectionBounds(int which)
		{
			if(which >= _selection.Length || which < 0)
			{
				throw new IllegalArgumentException("Argument 'which' is out of bounds");
			}
		}
		/// <summary>
		/// Updates the proxy adapter, which updates the dropdown view.
		/// </summary>
		private void UpdateAdapter()
		{
			_proxyAdapter.Clear();
			_proxyAdapter.Add(BuildSelectedItemString());
		}
		private void UpdateSettings(int which)
		{
			if (which != 0)
			{	
				if (_selection[which])
				{
					if (!IsSavedInSettings (_items [0], _items [which]))
					{
						AddToSettings(_items[0], _items[which]);
					}
				} 
				else
				{
					if (IsSavedInSettings (_items [0], _items [which]))
					{
						RemoveFromSettings(_items[0], _items[which]);
					}
				}
			}
		}
		/// <summary>
		/// Determines whether the multiselectspinner has nothing selected.
		/// </summary>
		/// <returns><c>true</c> if nothing is selected; otherwise, <c>false</c>.</returns>
		private bool IsNothingSelected()
		{
			bool result = true;
			//search for at least 1 selected element
			for (int i = 1; i < _items.Length; ++i)
			{
				//if at least 1 found
				if (_selection[i])
				{
					result = false;
					break;
				}
			}
			return result;
		}
		/// <summary>
		/// Call this view's OnClickListener, if it is defined.
		/// </summary>
		/// <returns>To be added.</returns>
		public override bool PerformClick()
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(Context, AlertDialog.ThemeDeviceDefaultLight);
			builder.SetMultiChoiceItems(_items, _selection, this);
			AlertDialog alert = builder.Create();
			builder.SetNegativeButton(Android.Resource.String.Ok, (s, e) => alert.Cancel());
			alert = builder.Create();
			alert.Show();
			return true;
		}

		/// <summary>
		/// Returns the adapter currently associated with this widget.
		/// </summary>
		/// <value>To be added.</value>
		public override ISpinnerAdapter Adapter
		{
			set { throw new RuntimeException("SetAdapter is not supported by MultiSelectionSpinner."); }
		}
		/// <summary>
		/// Sets the items.
		/// </summary>
		/// <param name="items">Items.</param>
		public void SetItems(string[] items)
		{
			_items = items;
			_selection = new bool[_items.Length];

			Arrays.Fill(_selection, false);
		}
		/// <param name="position">Index (starting at 0) of the data item to be selected.</param>
		/// <summary>
		/// Sets the currently selected item.
		/// </summary>
		/// <param name="selection">Selection.</param>
		public void SetSelection(string[] selection)
		{
			foreach (string sel in selection)
			{
				for (int j = 0; j < _items.Length; ++j)
				{
					if (_items[j].Equals(sel))
					{
						_selection[j] = true;
					}
				}
			}
		}

		/// <summary>
		/// Sets the selection.
		/// </summary>
		/// <param name="selectedIndicies">Selected indicies.</param>
		public void SetSelection(int[] selectedIndicies)
		{
			foreach (int index in selectedIndicies)
			{
				if (index >= 0 && index < _selection.Length)
				{
					_selection[index] = true;
				}
				else
				{
					throw new IllegalArgumentException("Index " + index + " is out of bounds.");
				}
			}
		}

		/// <summary>
		/// Gets the selected strings.
		/// </summary>
		/// <returns>The selected strings.</returns>
		public List<string> GetSelectedStrings()
		{
			List<string> selection = new List<string>();
			for (int i = 0; i < _items.Length; ++i)
			{
				if (_selection[i])
				{
					selection.Add(_items[i]);
				}
			}
			return selection;
		}

		/// <summary>
		/// Gets the selected indicies.
		/// </summary>
		/// <returns>The selected indicies.</returns>
		public List<int> GetSelectedIndicies()
		{
			List<int> selection = new List<int>();
			for (int i = 0; i < _items.Length; ++i)
			{
				if (_selection[i])
				{
					selection.Add(i);
				}
			}
			return selection;
		}

		/// <summary>
		/// Builds the selected item string.
		/// </summary>
		/// <returns>The selected item string.</returns>
		public string BuildSelectedItemString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			bool foundOne = false;

			for (int i = 0; i < _items.Length; ++i)
			{
				if (_selection[i])
				{
					if (foundOne)
					{
						sb.Append(", ");
					}
					foundOne = true;

					sb.Append(_items[i]);
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Saves to Settings.
		/// </summary>
		public void AddToSettings(string tag, string settingToBeSaved)
		{
			string type = TagToType(tag);
			AddToSettingsByType(type, settingToBeSaved);
		}
		/// <summary>
		/// Adds the settings to appropriate type of dropdown.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="settingToBeSaved">Setting to be saved.</param>
		void AddToSettingsByType(string type, string settingToBeSaved)
		{
			if (FilterDefinitions.Dictionaries[type].ContainsKey(settingToBeSaved))
			{
				string formattedSetting = FilterDefinitions.Dictionaries[type][settingToBeSaved];
				if (Settings.Filters[type] == "")
				{
					SetFilter(type, formattedSetting);
				}
				else
				{
					SetFilter(type, Settings.Filters[type] + "," + formattedSetting);
				}
			}
		}

		/// <summary>
		/// Removes from settings.
		/// </summary>
		/// <param name="tag">Tag.</param>
		/// <param name="settingToBeRemoved">Setting to be removed.</param>
		public void RemoveFromSettings(string tag, string settingToBeRemoved)
		{
			string type = TagToType(tag);
			RemoveFromSettingByType(type, settingToBeRemoved);
		}
		/// <summary>
		/// Converts the tag string to type string.
		/// Used to invoke ___ByType(...) functions.
		/// </summary>
		/// <returns>The to type.</returns>
		/// <param name="tag">Tag.</param>
		private string TagToType(string tag)
		{
			string result = "";
			switch(tag)
			{
				case "All Categories":
                case "Toutes les catégories":
					result = "category";
					break;
				case "All Units":
                case "Toutes les unités":
					result = "unit";
					break;
				case "All Audiences":
                case "Tous les publics":
					result = "audience";
					break;
				default:
					throw new IllegalArgumentException("Dropdown not found");
			}
			return result;
		}
		/// <summary>
		/// Removes the type of the from setting by.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="settingToBeRemoved">Setting to be removed.</param>
		void RemoveFromSettingByType(string type, string settingToBeRemoved)
		{
			if (FilterDefinitions.Dictionaries[type].ContainsKey(settingToBeRemoved))
			{
				string filtersStr = Settings.Filters[type];
				List<string> filtersList = Settings.Filters[type].Split(',').ToList();
				foreach (string s in filtersList)
				{
					if (s == FilterDefinitions.Dictionaries[type][settingToBeRemoved])
					{
						if (filtersStr.Contains(s + ","))
						{
							filtersStr = filtersStr.Replace(s + ",", "");
						}
						else if (filtersStr.Contains("," + s))
						{
							filtersStr = filtersStr.Replace("," + s, "");
						}
						else
						{
							filtersStr = filtersStr.Replace(s, "");
						}
					}
				}
				SetFilter(type, filtersStr);
			}
		}

		/// <summary>
		/// Determines whether this instance is saved in settings the specified tag setting.
		/// </summary>
		/// <returns><c>true</c> if this instance is saved in settings the specified tag setting; otherwise, <c>false</c>.</returns>
		/// <param name="tag">Tag.</param>
		/// <param name="setting">Setting.</param>
		public bool IsSavedInSettings(string tag, string setting)
		{
			string type = TagToType(tag);
			bool result = IsSavedInSettingsByType(type, setting);
			return result;
		}
		/// <summary>
		/// Determines whether this instance is saved in settings by type the specified type setting.
		/// </summary>
		/// <returns><c>true</c> if this instance is saved in settings by type the specified type setting; otherwise, <c>false</c>.</returns>
		/// <param name="type">Type.</param>
		/// <param name="setting">Setting.</param>
		bool IsSavedInSettingsByType(string type, string setting)
		{
			if (FilterDefinitions.Dictionaries[type].ContainsKey(setting))
			{
				string[] filters = Settings.Filters[type].Split(',');
				foreach (string s in filters)
				{
					if (s == FilterDefinitions.Dictionaries[type][setting])
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Loads the strings from saved settings.
		/// To be called only on loading of the Control
		/// </summary>
		public void LoadStringsFromSavedSettings()
		{
			string type = TagToType(_items[0]);
			LoadStringsFromSavedSettingsByType(type);
		}
		/// <summary>
		/// Loads the type of the strings from saved settings by.
		/// </summary>
		/// <param name="type">Type.</param>
		void LoadStringsFromSavedSettingsByType(string type)
		{
			if(Settings.Filters[type] == "")
			{
				this.SetSelection(new int[] { 0 });
			} 
			else
			{
				string[] categoryFilters = Settings.Filters[type].Split(',');
				foreach(string filter in categoryFilters)
				{
					for(int i = 1; i < _items.Length; ++i)
					{
						if(filter == FilterDefinitions.Dictionaries[type][_items[i]])
						{
							_selection[i] = true;
						}
					}
				}
			}
			UpdateAdapter();
		}
		/// <summary>
		/// Sets the filter.
		/// </summary>
		/// <param name="paramType">Parameter type.</param>
		/// <param name="param">Parameter.</param>
		public void SetFilter(string paramType, string param)
		{
			Dictionary<string,string> filters = Settings.Filters;
			if (filters.ContainsKey(paramType))
			{
				filters.Remove(paramType);
			}
			filters.Add(paramType, param);
			Settings.Filters = filters;
		}
	}
}