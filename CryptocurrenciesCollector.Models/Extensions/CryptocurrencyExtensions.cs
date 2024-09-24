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
                Name = asset.Data.Name,
                PriceUsd = decimal.Parse(asset.Data.PriceUsd, CultureInfo.InvariantCulture),
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

        public static List<TopCryptocurrencies> ToTopCryptocurrencies(this AssetsWrap<List<TopCryptocurrenciesData>> assets)
        {
            return assets.Data
                .Select(asset => new TopCryptocurrencies
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    Rank = int.Parse(asset.Rank)
                })
                .ToList();
        }

        public static List<CryptocurrencySearchIInfo> ToShortInfoCryptocurrency(this AssetsWrap<List<CryptocurrencySearchData>> assets)
        {
            return assets.Data
                .Select(asset => new CryptocurrencySearchIInfo
                {
                
                    Id = asset.Id,
                    Name = asset.Name,
                    PriceUsd = decimal.TryParse(asset.PriceUsd, CultureInfo.InvariantCulture, out decimal price) ? price : 0,
                    ChangePercent24Hr = decimal.TryParse(asset.ChangePercent24Hr, CultureInfo.InvariantCulture, out decimal change) ? change : 0
                })
                .ToList();
        }
    }
}
