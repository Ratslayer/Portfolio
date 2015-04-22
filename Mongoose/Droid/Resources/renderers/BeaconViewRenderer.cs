using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using EstimoteSdk;
using Android.Content;
using Android.App;
using Android.OS;
using Android.Views;


[assembly: ExportRenderer (typeof(Mongoose.BeaconView), typeof(Mongoose.Droid.BeaconViewRenderer))]
namespace Mongoose.Droid
{
	public class BeaconViewRenderer: ViewRenderer<BeaconView, BeaconDrawView>, IContentPageListener
	{

		private BeaconDrawView _view;
		protected override void OnElementChanged(ElementChangedEventArgs<BeaconView> e)
		{
			base.OnElementChanged(e);
			if(e.OldElement != null)
			{
				e.OldElement.UnregisterListener();
			}
			e.NewElement.RegisterListener(this);
		}

		#region IContentPageListener implementation

		public void OnAppear ()
		{
			_view = new BeaconDrawView (Forms.Context);
			SetNativeControl (_view);
		}

		public void OnDisappear ()
		{
			_view.OnDestroy ();
		}

		#endregion
	}
}

