
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

        [ObservableProperty]
        private CryptocurrencyDetailedInfo? cryptocurrencyInfo;

        [ObservableProperty]
        private TopCryptocurrencies? selectedTopCryptocurrency;

        [ObservableProperty]
        private CryptocurrencySearchIInfo? selectedSearchCryptocurrency;

        [ObservableProperty]
        private string? searchText;

        public ObservableCollection<TopCryptocurrencies> TopCryptocurrencies { get; } = [];
        const int maxTopCryptocurrencies = 10;

        public ObservableCollection<CryptocurrencySearchIInfo> SearchedCryptocurrencies { get; } = [];

        private bool _isSortedAscending = true;


        public MainViewModel(ICryptocurrencyApiService cryptoService, INavigationService navigationService)
        {
            this.cryptoService = cryptoService;
            this.navigationService = navigationService;
        }

        async partial void OnSelectedTopCryptocurrencyChanged(TopCryptocurrencies? value)
        {
            if (value != null)
            {
                await GetCryptocurrencyById(value.Id);
            }
        }

        async partial void OnSelectedSearchCryptocurrencyChanged(CryptocurrencySearchIInfo? value)
        {
            if (value != null)
            {
                await GetCryptocurrencyById(value.Id);
            }
        }

        private async Task GetCryptocurrencyById(string cryptocurrencyid)
        {
            var cryptocurrency = await cryptoService.GetAssetById(cryptocurrencyid);
            CryptocurrencyInfo = cryptocurrency;
            navigationService.NavigateTo(NavigationPage.DetailInformation);
        }

        [RelayCommand]
        public async Task GetAssets()
        {
            var topCryptocurrencies = await cryptoService.GetTopAssets(maxTopCryptocurrencies);
            TopCryptocurrencies.Clear();
            foreach (var cryptocurrency in topCryptocurrencies)
            {
                TopCryptocurrencies.Add(cryptocurrency);
            }
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

    }
}
