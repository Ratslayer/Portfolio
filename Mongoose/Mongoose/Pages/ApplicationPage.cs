using System;

using Xamarin.Forms;

namespace Mongoose
{
	public class ApplicationPage : BaseContentPage
	{
		public ApplicationPage (): base()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage()
		{
			Content = new StackLayout { 
				Children = {
					new Label { Text = "Hello Fill info about the application" }
				}
			};
		}
	}
}


