using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CryptocurrenciesCollector.Models.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo(string pageName);
    }
}
