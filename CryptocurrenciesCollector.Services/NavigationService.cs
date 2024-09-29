 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CryptocurrenciesCollector.Models.Enums;
using CryptocurrenciesCollector.Models.Interfaces;

namespace CryptocurrenciesCollector.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame frame;
        private readonly Dictionary<NavigationPage, Func<Page>> pages;

        public NavigationService(Frame frame)
        {
            this.frame = frame;
            pages = [];
        }

        public void AddNavigationPage(NavigationPage navigationPage, Func<Page> page)
        {
            pages.Add(navigationPage, page);
        }

        public void NavigateTo(NavigationPage navigationPage)
        {
            var page = pages[navigationPage];
            frame.NavigationService.Navigate(page());
        }
    }
}
