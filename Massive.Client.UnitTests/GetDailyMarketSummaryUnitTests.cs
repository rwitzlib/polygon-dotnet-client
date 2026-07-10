using FluentAssertions;
using Massive.Client.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Headers;

namespace Massive.Client.UnitTests
{
    public class GetDailyMarketSummaryUnitTests
    {
        private readonly TestService _testHarness;

        public GetDailyMarketSummaryUnitTests()
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
        public async Task GetDailyMarketSummary_With_Date_Returns_OK_Response()
        {
            var date = new DateTime(2024, 3, 25);

            var response = await _testHarness.MassiveClient.GetDailyMarketSummary(date);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.OK.ToString());
            response.Results.Should().NotBeNull();
            response.QueryCount.Should().BeGreaterThan(0);
            response.ResultsCount.Should().BeGreaterThan(0);
            response.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetDailyMarketSummary_With_Future_Date_Returns_Error_Response()
        {
            var futureDate = DateTime.UtcNow.AddDays(10);

            var response = await _testHarness.MassiveClient.GetDailyMarketSummary(futureDate);

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.Forbidden.ToString());
            response.Results.Should().BeEmpty();
            response.QueryCount.Should().Be(0);
            response.ResultsCount.Should().Be(0);
            response.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetDailyMarketSummary_Throws_Exception_Returns_ErrorResponse()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("invalid json")
                });

            var httpClient = new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://api.massive.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("MASSIVE_TOKEN")}");

            var client = new MassiveClient(httpClient, new NullLogger<MassiveClient>());

            var response = await client.GetDailyMarketSummary();

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.InternalServerError.ToString());
            response.Results.Should().BeEmpty();
            response.QueryCount.Should().Be(0);
            response.ResultsCount.Should().Be(0);
            response.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetDailyMarketSummary_With_Invalid_Response_Returns_ErrorResponse()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("")
                });

            var httpClient = new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://api.massive.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("MASSIVE_TOKEN")}");

            var client = new MassiveClient(httpClient, new NullLogger<MassiveClient>());

            var response = await client.GetDailyMarketSummary();

            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeEmpty();
            response.QueryCount.Should().Be(0);
            response.ResultsCount.Should().Be(0);
            response.Count.Should().Be(0);
        }
    }
}
