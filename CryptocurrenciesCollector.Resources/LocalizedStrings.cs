using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace CryptocurrenciesCollector.Resources
{
    public class LocalizedStrings
    {
        public static LocalizedStrings Instanse { get; } = new LocalizedStrings();

        public void SetCulture(string cultureInfo)
        {
            var newCulture = new CultureInfo(cultureInfo);
            LocalizeDictionary.Instance.Culture = newCulture;
        }
        public string? this[string key]
        {
            get
            {
                var result = LocalizeDictionary.Instance.GetLocalizedObject("CryptocurrenciesCollector.Resources", "Language", key, LocalizeDictionary.Instance.Culture);
                return result as string;
            }
        }
    }
}
