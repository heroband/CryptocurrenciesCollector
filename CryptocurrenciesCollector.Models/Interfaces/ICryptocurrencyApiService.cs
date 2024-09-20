using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Interfaces
{
    public interface ICryptocurrencyApiService
    {
        Task<Cryptocurrency> GetAssetById(string id);
        Task<List<TopCryptocurrencies>> GetAssets();
    }
}
