using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CryptocurrenciesCollector.Models;
using CryptocurrenciesCollector.Models.Extensions;
using CryptocurrenciesCollector.Models.Interfaces;
using CryptocurrenciesCollector.Models.Responses;


namespace CryptocurrenciesCollector.Services
{
    public class CryptocurrencyApiService : ICryptocurrencyApiService
    {
        private readonly HttpClient _httpClient;

        public CryptocurrencyApiService(string apiKey)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<CryptocurrencyDetailedInfo> GetAssetById(string id)
        {
            var assetResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}");
            assetResponse.EnsureSuccessStatusCode();
            var assetJson = await assetResponse.Content.ReadAsStringAsync();
            var asset = JsonSerializer.Deserialize<AssetsWrap<CryptocurrencyDetailedData>>(assetJson);

            var assetMarketsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}/markets?limit=2000");
            AssetsWrap<List<MarketPriceData>>? assetMarkets;

            if (assetMarketsResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                assetMarkets = null;
            }
            else 
            {
                var assetMarketsJson = await assetMarketsResponse.Content.ReadAsStringAsync();
                assetMarkets = JsonSerializer.Deserialize<AssetsWrap<List<MarketPriceData>>>(assetMarketsJson);
            }
            
            return asset.ToDetailedInfoCryptocurrency(assetMarkets);
        }

        public async Task<List<Cryptocurrency>> GetAssets(int limit)
        {
            var assetsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets?limit={limit}");
            assetsResponse.EnsureSuccessStatusCode();
            var assetsJson = await assetsResponse.Content.ReadAsStringAsync();
            var assets = JsonSerializer.Deserialize<AssetsWrap<List<CryptocurrencyData>>>(assetsJson);

            return assets.ToCryptocurrencies();
        }

        public async Task<List<Cryptocurrency>> GetSearchedAssets(string search, int limit)
        {
            var assetsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets?search={search}&limit={limit}");
            assetsResponse.EnsureSuccessStatusCode();
            var assetsJson = await assetsResponse.Content.ReadAsStringAsync();
            var assets = JsonSerializer.Deserialize<AssetsWrap<List<CryptocurrencyData>>>(assetsJson);

            return assets.ToCryptocurrencies();
        }
    }
}
