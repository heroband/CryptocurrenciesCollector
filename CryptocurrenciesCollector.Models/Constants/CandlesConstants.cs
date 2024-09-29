using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Constants
{
    public static class CandlesConstants
    {
        public static DateTime GroupByHour(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }

        public static DateTime GroupBy6Hours(DateTime time)
        {
            int hourBlock = time.Hour / 6 * 6;
            return new DateTime(time.Year, time.Month, time.Day, hourBlock, 0, 0);
        }

        public static DateTime GroupByDay(DateTime time)
        {
            return time.Date;
        }
    }
}
