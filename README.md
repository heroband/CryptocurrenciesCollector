# CryptocurrenciesCollector
Made a multi-page application with mvvm architecture, which allows you to see different information about cryptocurrencies.

1. **The basic page** provides the top 10 cryptocurrencies by rank from the API, when you click on one of them, a detailed information page is loaded.

2. **It is possible to find** a cryptocurrency: you need to enter the name, then click the search button. Next, a list of cryptocurrencies found by this request will be displayed. When you click on one of them, a detailed information page is loaded.

3. **The detailed information page** contains: *price*, *volume*, *price change*, *in which markets it can be purchased and at what price* (you can also **sort in ascending and descending** order). If no markets were found, then a corresponding message will be displayed. Also on this page, a cryptocurrency **Japanese candlestick chart** is built, the candle display interval of which the user can change to any of the available ones.

4. **Conversion page**: it is possible to convert one cryptocurrency to another. The cryptocurrency can be **selected from the list** or by **entering its name**. If you enter an incorrect cryptocurrency name, a corresponding message will be displayed. The value to be converted is validated, so text input is not available.

5. **Settings page**: allows you to select the theme and localization of the application. Available themes: **dark** (basic) and **light**. Available languages: **English** (basic) and **Ukrainian**.