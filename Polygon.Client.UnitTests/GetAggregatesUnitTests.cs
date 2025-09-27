using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Polygon.Client.DependencyInjection;
using Polygon.Client.Models;
using Polygon.Client.Requests;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Polygon.Client.UnitTests
{
    public class GetAggregatesUnitTests
    {
        private readonly TestService _testHarness;

        public GetAggregatesUnitTests()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("POLYGON_TOKEN")))
            {
                Environment.SetEnvironmentVariable("POLYGON_TOKEN", "");
            }

            var serviceProvider = new ServiceCollection()
                .AddPolygonClient($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}")
                .AddSingleton<TestService>()
                .AddLogging()
                .BuildServiceProvider();

            _testHarness = serviceProvider.GetRequiredService<TestService>();
        }

        [Fact]
        public async Task GetAggregates_With_DateTime_Request_Returns_OK_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = "2024-03-25",
                To = "2024-03-26"
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
        }

        [Fact]
        public async Task GetAggregates_With_Timestamp_Request_Returns_OK_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = "1699423200000",
                To = "1699595940000",
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
        }

        [Fact]
        public async Task GetAggregates_With_Null_Ticker_Returns_BadRequest_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = null,
                Multiplier = 1,
                Timespan = "minute",
                From = "2024-03-25",
                To = "2024-03-26"
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_With_Null_Timestamp_Returns_BadRequest_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = null,
                From = "2024-03-25",
                To = "2024-03-26"
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_With_Null_From_Returns_BadRequest_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = null,
                To = "2024-03-26"
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_With_Null_To_Returns_BadRequest_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = "2024-03-26",
                To = null
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_With_Bad_Multiplier_Returns_BadRequest_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 0,
                Timespan = "minute",
                From = "2024-03-25",
                To = "2024-03-26"
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_With_Bad_Date_Returns_BadRequest_Response()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 0,
                Timespan = "minute",
                From = "2024-03-26",
                To = "2024-03-25"
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_Throws_Exception_Returns_EmptyResponse()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = "2024-03-25",
                To = "2024-03-26"
            };

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("asdf")
                });

            var httpClient = new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var response = await client.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.InternalServerError.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_Returns_Correct_Timestamps()
        {
            var from = new DateTimeOffset(2024, 3, 5, 10, 0, 0, DateTimeOffset.Now.Offset);
            var to = new DateTimeOffset(2024, 3, 5, 14, 0, 0, DateTimeOffset.Now.Offset);

            var fromMilliseconds = from.ToUnixTimeMilliseconds();
            var toMilliseconds = to.ToUnixTimeMilliseconds();

            var fromUtc = (DateTimeOffset)from.UtcDateTime;
            var toUtc = (DateTimeOffset)to.UtcDateTime;

            var request = new PolygonAggregateRequest
            {
                Ticker = "MARA",
                Multiplier = 1,
                Timespan = "minute",
                From = fromMilliseconds.ToString(),
                To = toMilliseconds.ToString()
            };

            // Act
            var response = await _testHarness.PolygonClient.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());

            var firstTimestamp = response.Results.First().Timestamp;
            var lastTimestamp = response.Results.Last().Timestamp;

            fromMilliseconds.Should().Be(firstTimestamp);
            toMilliseconds.Should().Be(lastTimestamp);

            var firstDateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(firstTimestamp);
            var lastDateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(lastTimestamp);

            fromUtc.Should().Be(firstDateTimeOffset);
            toUtc.Should().Be(lastDateTimeOffset);

            var offset = (long)DateTimeOffset.Now.Offset.TotalSeconds;

            var convertedCandles = new List<Bar>();
            foreach (var candle in response.Results)
            {
                convertedCandles.Add(new Bar
                {
                    Timestamp = candle.Timestamp / 1000 + offset
                });
            }

            var firstConvertedCandle = convertedCandles.First();
            var lastConvertedCandle = convertedCandles.Last();

            var firstConvertedCandleTimestamp = DateTimeOffset.FromUnixTimeSeconds(firstConvertedCandle.Timestamp);
            var lastConvertedCandleTimestamp = DateTimeOffset.FromUnixTimeSeconds(lastConvertedCandle.Timestamp);

            firstConvertedCandleTimestamp.DateTime.Should().Be(from.DateTime);
            lastConvertedCandleTimestamp.DateTime.Should().Be(to.DateTime);
        }

        [Fact]
        public async Task GetAggregates_With_NextUrl_Returns_Combined_Results()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = "2024-03-25",
                To = "2024-03-26"
            };

            var initialResponse = new
            {
                status = "OK",
                request_id = "test",
                results = new[]
                {
                    new { o = 100f, h = 101f, l = 99f, c = 100.5f, v = 1000f, t = 1648166400000L, n = 100 }
                },
                next_url = "https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?next=123"
            };

            var nextResponse = new
            {
                status = "OK",
                request_id = "test2",
                results = new[]
                {
                    new { o = 102f, h = 103f, l = 101f, c = 102.5f, v = 2000f, t = 1648167400000L, n = 200 }
                }
            };

            var handler = new Mock<HttpMessageHandler>();
            
            // Setup initial request response
            handler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(initialResponse))
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(nextResponse))
                });

            var httpClient = new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var response = await client.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            response.Results.Should().HaveCount(2);
            
            var results = response.Results.ToList();
            results[0].Open.Should().Be(100f);
            results[1].Open.Should().Be(102f);
        }

        [Fact]
        public async Task GetAggregates_Without_NextUrl_Returns_Only_Initial_Results()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = "2024-03-25",
                To = "2024-03-26"
            };

            var response = new
            {
                status = "OK",
                request_id = "test",
                results = new[]
                {
                    new { o = 100f, h = 101f, l = 99f, c = 100.5f, v = 1000f, t = 1648166400000L, n = 100 }
                }
            };

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });

            var httpClient = new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var result = await client.GetAggregates(request);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(HttpStatusCode.OK.ToString());
            result.Results.Should().HaveCount(1);
            result.Results.First().Open.Should().Be(100f);
        }

        [Fact]
        public async Task GetAggregates_With_NextUrl_Error_Returns_Initial_Results()
        {
            // Arrange
            var request = new PolygonAggregateRequest
            {
                Ticker = "SPY",
                Multiplier = 1,
                Timespan = "minute",
                From = "2024-03-25",
                To = "2024-03-26"
            };

            var initialResponse = new
            {
                status = "OK",
                request_id = "test",
                results = new[]
                {
                    new { o = 100f, h = 101f, l = 99f, c = 100.5f, v = 1000f, t = 1648166400000L, n = 100 }
                },
                next_url = "https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?next=123"
            };

            var handler = new Mock<HttpMessageHandler>();
            
            // Setup initial request success and next_url request failure
            handler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(initialResponse))
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Error")
                });

            var httpClient = new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var response = await client.GetAggregates(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            response.Results.Should().HaveCount(1);
            response.Results.First().Open.Should().Be(100f);
        }
    }
}
