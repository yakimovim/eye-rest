using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace EyeRest.Model
{
    internal class Language
    {
        public static event EventHandler OnChange;

        public static void SetCulture(int languageIndex)
        {
            var cultureInfo = GetCultureInfo(languageIndex);

            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            SetResourceDictionary(cultureInfo);

            OnChange?.Invoke(null, EventArgs.Empty);
        }

        private static void SetResourceDictionary(CultureInfo cultureInfo)
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (cultureInfo.Name)
            {
                case "ru-RU":
                    dict.Source = new Uri(string.Format("Resources/UITexts.{0}.xaml", cultureInfo.Name), UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources/UITexts.xaml", UriKind.Relative);
                    break;
            }

            ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                          where d.Source != null && d.Source.OriginalString.StartsWith("Resources/UITexts.")
                                          select d).First();
            if (oldDict != null)
            {
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }

        private static CultureInfo GetCultureInfo(int languageIndex)
        {
            switch(languageIndex)
            {
                case 1: return new CultureInfo("ru-RU");
                default: return new CultureInfo("en-US");
            }
        }

    }
}
