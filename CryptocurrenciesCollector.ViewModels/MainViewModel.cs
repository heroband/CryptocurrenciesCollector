
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

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICryptocurrencyApiService cryptoService;
        private readonly INavigationService? navigationService;

        [ObservableProperty]
        private Cryptocurrency? cryptocurrencyInfo;

        [ObservableProperty]
        private TopCryptocurrencies? selectedTopCryptocurrency;

        [ObservableProperty]
        private string? searchText;

        public ObservableCollection<TopCryptocurrencies> TopCryptocurrencies { get; } = [];
        //const int maxTopCryptocurrencies = 15;


        public MainViewModel(ICryptocurrencyApiService cryptoService)
        {
            this.cryptoService = cryptoService;
        }

        partial void OnSelectedTopCryptocurrencyChanged(TopCryptocurrencies? value)
        {
            if (value != null)
            {
                GetCryptocurrencyByIdCommand.Execute(value.Id);
            }
        }

        [RelayCommand]
        private async Task GetCryptocurrencyById(string cryptocurrencyid)
        {
            var cryptocurrency = await cryptoService.GetAssetById(cryptocurrencyid);
            CryptocurrencyInfo = cryptocurrency;
        }

        [RelayCommand]
        public async Task GetAssets()
        {
            var topCryptocurrencies = await cryptoService.GetAssets();
            TopCryptocurrencies.Clear();
            foreach (var cryptocurrency in topCryptocurrencies)
            {
                TopCryptocurrencies.Add(cryptocurrency);
            }
        }

        [RelayCommand]
        public void Search()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Debug.WriteLine($"Искать: {SearchText}");
            }
        }

    }
}
