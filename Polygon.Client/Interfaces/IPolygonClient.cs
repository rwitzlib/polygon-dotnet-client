using Polygon.Client.Requests;
using Polygon.Client.Responses;
using System;
using System.Threading.Tasks;

namespace Polygon.Client.Interfaces
{
    public interface IPolygonClient
    {
        public Task<PolygonAggregateResponse> GetAggregates(PolygonAggregateRequest request);
        public Task<PolygonTickerDetailsResponse> GetTickerDetails(string ticker, DateTime? date = null);
        public Task<PolygonGetTickersResponse> GetTickers(PolygonGetTickersRequest request);
        public Task<PolygonSnapshotResponse> GetAllTickersSnapshot(string tickers, bool includeOtc = false);
        public Task<PolygonDailyMarketSummaryResponse> GetDailyMarketSummary(DateTime? date = null, bool includeOtc = false, bool adjusted = true);
    }
}
