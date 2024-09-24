
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

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICryptocurrencyApiService cryptoService;
        private readonly INavigationService navigationService;

        const int topCryptocurrenciesNumber = 10;
        const int assetsLimit = 2000;


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
        private double inputAmount;

        [ObservableProperty]
        private double outputAmount;

        [ObservableProperty]
        private Cryptocurrency convertFrom;

        [ObservableProperty]
        private Cryptocurrency convertTo;

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
            var cryptocurrencies = await cryptoService.GetAssets(assetsLimit);
            Cryptocurrencies.Clear();
            foreach (var cryptocurrency in cryptocurrencies)
            {
                Cryptocurrencies.Add(cryptocurrency);
            }
            ConvertFrom = Cryptocurrencies[0];
            ConvertTo = Cryptocurrencies[0];
        }

        [RelayCommand]
        public async Task GetSearchedCryptocurrencies()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var allCryptocurrencies = await cryptoService.GetSearchedAssets(SearchText, assetsLimit);

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

    }
}
