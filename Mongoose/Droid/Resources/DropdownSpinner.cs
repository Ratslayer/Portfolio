using Android.Widget;
using Android.Content;
using Android.Util;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using System;

namespace Mongoose.Droid
{
	public class DropdownSpinner : Spinner, IDialogInterfaceOnClickListener
	{
		string[] _items = null;
		int _selected = 0;

		public ArrayAdapter<string> _proxyAdapter;

		/// <summary>
		/// Initializes a new instance of the <see cref="DropdownSpinner.DropdownSpinner"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public DropdownSpinner(Context context) : base(context, SpinnerMode.Dropdown)
		{
			_proxyAdapter = new ArrayAdapter<string> (context, Android.Resource.Layout.SimpleSpinnerItem);
			base.Adapter = _proxyAdapter;
			_proxyAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleListItem1);
			base.SetMinimumHeight (45);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DropdownSpinner.DropdownSpinner"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attrs">Attrs.</param>
		public DropdownSpinner(Context context, IAttributeSet attrs) : base(context, attrs, 1, SpinnerMode.Dropdown)
		{
			_proxyAdapter = new ArrayAdapter<string> (context, Android.Resource.Layout.SimpleSpinnerItem);
			base.Adapter = _proxyAdapter;
			_proxyAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleListItem1);
			base.SetMinimumHeight (45);
		}

		public override void OnClick (IDialogInterface dialog, int which)
		{
			base.OnClick (dialog, which);
			_selected = which;
		}

		/// <summary>
		/// Call this view's OnClickListener, if it is defined.
		/// </summary>
		/// <returns>To be added.</returns>
		public override bool PerformClick ()
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (Context, AlertDialog.ThemeDeviceDefaultLight);
			builder.SetSingleChoiceItems (_items, _selected, this);
			AlertDialog alert = builder.Create ();
			alert.Show ();
			return true;
		}

		/// <summary>
		/// Returns the adapter currently associated with this widget.
		/// </summary>
		/// <value>To be added.</value>
		public override ISpinnerAdapter Adapter 
		{
			set { throw new RuntimeException ("SetAdapter is not supported by MultiSelectionSpinner."); }
		}

		/// <summary>
		/// Sets the items.
		/// </summary>
		/// <param name="items">Items.</param>
		public void SetItems(string[] items) 
		{
			_items = items;
		}

		/// <summary>
		/// Sets the items.
		/// </summary>
		/// <param name="items">Items.</param>
		public void SetItems(List<string> items) 
		{
			_items = items.ToArray<string>();
		}

		public override void SetSelection (int position)
		{
			base.SetSelection (position);
			_selected = position;
		}

		public void LoadItems ()
		{
			_proxyAdapter.Clear ();
			for (int i = 0; i < _items.Length; ++i) {
				_proxyAdapter.Add (_items[i]);
			}
		}
	}

}

