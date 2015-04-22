using System;
using NUnit.Framework;
using Xamarin.Forms;

namespace Mongoose
{
	[TestFixture]
	public class ThemeTest
	{
		string str = "Test";
		/// <summary>
		/// Tests the button creation.
		/// </summary>
		[Test]
		public void TestButtonCreation()
		{
			Button button = Theme.CreateButton (str, Color.Black, Color.White);
            AssertButton(button);
		}
		/// <summary>
		/// Tests the button creation with image.
		/// </summary>
		[Test]
		public void TestButtonCreationWithImage()
		{
			Button button = Theme.CreateButton (str, Color.Black, Color.White, "event.png");
            AssertButton(button);
		}
		/// <summary>
		/// Asserts the button.
		/// </summary>
		/// <param name="button">Button.</param>
        void AssertButton(Button button)
        {
            Assert.IsNotNull(button);
            Assert.That(button.GetType() == typeof(Button));
            Assert.That(button.Text.Equals(str));
            Assert.That(button.TextColor.Equals(Color.Black));
            Assert.That(button.BackgroundColor.Equals(Color.White));
        }
		/// <summary>
		/// Tests the header creation.
		/// </summary>
		[Test]
		public void TestHeaderCreation()
		{
			Label header = Theme.CreateHeaderLabel (str);
            AssertLabel(header);
		}
		/// <summary>
		/// Tests the label creation.
		/// </summary>
		[Test]
		public void TestLabelCreation()
		{
			Label label = Theme.CreateLabel (str);
            AssertLabel(label);
		}
		/// <summary>
		/// Asserts the label.
		/// </summary>
		/// <param name="header">Header.</param>
        void AssertLabel(Label header)
        {
            Assert.IsNotNull(header);
            Assert.That(header.GetType() == typeof(Label));
            Assert.That(header.Text.Equals(str));
        }
	}
}

