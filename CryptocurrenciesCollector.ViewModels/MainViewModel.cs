
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Models;
using CryptocurrenciesCollector.Models.Interfaces;
using CryptocurrenciesCollector.Services;
using System.Windows.Navigation;
using System.Windows.Media;
using CryptocurrenciesCollector.Models.Enums;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Reflection;

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICryptocurrencyApiService cryptoService;
        private readonly INavigationService navigationService;

        const int topCryptocurrenciesNumber = 10;

        [ObservableProperty]
        private CryptocurrencyDetailedInfo cryptocurrencyInfo;

        [ObservableProperty]
        private bool hasMarkets = true;

        [ObservableProperty]
        private Cryptocurrency? selectedTopCryptocurrency;

        [ObservableProperty]
        private Cryptocurrency? selectedSearchCryptocurrency;

        [ObservableProperty]
        private string? searchText;

        [ObservableProperty]
        private string? inputAmount;

        [ObservableProperty]
        private string? outputAmount;

        [ObservableProperty]
        private Cryptocurrency convertFromCryptocurrency;

        [ObservableProperty]
        private Cryptocurrency convertToCryptocurrency;
       
        [ObservableProperty]
        private bool isCurrencyConvertAvailable;

        public ObservableCollection<Cryptocurrency> SearchedCryptocurrencies { get; } = [];
        public ObservableCollection<Cryptocurrency> Cryptocurrencies { get; } = [];

        private bool _isSortedAscending = true;

        public MainViewModel(ICryptocurrencyApiService cryptoService, INavigationService navigationService)
        {
            this.cryptoService = cryptoService;
            this.navigationService = navigationService;
        }

        async partial void OnSelectedTopCryptocurrencyChanged(Cryptocurrency? value)
        {
            if (value != null)
            {
                await GetCryptocurrencyById(value.Id);
            }
        }

        async partial void OnSelectedSearchCryptocurrencyChanged(Cryptocurrency? value)
        {
            if (value != null)
            {
                await GetCryptocurrencyById(value.Id);
            }
        }

        private async Task GetCryptocurrencyById(string cryptocurrencyId)
        {
            var cryptocurrency = await cryptoService.GetAssetById(cryptocurrencyId);
            CryptocurrencyInfo = cryptocurrency;
            HasMarkets = CryptocurrencyInfo.Markets != null;
            navigationService.NavigateTo(NavigationPage.DetailInformation);
        }

        [RelayCommand]
        public async Task GetTopAssets()
        {
            var topCryptocurrencies = await cryptoService.GetAssets(topCryptocurrenciesNumber);
            Cryptocurrencies.Clear();
            foreach (var cryptocurrency in topCryptocurrencies)
            {
                Cryptocurrencies.Add(cryptocurrency);
            }
        }

        [RelayCommand]
        private async Task GetAllCryptocurrencies()
        {
            Cryptocurrencies.Clear();
            InputAmount = OutputAmount = default;
            var cryptocurrencies = await cryptoService.GetAssets();
            foreach (var cryptocurrency in cryptocurrencies)
            {
                if (cryptocurrency.PriceUsd != 0)
                {
                    Cryptocurrencies.Add(cryptocurrency);
                }
            }
            InputAmount = "1";
            ConvertFromCryptocurrency = Cryptocurrencies[0];
            ConvertToCryptocurrency = Cryptocurrencies[0];
            IsCurrencyConvertAvailable = true;
        }

        [RelayCommand]
        public async Task GetSearchedCryptocurrencies()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var allCryptocurrencies = await cryptoService.GetSearchedAssets(SearchText);

                SearchedCryptocurrencies.Clear();
                foreach (var cryptocurrency in allCryptocurrencies)
                {
                    SearchedCryptocurrencies.Add(cryptocurrency);
                }
                navigationService.NavigateTo(NavigationPage.Search);
            }
        }

        [RelayCommand]
        private void NavigateTo(NavigationPage navigationPage)
        {
            navigationService.NavigateTo(navigationPage);
        }

        [RelayCommand]
        private void SortByMarketPrice()
        {
            if (CryptocurrencyInfo != null)
            {
                var collectionView = CollectionViewSource.GetDefaultView(CryptocurrencyInfo.Markets);
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

        [RelayCommand]
        private void ConvertCurrency()
        {
            if (decimal.TryParse(InputAmount, CultureInfo.InvariantCulture, out var inputValue))
            {
                var fromPriceUsd = ConvertFromCryptocurrency.PriceUsd;
                var toPriceUsd = ConvertToCryptocurrency.PriceUsd;
                try
                {
                    OutputAmount = ((inputValue * fromPriceUsd) / toPriceUsd).ToString(CultureInfo.InvariantCulture);
                }
                catch (OverflowException) {
                    MessageBox.Show("The entered value is too large to convert.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                    OutputAmount = null;
                }
            }
        }

        partial void OnInputAmountChanged(string? oldValue, string? newValue)
        {
            if (ConvertFromCryptocurrency != null && ConvertToCryptocurrency != null)
            {
                ConvertCurrency();
            }
        }

        partial void OnConvertFromCryptocurrencyChanged(Cryptocurrency value)
        {
            IsCurrencyConvertAvailable = false;
            if (value != null && Cryptocurrencies.Contains(value) && ConvertToCryptocurrency != null)
            {
                IsCurrencyConvertAvailable = true;
                ConvertCurrency();
            }
            else
            {
                IsCurrencyConvertAvailable = false;
            }
        }

        partial void OnConvertToCryptocurrencyChanged(Cryptocurrency value)
        {
            IsCurrencyConvertAvailable = false;
            if (value != null && Cryptocurrencies.Contains(value) && ConvertFromCryptocurrency != null)
            {
                IsCurrencyConvertAvailable = true;
                ConvertCurrency();
            }
            else
            {
                IsCurrencyConvertAvailable = false;
            }
        }

        
    }
}
