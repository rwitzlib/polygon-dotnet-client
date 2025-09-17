using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Polygon.Client.Responses;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Polygon.Client.UnitTests
{
    public class GetMarketHolidaysUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<HttpMessageHandler> _handler;
        private readonly TestService _testHarness;

        public GetMarketHolidaysUnitTests()
        {
            _fixture = new Fixture();
            _handler = new Mock<HttpMessageHandler>();

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
        public async Task GetMarketHolidays_Returns_OK_Response()
        {
            // Act
            var response = await _testHarness.PolygonClient.GetMarketHolidays();

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Results.Should().NotBeNull();
        }

        [Fact]
        public async Task GetMarketHolidays_Returns_BadRequest_Response()
        {
            // Arrange
            var json = JsonSerializer.Serialize(_fixture.Create<PolygonMarketHolidaysResponse>());

            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(_handler.Object)
            {
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var response = await client.GetMarketHolidays();

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.Results.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetMarketHolidays_Throws_Exception_Returns_InternalServerError_Response()
        {
            // Arrange
            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("invalid json")
                });

            var httpClient = new HttpClient(_handler.Object)
            {
                BaseAddress = new Uri("https://api.polygon.io")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("POLYGON_TOKEN")}");

            var client = new PolygonClient(httpClient, new NullLogger<PolygonClient>());

            // Act
            var response = await client.GetMarketHolidays();

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("InternalServerError");
            response.Results.Should().BeNullOrEmpty();
        }
    }
}
