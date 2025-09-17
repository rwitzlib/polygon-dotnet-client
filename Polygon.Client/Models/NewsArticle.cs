using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Polygon.Client.Models
{
    /// <summary>
    /// Represents a news article.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class NewsArticle
    {
        /// <summary>
        /// The article ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The publisher of the article.
        /// </summary>
        [JsonPropertyName("publisher")]
        public Publisher Publisher { get; set; }

        /// <summary>
        /// The title of the article.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The author of the article.
        /// </summary>
        [JsonPropertyName("author")]
        public string Author { get; set; }

        /// <summary>
        /// The publication date in UTC.
        /// </summary>
        [JsonPropertyName("published_utc")]
        public long PublishedUtc { get; set; }

        /// <summary>
        /// The article content.
        /// </summary>
        [JsonPropertyName("article_url")]
        public string ArticleUrl { get; set; }

        /// <summary>
        /// A list of tickers mentioned in the article.
        /// </summary>
        [JsonPropertyName("tickers")]
        public IEnumerable<string> Tickers { get; set; }

        /// <summary>
        /// A list of AMP URLs for the article.
        /// </summary>
        [JsonPropertyName("amp_url")]
        public string AmpUrl { get; set; }

        /// <summary>
        /// The article image URL.
        /// </summary>
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// A brief description of the article.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Keywords associated with the article.
        /// </summary>
        [JsonPropertyName("keywords")]
        public IEnumerable<string> Keywords { get; set; }
    }

    /// <summary>
    /// Represents the publisher of a news article.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Publisher
    {
        /// <summary>
        /// The name of the publisher.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The homepage URL of the publisher.
        /// </summary>
        [JsonPropertyName("homepage_url")]
        public string HomepageUrl { get; set; }

        /// <summary>
        /// The logo URL of the publisher.
        /// </summary>
        [JsonPropertyName("logo_url")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// The favicon URL of the publisher.
        /// </summary>
        [JsonPropertyName("favicon_url")]
        public string FaviconUrl { get; set; }
    }
}
