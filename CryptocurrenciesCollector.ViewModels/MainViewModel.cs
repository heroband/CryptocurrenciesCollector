
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Models;
using CryptocurrenciesCollector.Models.Interfaces;
using CryptocurrenciesCollector.Services;
using System.Windows.Navigation;
using System.Windows.Media;
using CryptocurrenciesCollector.Models.Enums;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Reflection;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Reflection.Metadata;

namespace CryptocurrenciesCollector.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICryptocurrencyApiService cryptoService;
        private readonly INavigationService navigationService;

        const int topCryptocurrenciesNumber = 10;

        [ObservableProperty]
        private CryptocurrencyDetailedInfo cryptocurrencyInfo;

        [ObservableProperty]
        private bool hasMarkets = true;

        [ObservableProperty]
        private Cryptocurrency? selectedTopCryptocurrency;

        [ObservableProperty]
        private Cryptocurrency? selectedSearchCryptocurrency;

        [ObservableProperty]
        private string? searchText;

        [ObservableProperty]
        private string? inputAmount;

        [ObservableProperty]
        private string? outputAmount;

        [ObservableProperty]
        private Cryptocurrency convertFromCryptocurrency;

        [ObservableProperty]
        private Cryptocurrency convertToCryptocurrency;
       
        [ObservableProperty]
        private bool isCurrencyConvertAvailable;

        [ObservableProperty]
        private PlotModel? currentPlotModel;
        private bool _isPlotInitialized;

        public ObservableCollection<Cryptocurrency> SearchedCryptocurrencies { get; } = [];
        public ObservableCollection<Cryptocurrency> Cryptocurrencies { get; } = [];
        public ObservableCollection<Candle> CryptocurrencyCandles { get; } = [];


        private bool _isSortedAscending = true;

        private string interval;
        private long start;
        private long end;
        Func<DateTime, DateTime> strategy;
        private string AxisStringFormat;

        public MainViewModel(ICryptocurrencyApiService cryptoService, INavigationService navigationService)
        {
            this.cryptoService = cryptoService;
            this.navigationService = navigationService;
        }

        async partial void OnSelectedTopCryptocurrencyChanged(Cryptocurrency? value)
        {
            if (value != null)
            {
                await GetCryptocurrencyById(value.Id);
            }
        }

        async partial void OnSelectedSearchCryptocurrencyChanged(Cryptocurrency? value)
        {
            if (value != null)
            {
                await GetCryptocurrencyById(value.Id);
            }
        }

        private async Task GetCryptocurrencyById(string cryptocurrencyId)
        {
            var cryptocurrency = await cryptoService.GetAssetById(cryptocurrencyId);
            CryptocurrencyInfo = cryptocurrency;
            HasMarkets = CryptocurrencyInfo.Markets != null;

            interval = "h2";
            strategy = GroupByDay;
            AxisStringFormat = "dd/MM";

            start = new DateTimeOffset(DateTime.UtcNow.AddMonths(-1)).ToUnixTimeMilliseconds();
            end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            await GetCryptocurrencyHistory(cryptocurrencyId);
            navigationService.NavigateTo(NavigationPage.DetailInformation);
            InitializePlot();
        }

        private void InitializePlot()
        {
            if (CurrentPlotModel != null)
            {
                CurrentPlotModel.Series.Clear();
            }

            var plotModel = new PlotModel { Title = "Japanese candlestick chart" };

            var timeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                StringFormat = AxisStringFormat,
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };
            plotModel.Axes.Add(timeAxis);

            var priceAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Title = "Price (USD)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };
            plotModel.Axes.Add(priceAxis);

            var candleSeries = new CandleStickSeries
            {
                Title = "Candlestick",
                IncreasingColor = OxyColors.Green,
                DecreasingColor = OxyColors.Red
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

        [RelayCommand]
        public async Task GetTopAssets()
        {
            var topCryptocurrencies = await cryptoService.GetAssets(topCryptocurrenciesNumber);
            Cryptocurrencies.Clear();
            foreach (var cryptocurrency in topCryptocurrencies)
            {
                Cryptocurrencies.Add(cryptocurrency);
            }
        }

        [RelayCommand]
        private async Task GetAllCryptocurrencies()
        {
            Cryptocurrencies.Clear();
            InputAmount = OutputAmount = default;
            var cryptocurrencies = await cryptoService.GetAssets();
            foreach (var cryptocurrency in cryptocurrencies)
            {
                if (cryptocurrency.PriceUsd != 0)
                {
                    Cryptocurrencies.Add(cryptocurrency);
                }
            }
            InputAmount = "1";
            ConvertFromCryptocurrency = Cryptocurrencies[0];
            ConvertToCryptocurrency = Cryptocurrencies[0];
            IsCurrencyConvertAvailable = true;
        }

        [RelayCommand]
        public async Task GetSearchedCryptocurrencies()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var allCryptocurrencies = await cryptoService.GetSearchedAssets(SearchText);

                SearchedCryptocurrencies.Clear();
                foreach (var cryptocurrency in allCryptocurrencies)
                {
                    SearchedCryptocurrencies.Add(cryptocurrency);
                }
                navigationService.NavigateTo(NavigationPage.Search);
            }
        }

        [RelayCommand]
        private void NavigateTo(NavigationPage navigationPage)
        {
            navigationService.NavigateTo(navigationPage);
        }

        [RelayCommand]
        private void SortByMarketPrice()
        {
            if (CryptocurrencyInfo != null)
            {
                var collectionView = CollectionViewSource.GetDefaultView(CryptocurrencyInfo.Markets);
                if (collectionView != null)
                {
                    collectionView.SortDescriptions.Clear();

                    if (_isSortedAscending)
                    {
                        collectionView.SortDescriptions.Add(new SortDescription("PriceUsd", ListSortDirection.Ascending));
                    }
                    else
                    {
                        collectionView.SortDescriptions.Add(new SortDescription("PriceUsd", ListSortDirection.Descending));
                    }

                    _isSortedAscending = !_isSortedAscending;
                    collectionView.Refresh();
                }
            }
        }

        [RelayCommand]
        private void ConvertCurrency()
        {
            if (decimal.TryParse(InputAmount, CultureInfo.InvariantCulture, out var inputValue))
            {
                var fromPriceUsd = ConvertFromCryptocurrency.PriceUsd;
                var toPriceUsd = ConvertToCryptocurrency.PriceUsd;
                try
                {
                    OutputAmount = ((inputValue * fromPriceUsd) / toPriceUsd).ToString(CultureInfo.InvariantCulture);
                }
                catch (OverflowException) {
                    MessageBox.Show("The entered value is too large to convert.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                    OutputAmount = null;
                }
            }
        }

        partial void OnInputAmountChanged(string? oldValue, string? newValue)
        {
            if (ConvertFromCryptocurrency != null && ConvertToCryptocurrency != null)
            {
                ConvertCurrency();
            }
        }

        partial void OnConvertFromCryptocurrencyChanged(Cryptocurrency value)
        {
            IsCurrencyConvertAvailable = false;
            if (value != null && Cryptocurrencies.Contains(value) && ConvertToCryptocurrency != null)
            {
                IsCurrencyConvertAvailable = true;
                ConvertCurrency();
            }
            else
            {
                IsCurrencyConvertAvailable = false;
            }
        }

        partial void OnConvertToCryptocurrencyChanged(Cryptocurrency value)
        {
            IsCurrencyConvertAvailable = false;
            if (value != null && Cryptocurrencies.Contains(value) && ConvertFromCryptocurrency != null)
            {
                IsCurrencyConvertAvailable = true;
                ConvertCurrency();
            }
            else
            {
                IsCurrencyConvertAvailable = false;
            }
        }

        public async Task GetCryptocurrencyHistory(string cryptocurrencyId)
        {
            var assetHistory = await cryptoService.GetAssetHistory(cryptocurrencyId, interval, start, end);
            var candles = cryptoService.CreateCandlesFromHistory(assetHistory, strategy);
            CryptocurrencyCandles.Clear();
            foreach(var candle in candles)
            {
                CryptocurrencyCandles.Add(candle);
            }
            Debug.WriteLine("*");
        }


        [RelayCommand]
        private async Task GetCandlesWith1DayInterval()
        {
            interval = "m1";
            start = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)).ToUnixTimeMilliseconds();
            end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            strategy = GroupByHour;
            AxisStringFormat = "HH:mm\ndd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id);
            InitializePlot();
        }
        [RelayCommand]
        private async Task GetCandlesWith7DaysInterval()
        {
            interval = "m30";
            start = new DateTimeOffset(DateTime.UtcNow.AddDays(-7)).ToUnixTimeMilliseconds();
            end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            strategy = GroupBy6Hours;
            AxisStringFormat = "dd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id);
            InitializePlot();
        }
        [RelayCommand]
        private async Task GetCandlesWith1MonthInterval()
        {
            interval = "h2";
            start = new DateTimeOffset(DateTime.UtcNow.AddMonths(-1)).ToUnixTimeMilliseconds();
            end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            strategy = GroupByDay;
            AxisStringFormat = "dd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id);
            InitializePlot();
        }
        [RelayCommand]
        private async Task GetCandlesWith3MonthsInterval()
        {
            interval = "h6";
            start = new DateTimeOffset(DateTime.UtcNow.AddMonths(-3)).ToUnixTimeMilliseconds();
            end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            strategy = GroupByDay;
            AxisStringFormat = "dd/MM";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id);
            InitializePlot();
        }
        [RelayCommand]
        private async Task GetCandlesWith1YearInterval()
        {
            interval = "h12";
            start = new DateTimeOffset(DateTime.UtcNow.AddDays(2).AddYears(-1)).ToUnixTimeMilliseconds();
            end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            strategy = GroupByDay;
            AxisStringFormat = "dd/MM\nyyyy";

            await GetCryptocurrencyHistory(CryptocurrencyInfo.Id);
            InitializePlot();
        }


        private DateTime GroupByHour(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }

        private DateTime GroupBy6Hours(DateTime time)
        {
            int hourBlock = time.Hour / 6 * 6;
            return new DateTime(time.Year, time.Month, time.Day, hourBlock, 0, 0);
        }

        private DateTime GroupByDay(DateTime time)
        {
            return time.Date;
        }
    }
}
