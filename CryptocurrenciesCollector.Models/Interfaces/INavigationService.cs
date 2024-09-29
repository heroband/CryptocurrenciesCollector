using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CryptocurrenciesCollector.Models.Enums;

namespace CryptocurrenciesCollector.Models.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo(NavigationPage navigationPage);
        void AddNavigationPage(NavigationPage navigationPage, Func<Page> page);
    }
}
