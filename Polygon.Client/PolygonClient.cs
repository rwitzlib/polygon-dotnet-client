using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polygon.Client.Interfaces;
using Polygon.Client.Models;
using Polygon.Client.Requests;
using Polygon.Client.Responses;
using System.Text.Json;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace Polygon.Client;
public class PolygonClient : IPolygonClient
{
    private readonly HttpClient _client;
    private readonly ILogger<PolygonClient> _logger;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [ActivatorUtilitiesConstructor]
    public PolygonClient(HttpClient client, ILogger<PolygonClient> logger) 
    {
        _client = client;
        _logger = logger;
    }

    // TODO: move this to polygon client
    static async Task<string> ReadGzFile(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
        using (StreamReader reader = new StreamReader(gzipStream, Encoding.UTF8))
        {
            return await reader.ReadToEndAsync();
        }
    }

    // TODO: move this to polygon client
    //private static async Task<List<StocksResponse>> ParseStocksResponses(DateTimeOffset date)
    //{
    //    List<StocksResponse> responses = [];
    //    string gzFilePath = $"./{date:yyyy-MM-dd}.csv.gz"; // Replace with your file path

    //    try
    //    {

    //        string csvContent = await ReadGzFile(gzFilePath);

    //        string[] lines = csvContent.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);
    //        for (int i = 1; i < lines.Length; i++)
    //        {
    //            string[] columns = lines[i].Split(',');

    //            if (responses.Count == 0 || responses.Last().Ticker != columns[0])
    //            {
    //                responses.Add(new StocksResponse
    //                {
    //                    Ticker = columns[0],
    //                    Results = []
    //                });
    //            }

    //            var bar = new Bar
    //            {
    //                Volume = float.Parse(columns[1]),
    //                Open = float.Parse(columns[2]),
    //                Close = float.Parse(columns[3]),
    //                High = float.Parse(columns[4]),
    //                Low = float.Parse(columns[5]),
    //                Timestamp = long.Parse(columns[6]),
    //                TransactionCount = int.Parse(columns[7])
    //            };

    //            bar.Vwap = (bar.High + bar.Low + bar.Close) / 3;

    //            responses.Last().Results.Add(bar);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Oops, something went wrong: {ex.Message}");
    //    }

    //    return responses;
    //}

