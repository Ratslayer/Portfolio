using System;

using Xamarin.Forms;

namespace Mongoose
{
	public class HelpPage : BaseContentPage
	{
		public HelpPage (): base()
		{
			
		}

		public override void GeneratePage ()
		{
			base.GeneratePage ();
			Content = new StackLayout { 
				Children = {
					new Label { Text = "Fill info about the Help page" }
				}
			};
		}
	}
}


