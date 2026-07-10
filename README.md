# massive-dotnet-client
![Build](https://github.com/rwitzlib/polygon-dotnet-client/actions/workflows/nuget-package-publish.yml/badge.svg?event=push)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

.NET client for Massive stocks REST APIs.

Massive was formerly Polygon.io. This package has been renamed from `Polygon.Client` to `Massive.Client` as a breaking v3.0.0 release.

<b>
Independently developed, this is not an official library and I am not affiliated with Massive.
</b>

## Prerequisites
Create an account with [Massive](https://www.massive.com). Upon account creation, an API key will be provided for you.

You can manage your API keys on the [Dashboard](https://massive.com/dashboard/api-keys).

## Setup

Install the [Massive.Client](https://www.nuget.org/packages/Massive.Client) NuGet package.

Package Manager `PM > Install-Package Massive.Client`

Using the API key that was generated for you earlier, you can create a Massive client in the following ways.

#### Without Dependency Injection

```c#
using var client = new MassiveClient("API KEY GOES HERE");
```

#### With Dependency Injection

```c#
services.AddMassiveClient("API KEY GOES HERE");
```

By default, the client uses `https://api.massive.com`. To target the legacy Polygon URL, pass a base URL override:

```c#
using var client = new MassiveClient("API KEY GOES HERE", "https://api.polygon.io");

services.AddMassiveClient("API KEY GOES HERE", "https://api.polygon.io");
```

## Usage

```c#
public class SomeClass
{
    private readonly IMassiveClient _massiveClient;

    public SomeClass(IMassiveClient massiveClient)
    {
        _massiveClient = massiveClient;
    }

    public async Task<MassiveAggregateResponse> DoSomething()
    {
        // This will get all of the 1-minute bars for 2024-04-20

        var request = new MassiveAggregateRequest
        {
            Ticker = "SPY",
            Multiplier = 1,
            Timespan = "minute",
            From = "2024-04-20",
            To = "2024-04-20"
        };
        
        var response = await _massiveClient.GetAggregates(request);

        return response;
    }
}
```

## Migration from Polygon.Client

`Massive.Client` v3.0.0 is a breaking rebrand with no dual-package shim.

- Replace the NuGet package `Polygon.Client` with `Massive.Client`.
- Replace namespaces from `Polygon.Client.*` to `Massive.Client.*`.
- Replace `IPolygonClient` / `PolygonClient` with `IMassiveClient` / `MassiveClient`.
- Replace `AddPolygonClient` with `AddMassiveClient`.
- Replace public request/response DTOs from `Polygon*Request` / `Polygon*Response` to `Massive*Request` / `Massive*Response`.
- Rename environment variables and CI secrets from `POLYGON_TOKEN` to `MASSIVE_TOKEN`.
- The default REST base URL is now `https://api.massive.com`. Pass `https://api.polygon.io` as the optional base URL if you still need the legacy host.

## Supported Endpoints

| Market Data Endpoints | Supported? |
| - | - |
| [Aggregates](https://massive.com/docs/rest/stocks/aggregates) | Supported |
| [Grouped Daily](https://massive.com/docs/rest/stocks/aggregates/grouped-daily) | Supported |
| [Daily Open/Close](https://massive.com/docs/rest/stocks/open-close) | Not supported |
| [Previous Close](https://massive.com/docs/rest/stocks/aggregates/previous-close) | Not supported |
| [Trades](https://massive.com/docs/rest/stocks/trades-quotes/trades) | Not supported |
| [Last Trade](https://massive.com/docs/rest/stocks/trades-quotes/last-trade) | Not supported |
| [Quotes (NBBO)](https://massive.com/docs/rest/stocks/trades-quotes/quotes) | Not supported |
| [Last Quote](https://massive.com/docs/rest/stocks/trades-quotes/last-quote) | Not supported |
| [Snapshot - All Tickers](https://massive.com/docs/rest/stocks/snapshots/all-tickers) | Supported |
| [Snapshot - Gainers/Losers](https://massive.com/docs/rest/stocks/snapshots/gainers-losers) | Not supported |
| [Snapshot - Ticker](https://massive.com/docs/rest/stocks/snapshots/ticker) | Not supported |
| [Snapshot - Universal](https://massive.com/docs/rest/stocks/snapshots/universal) | Not supported |
| [Technical Indicators - SMA](https://massive.com/docs/rest/stocks/technical-indicators/simple-moving-average) | Not supported |
| [Technical Indicators - EMA](https://massive.com/docs/rest/stocks/technical-indicators/exponential-moving-average) | Not supported |
| [Technical Indicators - MACD](https://massive.com/docs/rest/stocks/technical-indicators/macd) | Not supported |
| [Technical Indicators - RSI](https://massive.com/docs/rest/stocks/technical-indicators/relative-strength-index) | Not supported |

| Reference Data Endpoints | Supported? |
| - | - |
| [Tickers](https://massive.com/docs/rest/stocks/tickers) | Supported |
| [Ticker Details V3](https://massive.com/docs/rest/stocks/tickers/ticker-details) | Supported |
| [Ticker Events](https://massive.com/docs/rest/stocks/tickers/events) | Not supported |
| [Ticker News](https://massive.com/docs/rest/stocks/news) | Not supported |
| [Ticker Types](https://massive.com/docs/rest/stocks/tickers/types) | Not supported |
| [Market Holidays](https://massive.com/docs/rest/stocks/market-operations/market-holidays) | Not supported |
| [Market Status](https://massive.com/docs/rest/stocks/market-operations/market-status) | Not supported |
| [Stock Splits V3](https://massive.com/docs/rest/stocks/corporate-actions/splits) | Not supported |
| [Dividends V3](https://massive.com/docs/rest/stocks/corporate-actions/dividends) | Not supported |
| [Stock Financials VX](https://massive.com/docs/rest/stocks/fundamentals/financials) | Not supported |
| [Conditions](https://massive.com/docs/rest/stocks/market-operations/conditions) | Not supported |
| [Exchanges](https://massive.com/docs/rest/stocks/market-operations/exchanges) | Not supported |
