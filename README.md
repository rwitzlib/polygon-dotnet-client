# polygon-dotnet-client
![Build](https://github.com/rwitzlib/polygon-dotnet-client/actions/workflows/nuget-package-publish.yml/badge.svg?event=push)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

![image](https://github.com/rwitzlib/polygon-dotnet-client/blob/master/docs/polygon_icon.png)

.NET client for getting stocks, crypto, forex, and indices data from Polygon.io API.

<b>
Independently developed, this is not an official library and I am not affiliated with Polygon.
</b>

## Prerequisites
Create an account with [Polygon](https://www.polygon.io).  Upon account creation, an API key will be provided for you.

You can manage your API keys on the [Dashboard](https://polygon.io/dashboard/api-keys)

## Setup

Install the [Polygon.Client](http://nuget.org/packages/polygon.client) NuGet package 

Package Manager `PM > Install-Package Polygon.Client`

Using the API key that was generated for you earlier, you can create a Polygon client in the following ways.

#### Without Dependency Injection

```c#
using var client = new PolygonClient("API KEY GOES HERE");
```

#### With Dependency Injection

```c#
services.AddPolygonClient("API KEY GOES HERE");
```

## Usage

```c#
public class SomeClass
{
    private readonly IPolygonClient _polygonClient;

    public SomeClass(IPolygonClient polygonClient)
    {
        _polygonClient = polygonClient;
    }

    public async Task<PolygonAggregateResponse> DoSomething()
    {
        // This will get all of the 1-minute bars for 2024-04-20

        var request = new PolygonAggregatesRequest
        {
            Ticker = "SPY",
            Multuplier = 1
            Timespan = "minute",
            From = "2024-04-20",
            To = "2024-04-20"
        };
        
        var response = await _polygonClient.GetAggregatesAsync(request);

        return response;
    }
}
```
## Supported Endpoints

| Market Data Endpoints | Supported? |
| - | - |
| [Aggregates](https://polygon.io/docs/stocks/get_v2_aggs_ticker__stocksticker__range__multiplier___timespan___from___to) | ✔️ |
| [Grouped Daily](https://polygon.io/docs/stocks/get_v2_aggs_grouped_locale_us_market_stocks__date) | ✔️ |
| [Daily Open/Close](https://polygon.io/docs/stocks/get_v1_open-close__stocksticker___date) | ❌ |
| [Previous Close](https://polygon.io/docs/stocks/get_v2_aggs_ticker__stocksticker__prev) | ❌|
| [Trades](https://polygon.io/docs/stocks/get_v3_trades__stockticker) | ❌ |
| [Last Trade](https://polygon.io/docs/stocks/get_v2_last_trade__stocksticker) | ❌ |
| [Quotes (NBBO)](https://polygon.io/docs/stocks/get_v3_quotes__stockticker) | ❌ |
| [Last Quote](https://polygon.io/docs/stocks/get_v2_last_nbbo__stocksticker) | ❌ |
| [Snapshot - All Tickers](https://polygon.io/docs/stocks/get_v2_snapshot_locale_us_markets_stocks_tickers) | ✔️ |
| [Snapshot - Gainers/Losers](https://polygon.io/docs/stocks/get_v2_snapshot_locale_us_markets_stocks__direction) | ❌ |
| [Snapshot - Ticker](https://polygon.io/docs/stocks/get_v2_snapshot_locale_us_markets_stocks_tickers__stocksticker) | ❌ |
| [Snapshot - Universal](https://polygon.io/docs/stocks/get_v3_snapshot) | ❌ |
| [Technical Indicators - SMA](https://polygon.io/docs/stocks/get_v1_indicators_sma__stockticker) | ❌ |
| [Technical Indicators - EMA](https://polygon.io/docs/stocks/get_v1_indicators_ema__stockticker) | ❌ |
| [Technical Indicators - MACD](https://polygon.io/docs/stocks/get_v1_indicators_macd__stockticker) | ❌ |
| [Technical Indicators - RSI](https://polygon.io/docs/stocks/get_v1_indicators_rsi__stockticker) | ❌ |

| Reference Data Endpoints | Supported? |
| - | - |
| [Tickers](https://polygon.io/docs/stocks/get_v3_reference_tickers) | ✔️ |
| [Ticker Details V3](https://polygon.io/docs/stocks/get_v3_reference_tickers__ticker) | ✔️ |
| [Ticker Events](https://polygon.io/docs/stocks/get_vx_reference_tickers__id__events) | ❌ |
| [Ticker News](https://polygon.io/docs/stocks/get_v2_reference_news) | ❌ |
| [Ticker Types](https://polygon.io/docs/stocks/get_v3_reference_tickers_types) | ❌ |
| [Market Holidays](https://polygon.io/docs/stocks/get_v1_marketstatus_upcoming) | ❌ |
| [Market Status](https://polygon.io/docs/stocks/get_v1_marketstatus_now) | ❌ |
| [Stock Splits V3](https://polygon.io/docs/stocks/get_v3_reference_splits) | ❌ |
| [Dividends V3](https://polygon.io/docs/stocks/get_v3_reference_dividends) | ❌ |
| [Stock Financials VX](https://polygon.io/docs/stocks/get_vx_reference_financials) | ❌ |
| [Conditions](https://polygon.io/docs/stocks/get_v3_reference_conditions) | ❌ |
| [Exchanges](https://polygon.io/docs/stocks/get_v3_reference_exchanges) | ❌ |
