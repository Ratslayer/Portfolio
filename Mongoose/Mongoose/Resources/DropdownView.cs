using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
	public class DropdownView : View
	{
		public static readonly BindableProperty ListItemsProperty =
			BindableProperty.Create<DropdownView, List<string>> (p => p.ListItems, null);
		/// <summary>
		/// Gets or sets the list items.
		/// </summary>
		/// <value>The list items.</value>
		public List<string> ListItems
		{
			get 
			{ 
				return (List<string>)base.GetValue (ListItemsProperty);
			}
			set 
			{
				base.SetValue (ListItemsProperty, value); 
			}
		}

		public static readonly BindableProperty SelectedItemIndexProperty =
			BindableProperty.Create<DropdownView, int> (p => p.SelectedItemIndex, 0);

		public int SelectedItemIndex
		{
			get
			{
				return (int)base.GetValue (SelectedItemIndexProperty); 
			}
			set 
			{ 
				base.SetValue (SelectedItemIndexProperty, value); 
				if(ItemSelected != null)
				{
					ItemSelected(this, value);
				}
			}
		}

		public event EventHandler<int> ItemSelected;
	}
}