    public PolygonClient(string bearerToken)
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://api.polygon.io"),
        };

        if (bearerToken.StartsWith("Bearer "))
        {
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearerToken);
        }
        else
        {
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {bearerToken}");
        }

        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<PolygonClient>();
    }

    public PolygonClient(string bearerToken, JsonSerializerOptions options = null)
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://api.polygon.io"),
        };

        if (bearerToken.StartsWith("Bearer "))
        {
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearerToken);
        }
        else
        {
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {bearerToken}");
        }

        if (options is not null)
        {
            _options = options;
        }

        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<PolygonClient>();
    }

    public async Task<PolygonAggregateResponse> GetAggregates(PolygonAggregateRequest request)
    {
        if (request is null 
            || request.Ticker is null 
            || request.Multiplier is 0
            || request.Timespan is null 
            || request.From is null
            || request.To is null
            || request.From.CompareTo(request.To) > 0)
        {
            return GenerateAggregatesErrorResponse(request.Ticker ?? null, HttpStatusCode.BadRequest);
        }

        try
        {
            var url = $"/v2/aggs/ticker/{request.Ticker}/range/{request.Multiplier}/{request.Timespan}/{request.From}/{request.To}" +
                $"?adjusted={request.Adjusted}&sort={request.Sort}&limit={request.Limit}";

            var response = await _client.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return GenerateAggregatesErrorResponse(request.Ticker, response.StatusCode);
            }

            var polygonAggregateResponse = JsonSerializer.Deserialize<PolygonAggregateResponse>(json, _options);

            return polygonAggregateResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting aggregate data from Polygon API: {ex.Message}");
            return GenerateAggregatesErrorResponse(request.Ticker, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<PolygonTickerDetailsResponse> GetTickerDetails(string ticker, DateTime? date = null)
    {
        if (ticker is null)
        {
            return GenerateTickerDetailsErrorResponse(HttpStatusCode.BadRequest);
        }

        try
        {
            var url = $"/v3/reference/tickers/{ticker}";

            if (date != null)
            {
                url += $"?date={date}";
            }

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return GenerateTickerDetailsErrorResponse(response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            var tickerDetailsResponse = JsonSerializer.Deserialize<PolygonTickerDetailsResponse>(json, _options);

            return tickerDetailsResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting ticker details from Polygon API: {ex.Message}");
            return GenerateTickerDetailsErrorResponse(HttpStatusCode.InternalServerError);
        }
    }

    public async Task<PolygonGetTickersResponse> GetTickers(PolygonGetTickersRequest request)
    {
        try
        {
            var tickerList = new List<TickerDetails>();
            var tickerUrl = $"/v3/reference/tickers" +
                $"?ticker={request.Ticker}&type={request.Type}&market={request.Market}" +
                $"&exchange={request.Exchange}&cusip={request.Cusip}&cik={request.Cik}" +
                $"&date={request.Date}&search={request.Search}&active={request.Active}" +
                $"&order={request.Order}&sort={request.Sort}&limit={request.Limit}";

            while (tickerUrl != null)
            {
                var response = await _client.GetAsync(tickerUrl);

                if (!response.IsSuccessStatusCode)
                {
                    if (tickerList.Any())
                    {
                        break;
                    }

                    return GenerateGetTickersErrorResponse(response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();

                var scanResponse = JsonSerializer.Deserialize<PolygonGetTickersResponse>(content, _options);
                tickerList.AddRange(scanResponse.Results);

                tickerUrl = scanResponse.NextUrl;
            }

            return new PolygonGetTickersResponse
            {
                Status = HttpStatusCode.OK.ToString(),
                Results = tickerList
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting tickers from Polygon API: {ex.Message}");
            return GenerateGetTickersErrorResponse(HttpStatusCode.InternalServerError);
        }
    }

    public async Task<PolygonSnapshotResponse> GetAllTickersSnapshot(string tickers, bool includeOtc = false)
    {
        try
        {
            var url = $"/v2/snapshot/locale/us/markets/stocks/tickers?tickers={tickers}&include_otc={includeOtc}";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return GenerateSnapshotErrorResponse(response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();

            var snapshotResponse = JsonSerializer.Deserialize<PolygonSnapshotResponse>(json, _options);

            return snapshotResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting tickers from Polygon API: {ex.Message}");
            return GenerateSnapshotErrorResponse(HttpStatusCode.InternalServerError);
        }
    }

    public async Task<PolygonDailyMarketSummaryResponse> GetDailyMarketSummary(DateTime? date = null, bool includeOtc = false, bool adjusted = true)
    {
        try
        {
            var url = "/v2/aggs/grouped/locale/us/market/stocks";
            
            if (date != null)
            {
                url += $"/{date:yyyy-MM-dd}";
            }

            url += $"?include_otc={includeOtc}&adjusted={adjusted}";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return GenerateDailyMarketSummaryErrorResponse(response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            var marketSummaryResponse = JsonSerializer.Deserialize<PolygonDailyMarketSummaryResponse>(json, _options);

            return marketSummaryResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting daily market summary from Polygon API: {ex.Message}");
            return GenerateDailyMarketSummaryErrorResponse(HttpStatusCode.InternalServerError);
        }
    }

    #region Private Methods
    private static PolygonAggregateResponse GenerateAggregatesErrorResponse(string ticker, HttpStatusCode status)
    {
        return new PolygonAggregateResponse
        {
            Ticker = ticker,
            Status = status.ToString(),
            Results = [],
            ResultsCount = 0
        };
    }

    private static PolygonTickerDetailsResponse GenerateTickerDetailsErrorResponse(HttpStatusCode status)
    {
        return new PolygonTickerDetailsResponse
        {
            Status = status.ToString(),
            Count = 0,
            TickerDetails = null
        };
    }

    private static PolygonGetTickersResponse GenerateGetTickersErrorResponse(HttpStatusCode status)
    {
        return new PolygonGetTickersResponse
        {
            Status = status.ToString(),
            Results = []
        };
    }

    private static PolygonSnapshotResponse GenerateSnapshotErrorResponse(HttpStatusCode status)
    {
        return new PolygonSnapshotResponse
        {
            Status = status.ToString(),
            Tickers = []
        };
    }

    private static PolygonDailyMarketSummaryResponse GenerateDailyMarketSummaryErrorResponse(HttpStatusCode status)
    {
        return new PolygonDailyMarketSummaryResponse
        {
            Status = status.ToString(),
            Results = new List<BarWithTicker>(),
            QueryCount = 0,
            ResultsCount = 0,
            Count = 0
        };
    }
    #endregion
}
