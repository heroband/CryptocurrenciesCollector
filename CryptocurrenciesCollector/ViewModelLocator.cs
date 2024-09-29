using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptocurrenciesCollector.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CryptocurrenciesCollector
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.Current.ServicesProvider.GetRequiredService<MainViewModel>();
        public SettingsViewModel SettingsViewModel => App.Current.ServicesProvider.GetRequiredService<SettingsViewModel>();

    }
}
