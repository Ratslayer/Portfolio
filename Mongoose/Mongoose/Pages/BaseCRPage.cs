using System;

namespace Mongoose
{
	public class BaseCRPage<T> : BaseContentPage where T : BaseView, new()
	{
		private T _view;
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage()
		{
			base.GeneratePage();
			_view = new T();
			Content = _view;
		}
		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing()
		{
			base.OnAppearing();
			_view.OnAppear();
		}
		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_view.OnDisappear();
		}
	}
}

