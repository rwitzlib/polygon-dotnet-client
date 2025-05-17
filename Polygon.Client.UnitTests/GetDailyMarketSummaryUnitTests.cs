using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Polygon.Client.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace Polygon.Client.UnitTests
{
    public class GetDailyMarketSummaryUnitTests
    {
        private readonly TestService _testHarness;

        public GetDailyMarketSummaryUnitTests()
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
        public async Task GetDailyMarketSummary_With_Date_Returns_OK_Response()
        {
            // Arrange
            var date = new DateTime(2024, 3, 25);

            // Act
            var response = await _testHarness.PolygonClient.GetDailyMarketSummary(date);

            // Assert
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
            // Arrange
            var futureDate = DateTime.Now.AddDays(1);

            // Act
            var response = await _testHarness.PolygonClient.GetDailyMarketSummary(futureDate);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().NotBe(HttpStatusCode.OK.ToString());
            response.Results.Should().BeEmpty();
            response.QueryCount.Should().Be(0);
            response.ResultsCount.Should().Be(0);
            response.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetDailyMarketSummary_Throws_Exception_Returns_ErrorResponse()
        {
            // Arrange
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
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var response = await client.GetDailyMarketSummary();

            // Assert
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
            // Arrange
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
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var response = await client.GetDailyMarketSummary();

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            response.Results.Should().BeEmpty();
            response.QueryCount.Should().Be(0);
            response.ResultsCount.Should().Be(0);
            response.Count.Should().Be(0);
        }
    }
} 