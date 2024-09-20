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
        public static Cryptocurrency ToCryptocurrency(this Asset asset, AssetMarket assetMarkets)
        {
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
