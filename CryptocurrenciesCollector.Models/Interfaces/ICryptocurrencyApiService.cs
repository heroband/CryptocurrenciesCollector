using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CryptocurrenciesCollector.Models.Interfaces
{
    public interface ICryptocurrencyApiService
    {
        Task<CryptocurrencyDetailedInfo> GetAssetById(string id);
        Task<List<Cryptocurrency>> GetAssets();
        Task<List<Cryptocurrency>> GetAssets(int limit);
        Task<List<Cryptocurrency>> GetSearchedAssets(string search);
        Task<List<History>> GetAssetHistory(string id, string interval, long start, long end);
        List<Candle> CreateCandlesFromHistory(List<History> historyData, Func<DateTime, DateTime> groupingStrategy);
    }
}
