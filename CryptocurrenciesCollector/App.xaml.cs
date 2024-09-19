using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using CryptocurrenciesCollector.Models.Interfaces;
using CryptocurrenciesCollector.Services;
using CryptocurrenciesCollector.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptocurrenciesCollector
{
    public partial class App : Application
    {
        private readonly IConfiguration _configuration;

        public App()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
            ServicesProvider = ConfigureServices();
        }

        public new static App Current => (App)Application.Current;
        public IServiceProvider ServicesProvider { get; }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICryptocurrencyApiService, CryptocurrencyApiService>(s => 
                new CryptocurrencyApiService(_configuration["ApiSettings:CoinCapApiKey"])
            );
            services.AddSingleton<MainViewModel>();
            return services.BuildServiceProvider();
        }
    }

}
