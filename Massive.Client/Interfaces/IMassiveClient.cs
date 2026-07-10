using Massive.Client.Requests;
using Massive.Client.Responses;
using System;
using System.Threading.Tasks;

namespace Massive.Client.Interfaces
{
    public interface IMassiveClient
    {
        public Task<MassiveAggregateResponse> GetAggregates(MassiveAggregateRequest request);
        public Task<MassiveTickerDetailsResponse> GetTickerDetails(string ticker, DateTime? date = null);
        public Task<MassiveGetTickersResponse> GetTickers(MassiveGetTickersRequest request);
        public Task<MassiveSnapshotResponse> GetAllTickersSnapshot(string tickers, bool includeOtc = false);
        public Task<MassiveDailyMarketSummaryResponse> GetDailyMarketSummary(DateTime? date = null, bool includeOtc = false, bool adjusted = true);
    }
}
