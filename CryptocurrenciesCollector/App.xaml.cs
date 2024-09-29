using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using CryptocurrenciesCollector.Models.Enums;
using System.Windows.Controls;
using CryptocurrenciesCollector.Models.Interfaces;
using CryptocurrenciesCollector.Services;
using CryptocurrenciesCollector.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CryptocurrenciesCollector.Pages;
using System.Globalization;
using WPFLocalizeExtension.Engine;
using System.Diagnostics;

namespace CryptocurrenciesCollector
{
    public partial class App : Application
    {
        private readonly IConfiguration _configuration;

        public App()
        {
            this.Activated += OnNavigationInitialization;
            LocalizeDictionary.Instance.Culture = new CultureInfo("en");

            var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT_STAGE");

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
            ServicesProvider = ConfigureServices(_configuration);
        }

        public new static App Current => (App)Application.Current;
        public IServiceProvider ServicesProvider { get; }

        private static IServiceProvider ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddTransient<ICryptocurrencyApiService, CryptocurrencyApiService>(s => 
                new CryptocurrencyApiService(configuration["ApiSettings:CoinCapApiKey"])
            );
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsViewModel>();


            services.AddSingleton<INavigationService, NavigationService>(provider =>
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                return new NavigationService(mainWindow.BaseFrame);
            });

            return services.BuildServiceProvider();
        }

        private void OnNavigationInitialization(object? sender, EventArgs e)
        {
            this.Activated -= OnNavigationInitialization;
            var navigationService = ServicesProvider.GetRequiredService<INavigationService>();
            var viewModel = ServicesProvider.GetRequiredService<MainViewModel>();
            var settingsViewModel = ServicesProvider.GetRequiredService<SettingsViewModel>();

            navigationService.AddNavigationPage(NavigationPage.Main, () => new MainPage(viewModel));
            navigationService.AddNavigationPage(NavigationPage.DetailInformation, () => new DetailInformationPage(viewModel));
            navigationService.AddNavigationPage(NavigationPage.Search, () => new SearchPage(viewModel));
            navigationService.AddNavigationPage(NavigationPage.Convert, () => new ConvertPage(viewModel));
            navigationService.AddNavigationPage(NavigationPage.Settings, () => new SettingsPage(settingsViewModel));

        }
    }

}
