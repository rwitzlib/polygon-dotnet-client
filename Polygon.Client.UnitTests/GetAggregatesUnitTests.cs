using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Polygon.Client.DependencyInjection;
using Polygon.Client.Models;
using Polygon.Client.Requests;
using Polygon.Client.Responses;
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
        public async Task GetAggregates_With_NextUrl_Null_Returns_Single_Response()
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

            var mockResponse = new PolygonAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 2,
                Results = new List<Bar>
                {
                    new Bar { Timestamp = 1711324800000, Open = 100.0f, High = 101.0f, Low = 99.0f, Close = 100.5f, Volume = 1000 },
                    new Bar { Timestamp = 1711324860000, Open = 100.5f, High = 102.0f, Low = 100.0f, Close = 101.5f, Volume = 1200 }
                },
                NextUrl = null // No next page
            };

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(mockResponse))
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
            response.NextUrl.Should().BeNull();

            // Verify only one HTTP call was made (no nexturl fetching)
            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAggregates_With_NextUrl_Fetches_Additional_Results()
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

            var firstResponse = new PolygonAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 2,
                Results = new List<Bar>
                {
                    new Bar { Timestamp = 1711324800000, Open = 100.0f, High = 101.0f, Low = 99.0f, Close = 100.5f, Volume = 1000 },
                    new Bar { Timestamp = 1711324860000, Open = 100.5f, High = 102.0f, Low = 100.0f, Close = 101.5f, Volume = 1200 }
                },
                NextUrl = "https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=next_page_token"
            };

            var secondResponse = new PolygonAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 1,
                Results = new List<Bar>
                {
                    new Bar { Timestamp = 1711324920000, Open = 101.5f, High = 103.0f, Low = 101.0f, Close = 102.5f, Volume = 1500 }
                },
                NextUrl = null // End of pagination
            };

            var handler = new Mock<HttpMessageHandler>();
            var callCount = 0;
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    callCount++;
                    if (callCount == 1)
                    {
                        // First call - initial request
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(firstResponse))
                        };
                    }
                    else
                    {
                        // Second call - nexturl request
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(secondResponse))
                        };
                    }
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
            response.Results.Should().HaveCount(3); // 2 from first page + 1 from second page
            response.NextUrl.Should().Be("https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=next_page_token");

            // Verify two HTTP calls were made
            handler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAggregates_With_Multiple_NextUrl_Pages_Fetches_All_Results()
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

            var firstResponse = new PolygonAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 2,
                Results = new List<Bar>
                {
                    new Bar { Timestamp = 1711324800000, Open = 100.0f, High = 101.0f, Low = 99.0f, Close = 100.5f, Volume = 1000 },
                    new Bar { Timestamp = 1711324860000, Open = 100.5f, High = 102.0f, Low = 100.0f, Close = 101.5f, Volume = 1200 }
                },
                NextUrl = "https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=page1"
            };

            var secondResponse = new PolygonAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 2,
                Results = new List<Bar>
                {
                    new Bar { Timestamp = 1711324920000, Open = 101.5f, High = 103.0f, Low = 101.0f, Close = 102.5f, Volume = 1500 },
                    new Bar { Timestamp = 1711324980000, Open = 102.5f, High = 104.0f, Low = 102.0f, Close = 103.5f, Volume = 1800 }
                },
                NextUrl = "https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=page2"
            };

            var thirdResponse = new PolygonAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 1,
                Results = new List<Bar>
                {
                    new Bar { Timestamp = 1711325040000, Open = 103.5f, High = 105.0f, Low = 103.0f, Close = 104.5f, Volume = 2000 }
                },
                NextUrl = null // End of pagination
            };

            var handler = new Mock<HttpMessageHandler>();
            var callCount = 0;
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    callCount++;
                    if (callCount == 1)
                    {
                        // First call - initial request
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(firstResponse))
                        };
                    }
                    else if (callCount == 2)
                    {
                        // Second call - first nexturl request
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(secondResponse))
                        };
                    }
                    else
                    {
                        // Third call - second nexturl request
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(thirdResponse))
                        };
                    }
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
            response.Results.Should().HaveCount(5); // 2 + 2 + 1 from three pages
            response.NextUrl.Should().Be("https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=page1");

            // Verify three HTTP calls were made
            handler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAggregates_With_NextUrl_Request_Failure_Returns_Original_Results()
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

            var firstResponse = new PolygonAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 2,
                Results = new List<Bar>
                {
                    new Bar { Timestamp = 1711324800000, Open = 100.0f, High = 101.0f, Low = 99.0f, Close = 100.5f, Volume = 1000 },
                    new Bar { Timestamp = 1711324860000, Open = 100.5f, High = 102.0f, Low = 100.0f, Close = 101.5f, Volume = 1200 }
                },
                NextUrl = "https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=next_page_token"
            };

            var handler = new Mock<HttpMessageHandler>();
            var callCount = 0;
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    callCount++;
                    if (callCount == 1)
                    {
                        // First call - initial request succeeds
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(firstResponse))
                        };
                    }
                    else
                    {
                        // Second call - nexturl request fails
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Content = new StringContent("Server error")
                        };
                    }
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
            response.Results.Should().HaveCount(2); // Only original results, nexturl failed
            response.NextUrl.Should().Be("https://api.polygon.io/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=next_page_token");

            // Verify two HTTP calls were made (second one failed but was attempted)
            handler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
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
    }
}
