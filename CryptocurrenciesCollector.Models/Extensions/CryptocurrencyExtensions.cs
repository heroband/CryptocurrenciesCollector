using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptocurrenciesCollector.Models.Responses;

namespace CryptocurrenciesCollector.Models.Extensions
{
    public static class CryptocurrencyExtensions
    {
        public static CryptocurrencyDetailedInfo ToDetailedInfoCryptocurrency(this AssetsWrap<CryptocurrencyDetailedData> asset, AssetsWrap<List<MarketPriceData>>? assetMarkets)
        {
            return new CryptocurrencyDetailedInfo
            {
                Id = asset.Data.Id,
                Name = asset.Data.Name,
                PriceUsd = decimal.TryParse(asset.Data.PriceUsd, CultureInfo.InvariantCulture, out decimal price) ? price : 0,
                ChangePercent24Hr = decimal.TryParse(asset.Data.ChangePercent24Hr, CultureInfo.InvariantCulture, out decimal change) ? change : 0,
                VolumeUsd24Hr = decimal.TryParse(asset.Data.VolumeUsd24Hr, CultureInfo.InvariantCulture, out decimal volume) ? volume : 0,
                Markets = assetMarkets?.Data
                    .Select(m => new MarketPrice
                    {
                        ExchangeId = m.ExchangeId,
                        PriceUsd = decimal.TryParse(m.PriceUsd, CultureInfo.InvariantCulture, out decimal price) ? price : 0
                    })
                    .ToList(),
            };
        }

        public static List<Cryptocurrency> ToCryptocurrencies(this AssetsWrap<List<CryptocurrencyData>> assets)
        {
            return assets.Data
                .Select(asset => new Cryptocurrency
                {
                    Id = asset.Id,
                    Rank = int.Parse(asset.Rank),
                    Name = asset.Name,
                    PriceUsd = decimal.TryParse(asset.PriceUsd, CultureInfo.InvariantCulture, out decimal price) ? price : 0,
                    ChangePercent24Hr = decimal.TryParse(asset.ChangePercent24Hr, CultureInfo.InvariantCulture, out decimal change) ? change : 0
                })
                .ToList();
        }

        public static List<History> ToCryptocurrencyHistory(this AssetsWrap<List<HistoryData>> assetInfo)
        {
            return assetInfo.Data
                .Select(info => new History
                {
                    PriceUsd = decimal.TryParse(info.PriceUsd, CultureInfo.InvariantCulture, out decimal price) ? price : 0,
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(info.Time),
                    CirculatingSupply = info.CirculatingSupply,
                    Date = info.Date,
                })
                .ToList();
        }

        public static List<Candle> ToCandles(this List<History> historyData, Func<DateTime, DateTime> groupingStrategy)
        {
            var groupedData = historyData.GroupBy(h => groupingStrategy(h.Time.UtcDateTime)).ToList();

            var candles = new List<Candle>();

            foreach (var groupData in groupedData)
            {
                var candle = new Candle
                {
                    Time = groupData.Key,
                    Open = groupData.First().PriceUsd,
                    Close = groupData.Last().PriceUsd,
                    High = groupData.Max(h => h.PriceUsd),
                    Low = groupData.Min(h => h.PriceUsd)
                };

                candles.Add(candle);
            }
            return candles;
        }
    }
}
