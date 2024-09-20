
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Models;
using CryptocurrenciesCollector.Models.Interfaces;
using CryptocurrenciesCollector.Services;

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICryptocurrencyApiService cryptoService;

        [ObservableProperty]
        private string? selectedCryptocurrencyId;

        [ObservableProperty]
        private Cryptocurrency? cryptocurrencyInfo;

        public ObservableCollection<TopCryptocurrencies> TopCryptocurrencies { get; } = [];
        //const int maxTopCryptocurrencies = 15;

        public ObservableCollection<string>? Cryptocurrencies { get; } = [];

        public MainViewModel(ICryptocurrencyApiService cryptoService)
        {
            this.cryptoService = cryptoService;
            
        }

        [RelayCommand]
        public async Task GetCryptocurrencyById()
        {
            var cryptocurrency = await cryptoService.GetAssetById(SelectedCryptocurrencyId);
            CryptocurrencyInfo = cryptocurrency;
        }

        public async Task GetAssets()
        {
            var topCryptocurrencies = await cryptoService.GetAssets();
            foreach (var cryptocurrency in topCryptocurrencies)
            {
                TopCryptocurrencies.Add(cryptocurrency);
                Cryptocurrencies.Add(cryptocurrency.Id);
            }
            SelectedCryptocurrencyId = Cryptocurrencies[0];
        }
    }
}
