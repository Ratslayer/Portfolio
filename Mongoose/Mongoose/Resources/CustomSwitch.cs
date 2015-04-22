using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mongoose
{
    public class CustomSwitch : View
    {
        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create<CustomSwitch, bool> (p => p.IsToggled, true);

		/// <summary>
		/// Gets or sets a value indicating whether this instance is toggled.
		/// </summary>
		/// <value><c>true</c> if this instance is toggled; otherwise, <c>false</c>.</value>
        public bool IsToggled
        {
            get 
            { 
                return (bool)base.GetValue (IsToggledProperty);
            }
            set 
            {
                base.SetValue (IsToggledProperty, value); 
                if(Toggled != null)
                {
                    Toggled(this, value);
                }
            }
        }
            
        public event EventHandler<bool> Toggled;
    }
}

