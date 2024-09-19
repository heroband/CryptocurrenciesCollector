
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
        private string selectedCryptocurrencyId;

        public ObservableCollection<Cryptocurrency> CryptocurrencyInfo { get; } = [];

        public ObservableCollection<string> Cryptocurrencies { get; }

        public MainViewModel(ICryptocurrencyApiService cryptoService)
        {
            this.cryptoService = cryptoService;
            Cryptocurrencies = new ObservableCollection<string>
            {
                "bitcoin",
                "ethereum",
                "litecoin"
            };
            SelectedCryptocurrencyId = Cryptocurrencies[0];
        }


        [RelayCommand]
        public async Task GetCryptocurrencyById() {
            var cryptocurrency = await cryptoService.GetAssetById(SelectedCryptocurrencyId);
            CryptocurrencyInfo.Add(cryptocurrency);
        }
    }
}
