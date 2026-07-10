using System.Diagnostics.CodeAnalysis;

namespace Massive.Client.Requests
{
    [ExcludeFromCodeCoverage]
    public class MassiveAggregateRequest
    {
        /// <summary>
        /// The ticker symbol of the stock/equity.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// The size of the timespan multiplier.
        /// </summary>
        public int Multiplier { get; set; }

        /// <summary>
        /// The size of the time window.
        /// </summary>
        public string Timespan { get; set; }

        /// <summary>
        /// The start of the aggregate time window. Either a date with the format YYYY-MM-DD or
        /// a millisecond timestamp.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The end of the aggregate time window. Either a date with the format YYYY-MM-DD or
        /// a millisecond timestamp.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Whether or not the results are adjusted for splits. By default, results are adjusted. Set this
        /// to false to get results that are NOT adjusted for splits.
        /// </summary>
        public bool Adjusted { get; set; } = true;

        /// <summary>
        /// Sort the results by timestamp. asc will return results in ascending order (oldest at the top),
        /// desc will return results in descending order (newest at the top).
        /// </summary>
        public string Sort { get; set; } = "asc";

        /// <summary>
        /// Limits the number of base aggregates queried to create the aggregate results. Max 50000 and
        /// Default 5000. Read more about how limit is used to calculate aggregate results in our article on
        /// Aggregate Data API Improvements.
        /// </summary>
        public int Limit { get; set; } = 5000;
    }
}
