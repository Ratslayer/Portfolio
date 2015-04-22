using System;
using Mongoose;
using Mongoose.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using System.Collections.Generic;

[assembly: ExportRenderer (typeof (DropdownMultiSelectView), typeof (DropdownMultiSelectViewRenderer))]

namespace Mongoose.Droid
{
	public class DropdownMultiSelectViewRenderer : ViewRenderer<DropdownMultiSelectView, Spinner>
	{
		public DropdownMultiSelectViewRenderer() 
		{
		}

		MultiSelectSpinner mySpinner = new MultiSelectSpinner (Forms.Context);

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<DropdownMultiSelectView> e) 
		{
			base.OnElementChanged (e);

			if(e.OldElement != null || this.Element == null)
			{
				return;
			}

			if (this.Control == null) 
			{
				mySpinner.SetItems (Element.ListItems.ToArray());
				SetNativeControl (mySpinner);
			}

			mySpinner.LoadStringsFromSavedSettings ();
		}
	}
}


