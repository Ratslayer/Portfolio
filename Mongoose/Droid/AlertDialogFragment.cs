
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Mongoose.Droid
{
	public class AlertDialogFragment : DialogFragment
	{
		/// <param name="savedInstanceState">The last saved instance state of the Fragment,
		///  or null if this is a freshly created Fragment.</param>
		/// <summary>
		/// Override to build your own custom Dialog container.
		/// </summary>
		/// <returns>To be added.</returns>
		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			var builder = new AlertDialog.Builder(Activity)
				.SetMessage("This is my dialog.")
				.SetPositiveButton("Ok", (sender, args) =>
					{
						// Do something when this button is clicked.
					})
				.SetTitle("Custom Dialog");
			return builder.Create();
		}
	}
}

