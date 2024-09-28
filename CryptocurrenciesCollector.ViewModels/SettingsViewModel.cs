using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Helpers;
using WPFLocalizeExtension.Engine;

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool currentThemeIsDark = true;

        [ObservableProperty]
        private bool currentLanguageIsEnglish = true;

        [RelayCommand]
        private void ChangeTheme(string theme)
        {
            var darkTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source.ToString().Contains("DarkTheme.xaml"));

            var lightTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source.ToString().Contains("LightTheme.xaml"));

            if (theme == "DarkTheme" && darkTheme != null)
            {
                CurrentThemeIsDark = true;
                Application.Current.Resources.MergedDictionaries.Remove(darkTheme);
                Application.Current.Resources.MergedDictionaries.Add(darkTheme);
            }
            else if (theme == "LightTheme" && lightTheme != null)
            {
                CurrentThemeIsDark = false;
                Application.Current.Resources.MergedDictionaries.Remove(lightTheme);
                Application.Current.Resources.MergedDictionaries.Add(lightTheme);
            }
        }

        [RelayCommand]
        private void ChangeLanguage(string language)
        {
            if (language == "en")
            {
                CurrentLanguageIsEnglish = true;
                LocalizeDictionary.Instance.Culture = new CultureInfo("en");
            }
            else if (language == "uk")
            {
                CurrentLanguageIsEnglish = false;
                LocalizeDictionary.Instance.Culture = new CultureInfo("uk");
            }
        }

    }
}
