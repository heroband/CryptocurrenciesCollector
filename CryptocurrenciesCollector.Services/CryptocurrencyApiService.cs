using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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

            var assetMarketsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}/markets");
            assetMarketsResponse.EnsureSuccessStatusCode();
            var assetMarketsJson = await assetMarketsResponse.Content.ReadAsStringAsync();
            var assetMarkets = JsonSerializer.Deserialize<AssetsWrap<List<MarketPriceData>>>(assetMarketsJson);

            return asset.ToCryptocurrency(assetMarkets);
        }

        public async Task<List<TopCryptocurrencies>> GetTopAssets()
        {
            var assetsResponse = await _httpClient.GetAsync("https://api.coincap.io/v2/assets");
            assetsResponse.EnsureSuccessStatusCode();
            var assetsJson = await assetsResponse.Content.ReadAsStringAsync();
            var assets = JsonSerializer.Deserialize<AssetsWrap<List<TopCryptocurrenciesData>>>(assetsJson);

            return assets.Data.Select(asset => new TopCryptocurrencies
            {
                Id = asset.Id,
                Name = asset.Name,
                Rank = int.Parse(asset.Rank)
            }).ToList();
        }

        public async Task<List<CryptocurrenciesSearchIInfo>> GetSearchedAssets(string search)
        {
            var assetsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets?search={search}");
            assetsResponse.EnsureSuccessStatusCode();
            var assetsJson = await assetsResponse.Content.ReadAsStringAsync();
            var assets = JsonSerializer.Deserialize<AssetsWrap<List<CryptocurrencySearchData>>>(assetsJson);

            return assets.Data.Select(asset => new CryptocurrenciesSearchIInfo
            {
                Id = asset.Id,
                Name = asset.Name,
                PriceUsd = decimal.Parse(asset.PriceUsd, CultureInfo.InvariantCulture), 
                ChangePercent24Hr = string.IsNullOrEmpty(asset.ChangePercent24Hr)
                                        ? 0
                                        : decimal.Parse(asset.ChangePercent24Hr, CultureInfo.InvariantCulture)
            }).ToList();
        }
    }
}
