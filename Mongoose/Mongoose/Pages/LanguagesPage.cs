using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mongoose
{
	public class LanguagesPage : BaseContentPage
	{
		public LanguagesPage () : base()
		{
		}
		/// <summary>
		/// Generates the page.
		/// </summary>
		public override void GeneratePage ()
		{
			base.GeneratePage();
			Title = strings.Language;

            StackLayout englishButtonStack = CreateSettingsTextCell(strings.English, "en_tag.png");
            var englishTap = new TapGestureRecognizer();
            englishTap.Tapped += (sender, e) =>
                    ChangeLanguage ("en-US", englishButtonStack);
            englishButtonStack.GestureRecognizers.Add(englishTap);

			StackLayout frenchButtonStack = CreateSettingsTextCell(strings.French, "fr_tag.png");
            var frenchTap = new TapGestureRecognizer();
            frenchTap.Tapped += (sender, e) =>
                    ChangeLanguage ("fr-FR", frenchButtonStack);
            frenchButtonStack.GestureRecognizers.Add(frenchTap);

			Content = new StackLayout { 
				Children = {
                    englishButtonStack,
                    frenchButtonStack
				},
				BackgroundColor = Theme.backgroundLightColor
			};
		}

        async void ChangeLanguage(string culture, StackLayout stack)
		{
            stack.BackgroundColor = Theme.textLightColor;

			var allow = await DisplayAlert (strings.Warning, strings.LanguageAvailability, strings.OK, strings.Cancel);
			if (allow) {
				Settings.Language = culture;
				strings.Culture = new CultureInfo (Settings.Language);
				MessagingCenter.Send<LanguagesPage> (this, "Language changed.");
			}

            stack.BackgroundColor = Theme.backgroundLightColor;
		}
	}
}


