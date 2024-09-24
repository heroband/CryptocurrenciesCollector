using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Interfaces
{
    public interface ICryptocurrencyApiService
    {
        Task<CryptocurrencyDetailedInfo> GetAssetById(string id);
        Task<List<Cryptocurrency>> GetAssets(int limit);
        Task<List<Cryptocurrency>> GetSearchedAssets(string search, int limit);
    }
}
