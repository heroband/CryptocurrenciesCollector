using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CryptocurrenciesCollector.ViewModels;
using CryptocurrenciesCollector.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using CryptocurrenciesCollector.Pages;
using CryptocurrenciesCollector.Models;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Models.Interfaces;


namespace CryptocurrenciesCollector
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Current.ServicesProvider.GetRequiredService<MainViewModel>();
            BaseFrame.Content = new DetailInformationPage();
        }


        //private void NavigateToMainPage(object sender, RoutedEventArgs e)
        //{
        //    BaseFrame.NavigationService.Navigate(new MainPage());
        //    if (sender is Button button)
        //    {
        //        UpdateButtonVisibility(button);
        //    }
        //}

        //private void NavigateToDetailInformationPage(object sender, RoutedEventArgs e)
        //{
        //    BaseFrame.NavigationService.Navigate(new DetailInformationPage());
        //    if (sender is Button button)
        //    {
        //        UpdateButtonVisibility(button);
        //    }
        //}

        //private void NavigateToSearchPage(object sender, RoutedEventArgs e)
        //{
        //    BaseFrame.NavigationService.Navigate(new SearchPage());
        //    if (sender is Button button)
        //    {
        //        UpdateButtonVisibility(button);
        //    }
        //}

        //private void UpdateButtonVisibility(Button pressedButton)
        //{
        //    MainPageButton.Visibility = Visibility.Visible;
        //    DetailInformationPageButton.Visibility = Visibility.Visible;

        //    pressedButton.Visibility = Visibility.Hidden;
        //}
    }
}