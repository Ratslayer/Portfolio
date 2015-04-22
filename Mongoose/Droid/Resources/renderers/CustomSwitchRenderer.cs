using System;
using Xamarin.Forms;
using Mongoose.Droid;
using Xamarin.Forms.Platform.Android;
using Mongoose;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]

namespace Mongoose.Droid
{
    public class CustomSwitchRenderer : ViewRenderer<CustomSwitch, Android.Widget.CheckBox>
	{
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CustomSwitch> e)
		{
			base.OnElementChanged(e);

			if(e.OldElement != null || Element == null)
			{
				return;
			}

			if (Control == null) {
				Android.Widget.CheckBox myCheckbox = new Android.Widget.CheckBox (Context);
                myCheckbox.CheckedChange += CheckBoxCheckedChange;
				myCheckbox.Checked = Element.IsToggled;
				SetNativeControl (myCheckbox);
			}
		}
		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E read function</param>
		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e) {
			base.OnElementPropertyChanged (sender, e);
			if (this.Element == null || this.Control == null) 
			{
				return;
			}
			if (e.PropertyName == Switch.IsToggledProperty.PropertyName) 
			{
				Control.Checked = Element.IsToggled;
			}
		}
		/// <summary>
		/// Checks the box checked change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E read the function</param>
        void CheckBoxCheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            Element.IsToggled = e.IsChecked;
        }
	}
}

