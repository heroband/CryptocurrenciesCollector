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


namespace CryptocurrenciesCollector
{
    public partial class MainWindow : Window
    {
        private readonly IConfiguration _configuration;
        public MainWindow()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            InitializeComponent();

            var apiKey = _configuration["ApiSettings:CoinCapApiKey"];
            var cryptoService = new CryptocurrencyApiService(apiKey);

            DataContext = new MainViewModel(cryptoService);
        }
    }
}