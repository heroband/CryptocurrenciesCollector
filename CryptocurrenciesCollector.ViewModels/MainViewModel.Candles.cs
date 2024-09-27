using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Models.Constants;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System.Windows;
using CryptocurrenciesCollector.Models;
using System.Collections.ObjectModel;
using CryptocurrenciesCollector.Models.Extensions;

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<Candle> CryptocurrencyCandles { get; } = [];

        [RelayCommand]
        private async Task GetCandlesWith1DayInterval()
        {
            var interval = "m1";
            var start = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)).ToUnixTimeMilliseconds();
            var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            var strategy = CandlesConstants.GroupByHour;
            var axisStringFormat = "HH:mm\ndd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id, interval, start, end, strategy);
            InitializePlot(axisStringFormat);
        }
        [RelayCommand]
        private async Task GetCandlesWith7DaysInterval()
        {
            var interval = "m30";
            var start = new DateTimeOffset(DateTime.UtcNow.AddDays(-7)).ToUnixTimeMilliseconds();
            var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            var strategy = CandlesConstants.GroupBy6Hours;
            var axisStringFormat = "dd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id, interval, start, end, strategy);
            InitializePlot(axisStringFormat);
        }
        [RelayCommand]
        private async Task GetCandlesWith1MonthInterval()
        {
            var interval = "h2";
            var start = new DateTimeOffset(DateTime.UtcNow.AddMonths(-1)).ToUnixTimeMilliseconds();
            var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            var strategy = CandlesConstants.GroupByDay;
            var axisStringFormat = "dd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id, interval, start, end, strategy);
            InitializePlot(axisStringFormat);
        }
        [RelayCommand]
        private async Task GetCandlesWith3MonthsInterval()
        {
            var interval = "h6";
            var start = new DateTimeOffset(DateTime.UtcNow.AddMonths(-3)).ToUnixTimeMilliseconds();
            var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            var strategy = CandlesConstants.GroupByDay;
            var axisStringFormat = "dd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id, interval, start, end, strategy);
            InitializePlot(axisStringFormat);
        }
        [RelayCommand]
        private async Task GetCandlesWith1YearInterval()
        {
            var interval = "h12";
            var start = new DateTimeOffset(DateTime.UtcNow.AddDays(2).AddYears(-1)).ToUnixTimeMilliseconds();
            var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            var strategy = CandlesConstants.GroupByDay;
            var axisStringFormat = "dd/MM\nyyyy";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id, interval, start, end, strategy);
            InitializePlot(axisStringFormat);
        }

        private void InitializePlot(string axisStringFormat)
        {
            
            CurrentPlotModel?.Series.Clear();
            
            var backdropBrush = (SolidColorBrush)Application.Current.Resources["Backdrop"];
            var textBrush = (SolidColorBrush)Application.Current.Resources["Text"];
            var cardBrush = (SolidColorBrush)Application.Current.Resources["Card"];

            var oxyBackdropColor = OxyColor.FromArgb(backdropBrush.Color.A, backdropBrush.Color.R, backdropBrush.Color.G, backdropBrush.Color.B);
            var oxyTextColor = OxyColor.FromArgb(textBrush.Color.A, textBrush.Color.R, textBrush.Color.G, textBrush.Color.B);
            var oxyCardColor = OxyColor.FromArgb(cardBrush.Color.A, cardBrush.Color.R, cardBrush.Color.G, cardBrush.Color.B);

            var plotModel = new PlotModel
            {
                Title = "Japanese candlestick chart",
                TitleColor = oxyTextColor,
            };

            var timeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                StringFormat = axisStringFormat,
                Title = "Date",
                TitleColor = oxyTextColor,
                IntervalType = DateTimeIntervalType.Days,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = oxyCardColor,
                MinorGridlineColor = oxyCardColor,
                TicklineColor = oxyTextColor,
                TextColor = oxyTextColor,
            };
            plotModel.Axes.Add(timeAxis);


            var priceAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Title = "Price (USD)",
                TitleColor = oxyTextColor,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = oxyCardColor,
                MinorGridlineColor = oxyCardColor,
                TicklineColor = oxyTextColor,
                TextColor = oxyTextColor,
            };
            plotModel.Axes.Add(priceAxis);

            var candleSeries = new CandleStickSeries
            {
                Title = "Candlestick",
                IncreasingColor = OxyColors.Green,
                DecreasingColor = OxyColors.Red,
            };

            candleSeries.Items.Clear();
            foreach (var candle in CryptocurrencyCandles)
            {
                candleSeries.Items.Add(new HighLowItem
                {
                    X = DateTimeAxis.ToDouble(candle.Time.DateTime),
                    Open = Convert.ToDouble(candle.Open),
                    High = Convert.ToDouble(candle.High),
                    Low = Convert.ToDouble(candle.Low),
                    Close = Convert.ToDouble(candle.Close)
                });
            }

            plotModel.Series.Add(candleSeries);

            CurrentPlotModel = plotModel;
        }

        private async Task GetCryptocurrencyHistory(string cryptocurrencyId, string interval, long start, long end, Func<DateTime, DateTime> strategy)
        {
            var assetHistory = await cryptoService.GetAssetHistory(cryptocurrencyId, interval, start, end);
            var candles = assetHistory.ToCandles(strategy);
            CryptocurrencyCandles.Clear();
            foreach (var candle in candles)
            {
                CryptocurrencyCandles.Add(candle);
            }
        }
    }
}
