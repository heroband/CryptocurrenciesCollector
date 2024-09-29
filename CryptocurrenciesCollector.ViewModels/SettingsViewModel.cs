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
using System.Windows.Controls.Primitives;
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
            if (theme != Properties.Settings.Default.Theme)
            {
                var isCurrentThemeDark = Properties.Settings.Default.Theme == "Dark";

                var darkTheme = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source.ToString().Contains("DarkTheme.xaml"));

                var lightTheme = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source.ToString().Contains("LightTheme.xaml"));

                if (!isCurrentThemeDark && darkTheme != null)
                {
                    CurrentThemeIsDark = true;
                    Application.Current.Resources.MergedDictionaries.Remove(darkTheme);
                    Application.Current.Resources.MergedDictionaries.Add(darkTheme);
                }
                else
                {
                    CurrentThemeIsDark = false;
                    Application.Current.Resources.MergedDictionaries.Remove(lightTheme);
                    Application.Current.Resources.MergedDictionaries.Add(lightTheme);
                }
                Properties.Settings.Default.Theme = CurrentThemeIsDark ? "Dark" : "Light";
                Properties.Settings.Default.Save();
            }
        }

        [RelayCommand]
        private void ChangeLanguage(string language)
        {
            if (language != Properties.Settings.Default.Language)
            {
                if (language == "English")
                {
                    CurrentLanguageIsEnglish = true;
                    LocalizeDictionary.Instance.Culture = new CultureInfo("en");
                }
                else if (language == "Ukrainian")
                {
                    CurrentLanguageIsEnglish = false;
                    LocalizeDictionary.Instance.Culture = new CultureInfo("uk");
                }

                Properties.Settings.Default.Language = CurrentLanguageIsEnglish ? "English" : "Українська";
                Properties.Settings.Default.Save();
            }
        }

        [RelayCommand]
        private void InitializeSettings()
        {
            var themeName = Properties.Settings.Default.Theme;
            var theme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source.ToString().Contains($"{themeName}Theme.xaml"));

            var language = Properties.Settings.Default.Language;

            LocalizeDictionary.Instance.Culture = language == "English" ? new CultureInfo("en") : new CultureInfo("uk");

            Application.Current.Resources.MergedDictionaries.Remove(theme);
            Application.Current.Resources.MergedDictionaries.Add(theme);

            CurrentThemeIsDark = themeName == "Dark";
            CurrentLanguageIsEnglish = language == "English";
        }
    }
}
