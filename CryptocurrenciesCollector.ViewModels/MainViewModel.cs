
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Services;

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly CryptocurrencyApiService CryptoService;

        [ObservableProperty]
        private string cryptocurrencyId = "bitcoin";

        private ObservableCollection<string> cryptocurrencyInfo = new ObservableCollection<string>();

        public ObservableCollection<string> Cryptocurrencies { get; set; }

        public MainViewModel(CryptocurrencyApiService cryptoService)
        {
            CryptoService = cryptoService;
            Cryptocurrencies = new ObservableCollection<string>
            {
                "bitcoin",
                "ethereum",
                "litecoin"
            };
            
        }


        [RelayCommand]
        public async Task GetCryptocurrencyById() {
            var result = await CryptoService.GetAssetById(CryptocurrencyId);
            cryptocurrencyInfo.Add(result);
        }
    }
}
