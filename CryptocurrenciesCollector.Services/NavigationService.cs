 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CryptocurrenciesCollector.Models.Interfaces;

namespace CryptocurrenciesCollector.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;
        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(string cryptocurrencyId)
        {
            
        }
    }
}
