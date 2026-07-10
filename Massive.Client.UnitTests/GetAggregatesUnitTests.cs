using FluentAssertions;
using Massive.Client.DependencyInjection;
using Massive.Client.Models;
using Massive.Client.Requests;
using Massive.Client.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Massive.Client.UnitTests
{
    public class GetAggregatesUnitTests
    {
        private readonly TestService _testHarness;

        public GetAggregatesUnitTests()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MASSIVE_TOKEN")))
            {
                Environment.SetEnvironmentVariable("MASSIVE_TOKEN", "");
            }

            var serviceProvider = new ServiceCollection()
                .AddMassiveClient($"Bearer {Environment.GetEnvironmentVariable("MASSIVE_TOKEN")}")
                .AddSingleton<TestService>()
                .AddLogging()
                .BuildServiceProvider();

            _testHarness = serviceProvider.GetRequiredService<TestService>();
        }

        [Fact]
        public async Task GetAggregates_With_DateTime_Request_Returns_OK_Response()
        {
            var request = new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" };

            var response = await _testHarness.MassiveClient.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
        }

        [Fact]
        public async Task GetAggregates_With_Timestamp_Request_Returns_OK_Response()
        {
            var request = new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "1699423200000", To = "1699595940000" };

            var response = await _testHarness.MassiveClient.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
        }

        [Fact]
        public async Task GetAggregates_With_Null_Ticker_Returns_BadRequest_Response()
        {
            await AssertBadAggregateRequest(new MassiveAggregateRequest { Ticker = null, Multiplier = 1, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" });
        }

        [Fact]
        public async Task GetAggregates_With_Null_Timestamp_Returns_BadRequest_Response()
        {
            await AssertBadAggregateRequest(new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = null, From = "2024-03-25", To = "2024-03-26" });
        }

        [Fact]
        public async Task GetAggregates_With_Null_From_Returns_BadRequest_Response()
        {
            await AssertBadAggregateRequest(new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = null, To = "2024-03-26" });
        }

        [Fact]
        public async Task GetAggregates_With_Null_To_Returns_BadRequest_Response()
        {
            await AssertBadAggregateRequest(new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "2024-03-26", To = null });
        }

        [Fact]
        public async Task GetAggregates_With_Bad_Multiplier_Returns_BadRequest_Response()
        {
            await AssertBadAggregateRequest(new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 0, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" });
        }

        [Fact]
        public async Task GetAggregates_With_Bad_Date_Returns_BadRequest_Response()
        {
            await AssertBadAggregateRequest(new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 0, Timespan = "minute", From = "2024-03-26", To = "2024-03-25" });
        }

        [Fact]
        public async Task GetAggregates_Throws_Exception_Returns_EmptyResponse()
        {
            var request = new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" };
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("asdf")
                });

            var client = CreateClient(handler);

            var response = await client.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.InternalServerError.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAggregates_With_NextUrl_Null_Returns_Single_Response()
        {
            var request = new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" };
            var mockResponse = new MassiveAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = 2,
                Results =
                [
                    new Bar { Timestamp = 1711324800000, Open = 100.0f, High = 101.0f, Low = 99.0f, Close = 100.5f, Volume = 1000 },
                    new Bar { Timestamp = 1711324860000, Open = 100.5f, High = 102.0f, Low = 100.0f, Close = 101.5f, Volume = 1200 }
                ],
                NextUrl = null
            };

            var handler = JsonHandler(mockResponse);
            var client = CreateClient(handler);

            var response = await client.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            response.Results.Should().HaveCount(2);
            response.NextUrl.Should().BeNull();

            handler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAggregates_With_NextUrl_Fetches_Additional_Results()
        {
            var request = new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" };
            var firstResponse = AggregateResponse(2, "https://api.massive.com/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=next_page_token");
            var secondResponse = AggregateResponse(1, null, 1711324920000);
            var handler = SequenceHandler(firstResponse, secondResponse);
            var client = CreateClient(handler);

            var response = await client.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            response.Results.Should().HaveCount(3);
            response.NextUrl.Should().Be("https://api.massive.com/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=next_page_token");
            handler.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAggregates_With_Multiple_NextUrl_Pages_Fetches_All_Results()
        {
            var request = new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" };
            var firstResponse = AggregateResponse(2, "https://api.massive.com/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=page1");
            var secondResponse = AggregateResponse(2, "https://api.massive.com/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=page2", 1711324920000);
            var thirdResponse = AggregateResponse(1, null, 1711325040000);
            var handler = SequenceHandler(firstResponse, secondResponse, thirdResponse);
            var client = CreateClient(handler);

            var response = await client.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            response.Results.Should().HaveCount(5);
            response.NextUrl.Should().Be("https://api.massive.com/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=page1");
            handler.Protected().Verify("SendAsync", Times.Exactly(3), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAggregates_With_NextUrl_Request_Failure_Returns_Original_Results()
        {
            var request = new MassiveAggregateRequest { Ticker = "SPY", Multiplier = 1, Timespan = "minute", From = "2024-03-25", To = "2024-03-26" };
            var nextUrl = "https://api.massive.com/v2/aggs/ticker/SPY/range/1/minute/2024-03-25/2024-03-26?cursor=next_page_token";
            var firstResponse = AggregateResponse(2, nextUrl);
            var handler = new Mock<HttpMessageHandler>();
            var callCount = 0;
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    callCount++;
                    return callCount == 1
                        ? new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(firstResponse)) }
                        : new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("Server error") };
                });
            var client = CreateClient(handler);

            var response = await client.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            response.Results.Should().HaveCount(2);
            response.NextUrl.Should().Be(nextUrl);
            handler.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
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
            var request = new MassiveAggregateRequest { Ticker = "MARA", Multiplier = 1, Timespan = "minute", From = fromMilliseconds.ToString(), To = toMilliseconds.ToString() };

            var response = await _testHarness.MassiveClient.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            var firstTimestamp = response.Results.First().Timestamp;
            var lastTimestamp = response.Results.Last().Timestamp;
            fromMilliseconds.Should().Be(firstTimestamp);
            toMilliseconds.Should().Be(lastTimestamp);
            fromUtc.Should().Be(DateTimeOffset.FromUnixTimeMilliseconds(firstTimestamp));
            toUtc.Should().Be(DateTimeOffset.FromUnixTimeMilliseconds(lastTimestamp));
        }

        private static MassiveClient CreateClient(Mock<HttpMessageHandler> handler)
        {
            var httpClient = new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://api.massive.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("MASSIVE_TOKEN")}");
            return new MassiveClient(httpClient, new NullLogger<MassiveClient>());
        }

        private async Task AssertBadAggregateRequest(MassiveAggregateRequest request)
        {
            var response = await _testHarness.MassiveClient.GetAggregates(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeNullOrEmpty();
        }

        private static Mock<HttpMessageHandler> JsonHandler(MassiveAggregateResponse response)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });
            return handler;
        }

        private static Mock<HttpMessageHandler> SequenceHandler(params MassiveAggregateResponse[] responses)
        {
            var handler = new Mock<HttpMessageHandler>();
            var callCount = 0;
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    var response = responses[Math.Min(callCount, responses.Length - 1)];
                    callCount++;
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(response))
                    };
                });
            return handler;
        }

        private static MassiveAggregateResponse AggregateResponse(int count, string nextUrl, long startTimestamp = 1711324800000)
        {
            return new MassiveAggregateResponse
            {
                Ticker = "SPY",
                Status = HttpStatusCode.OK.ToString(),
                ResultsCount = count,
                Results = Enumerable.Range(0, count)
                    .Select(index => new Bar
                    {
                        Timestamp = startTimestamp + (index * 60000),
                        Open = 100.0f + index,
                        High = 101.0f + index,
                        Low = 99.0f + index,
                        Close = 100.5f + index,
                        Volume = 1000 + index
                    })
                    .ToList(),
                NextUrl = nextUrl
            };
        }
    }
}
