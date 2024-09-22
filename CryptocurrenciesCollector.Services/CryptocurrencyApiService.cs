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

        public async Task<Cryptocurrency> GetAssetById(string id)
        {
            var assetResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}");
            assetResponse.EnsureSuccessStatusCode();
            var assetJson = await assetResponse.Content.ReadAsStringAsync();
            var asset = JsonSerializer.Deserialize<CryptocurrencyAsset>(assetJson);

            var assetMarketsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}/markets");
            assetMarketsResponse.EnsureSuccessStatusCode();
            var assetMarketsJson = await assetMarketsResponse.Content.ReadAsStringAsync();
            var assetMarkets = JsonSerializer.Deserialize<MarketAsset>(assetMarketsJson);

            return asset.ToCryptocurrency(assetMarkets);
        }

        public async Task<List<TopCryptocurrencies>> GetTopAssets()
        {
            var assetsResponse = await _httpClient.GetAsync("https://api.coincap.io/v2/assets");
            assetsResponse.EnsureSuccessStatusCode();
            var assetsJson = await assetsResponse.Content.ReadAsStringAsync();
            var assets = JsonSerializer.Deserialize<CryptocurrencyAssets<TopCryptocurrenciesData>>(assetsJson);

            return assets.Data.Select(asset => new TopCryptocurrencies
            {
                Id = asset.Id,
                Name = asset.Name,
                Rank = int.Parse(asset.Rank)
            }).ToList();
        }

        public async Task<List<ShortInfoCryptocurrency>> GetAllAssets()
        {
            var assetsResponse = await _httpClient.GetAsync("https://api.coincap.io/v2/assets");
            assetsResponse.EnsureSuccessStatusCode();
            var assetsJson = await assetsResponse.Content.ReadAsStringAsync();
            var assets = JsonSerializer.Deserialize<CryptocurrencyAssets<CryptocurrencyShortData>>(assetsJson);

            return assets.Data.Select(asset => new ShortInfoCryptocurrency
            {
                Id = asset.Id,
                Name = asset.Name,
                PriceUsd = decimal.Parse(asset.PriceUsd, CultureInfo.InvariantCulture), 
                ChangePercent24Hr = decimal.Parse(asset.ChangePercent24Hr, CultureInfo.InvariantCulture)
            }).ToList();
        }
    }
}
