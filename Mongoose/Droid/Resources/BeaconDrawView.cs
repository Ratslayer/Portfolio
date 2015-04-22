using System;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using EstimoteSdk;

namespace Mongoose.Droid
{
	public class BeaconDrawView: View
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Mongoose.Droid.BeaconDrawView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public BeaconDrawView (Context context) : base(context)
		{
			context.StartActivity (typeof(EX_FindAllBeacons));
		}
		/// <param name="canvas">the canvas on which the background will be drawn</param>
		/// <summary>
		/// Implement this to do your drawing.
		/// </summary>
		protected override void OnDraw(Canvas canvas)
		{
		}
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		public void OnDestroy()
		{
		}


	}
}

