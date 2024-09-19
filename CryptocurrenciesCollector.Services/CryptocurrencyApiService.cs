using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CryptocurrenciesCollector.Models;
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
            var asset = JsonSerializer.Deserialize<AssetResponse>(assetJson);

            var assetMarketsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}/markets");
            assetMarketsResponse.EnsureSuccessStatusCode();
            var assetMarketsJson = await assetMarketsResponse.Content.ReadAsStringAsync();
            var assetMarkets = JsonSerializer.Deserialize<AssetMarketsResponse>(assetMarketsJson);


            return new Cryptocurrency
            {
                Name = asset.Data.Name,
                PriceUsd = decimal.Parse(asset.Data.PriceUsd, CultureInfo.InvariantCulture),
                ChangePercent24Hr = decimal.Parse(asset.Data.ChangePercent24Hr, CultureInfo.InvariantCulture),
                VolumeUsd24Hr = decimal.Parse(asset.Data.VolumeUsd24Hr, CultureInfo.InvariantCulture),
                Markets = assetMarkets.Data
                    .Select(m => new Market
                    {
                        ExchangeId = m.ExchangeId,
                        PriceUsd = decimal.Parse(m.PriceUsd, CultureInfo.InvariantCulture)
                    })
                    .ToList(),
            };
        }
    }
}
