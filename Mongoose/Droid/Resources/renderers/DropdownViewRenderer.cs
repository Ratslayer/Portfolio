using System;
using Mongoose;
using Mongoose.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using System.Collections.Generic;
using System.ComponentModel;

[assembly: ExportRenderer (typeof (DropdownView), typeof (DropdownViewRenderer))]

namespace Mongoose.Droid
{
	public class DropdownViewRenderer : ViewRenderer<DropdownView, DropdownSpinner>
	{
		public DropdownViewRenderer () {

		}

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<DropdownView> e) 
		{
			base.OnElementChanged (e);

			if (e.OldElement != null || this.Element == null) 
			{
				return;
			}

			if (this.Control == null) 
			{
				DropdownSpinner mySpinner = new DropdownSpinner (Forms.Context);
				mySpinner.SetItems (Element.ListItems);
				mySpinner.SetSelection (Element.SelectedItemIndex);
				mySpinner.ItemSelected += (sender, ev) => Element.SelectedItemIndex = ev.Position;
				SetNativeControl (mySpinner);
			}

			Control.LoadItems ();
		}
		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e) {
			base.OnElementPropertyChanged (sender, e);
			if (this.Element == null || this.Control == null) 
			{
				return;
			}
			if (e.PropertyName == DropdownView.SelectedItemIndexProperty.PropertyName) 
			{
				Control.SetSelection(Element.SelectedItemIndex);
			}
		}
	}
}

