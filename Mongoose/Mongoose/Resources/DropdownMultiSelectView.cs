using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
	public class DropdownMultiSelectView : View
	{
		public static readonly BindableProperty ListItemsProperty =
			BindableProperty.Create<DropdownMultiSelectView, List<string>> (p => p.ListItems, null);
		/// <summary>
		/// Gets or sets the list items.
		/// </summary>
		/// <value>The list items.</value>
		public List<string> ListItems {
			get { return (List<string>)base.GetValue (ListItemsProperty); }
			set { base.SetValue (ListItemsProperty, value); }
		}

		public static readonly BindableProperty PromptProperty =
			BindableProperty.Create<DropdownMultiSelectView, string> (p => p.Prompt, null);
		/// <summary>
		/// Gets or sets the prompt.
		/// </summary>
		/// <value>The prompt.</value>
		public string Prompt {
			get { return (string)base.GetValue (PromptProperty); }
			set { base.SetValue (PromptProperty, value); }
		}
	}
}

