using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Microsoft.Extensions.DependencyInjection;

namespace CryptocurrenciesCollector.Pages
{
    /// <summary>
    /// Логика взаимодействия для DetailInformationPage.xaml
    /// </summary>
    public partial class DetailInformationPage : Page
    {
        private bool _isSortedAscending = true;

        public DetailInformationPage()
        {
            InitializeComponent();
            DataContext = App.Current.ServicesProvider.GetRequiredService<MainViewModel>();
        }

        private void SortByMarketPrice(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel && viewModel.CryptocurrencyInfo != null)
            {
                var collectionView = CollectionViewSource.GetDefaultView(viewModel.CryptocurrencyInfo.Markets);
                if (collectionView != null)
                {
                    collectionView.SortDescriptions.Clear();

                    if (_isSortedAscending)
                    {
                        collectionView.SortDescriptions.Add(new SortDescription("PriceUsd", ListSortDirection.Ascending));
                    }
                    else
                    {
                        collectionView.SortDescriptions.Add(new SortDescription("PriceUsd", ListSortDirection.Descending));
                    }

                    _isSortedAscending = !_isSortedAscending;
                    collectionView.Refresh();
                }
            }
        }
    }
}
