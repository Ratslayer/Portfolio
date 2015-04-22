using System;
using Xamarin.Forms;

namespace Mongoose
{
	/// <summary>
	/// ARView that is implemented by the custom renderer, so that ArchitectView can be used with ContentPages.
	/// </summary>
	public class BaseView : View
	{
		/// <summary>
		/// Native View that is registered as a listener to this page, as to receive OnAppear and OnDisappear calls.
		/// </summary>
		private IContentPageListener _pageListener;
		public BaseView()
		{
		}
		/// <summary>
		/// Registers the listener.
		/// </summary>
		/// <param name="listener">Listener.</param>
		public void RegisterListener(IContentPageListener listener)
		{
			_pageListener = listener;
		}
		/// <summary>
		/// Unregisters the listener.
		/// </summary>
		public void UnregisterListener()
		{
			_pageListener = null;
		}
		/// <summary>
		/// Function called when the page is pushed on stack or appears on the top of the stack again due to a pop.
		/// </summary>
		public void OnAppear()
		{
			if(_pageListener != null)
			{
				_pageListener.OnAppear();
			}
		}
		/// <summary>
		/// Function called when the page is popped off a stack or another page is pushed on top of this one.
		/// </summary>
		public void OnDisappear()
		{
			if(_pageListener != null)
			{
				_pageListener.OnDisappear();
			}
		}
	}
}

