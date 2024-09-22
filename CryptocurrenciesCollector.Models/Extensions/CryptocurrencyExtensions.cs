using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptocurrenciesCollector.Models.Responses;

namespace CryptocurrenciesCollector.Models.Extensions
{
    public static class CryptocurrencyExtensions
    {
        public static CryptocurrencyDetailedInfo ToCryptocurrency(this AssetsWrap<CryptocurrencyDetailedData> asset, AssetsWrap<List<MarketPriceData>> assetMarkets)
        {
            return new CryptocurrencyDetailedInfo
            {
                Name = asset.Data.Name,
                PriceUsd = decimal.Parse(asset.Data.PriceUsd, CultureInfo.InvariantCulture),
                ChangePercent24Hr = string.IsNullOrEmpty(asset.Data.ChangePercent24Hr)
                                        ? 0 
                                        : decimal.Parse(asset.Data.ChangePercent24Hr, CultureInfo.InvariantCulture),
                VolumeUsd24Hr = string.IsNullOrEmpty(asset.Data.VolumeUsd24Hr)
                                    ? 0
                                    : decimal.Parse(asset.Data.VolumeUsd24Hr, CultureInfo.InvariantCulture),
                Markets = assetMarkets.Data
                    .Select(m => new MarketPrice
                    {
                        ExchangeId = m.ExchangeId,
                        PriceUsd = decimal.Parse(m.PriceUsd, CultureInfo.InvariantCulture)
                    })
                    .ToList(),
            };
        }
    }
}
