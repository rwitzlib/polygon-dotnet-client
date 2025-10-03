using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Polygon.Client.DependencyInjection;
using Polygon.Client.Requests;
using Polygon.Client.Responses;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Polygon.Client.UnitTests
{
    public class GetSnapshotGainersLosersUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<HttpMessageHandler> _handler;
        private readonly TestService _testHarness;

        public GetSnapshotGainersLosersUnitTests()
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
        public async Task GetSnapshotGainersLosers_With_Gainers_Returns_OK_Response()
        {
            // Act
            var request = new PolygonSnapshotGainersLosersRequest
            {
                Direction = "gainers",
                IncludeOtc = false,
                Limit = 20
            };

            var response = await _testHarness.PolygonClient.GetSnapshotGainersLosers(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Tickers.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSnapshotGainersLosers_With_Losers_Returns_OK_Response()
        {
            // Act
            var request = new PolygonSnapshotGainersLosersRequest
            {
                Direction = "losers",
                IncludeOtc = false,
                Limit = 20
            };

            var response = await _testHarness.PolygonClient.GetSnapshotGainersLosers(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Tickers.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSnapshotGainersLosers_With_Otc_Included_Returns_OK_Response()
        {
            // Act
            var request = new PolygonSnapshotGainersLosersRequest
            {
                Direction = "gainers",
                IncludeOtc = true,
                Limit = 50
            };

            var response = await _testHarness.PolygonClient.GetSnapshotGainersLosers(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Tickers.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSnapshotGainersLosers_With_Null_Request_Returns_BadRequest_Response()
        {
            // Arrange
            var json = JsonSerializer.Serialize(_fixture.Create<PolygonSnapshotGainersLosersResponse>());

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
            var response = await client.GetSnapshotGainersLosers(null);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.Tickers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetSnapshotGainersLosers_With_Invalid_Direction_Returns_BadRequest_Response()
        {
            // Arrange
            var json = JsonSerializer.Serialize(_fixture.Create<PolygonSnapshotGainersLosersResponse>());

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
            var request = new PolygonSnapshotGainersLosersRequest
            {
                Direction = "invalid",
                IncludeOtc = false,
                Limit = 20
            };

            var response = await client.GetSnapshotGainersLosers(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.Tickers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetSnapshotGainersLosers_With_Empty_Direction_Returns_BadRequest_Response()
        {
            // Arrange
            var json = JsonSerializer.Serialize(_fixture.Create<PolygonSnapshotGainersLosersResponse>());

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
            var request = new PolygonSnapshotGainersLosersRequest
            {
                Direction = "",
                IncludeOtc = false,
                Limit = 20
            };

            var response = await client.GetSnapshotGainersLosers(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.Tickers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetSnapshotGainersLosers_Returns_BadRequest_Response()
        {
            // Arrange
            var json = JsonSerializer.Serialize(_fixture.Create<PolygonSnapshotGainersLosersResponse>());

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
            var request = new PolygonSnapshotGainersLosersRequest
            {
                Direction = "gainers",
                IncludeOtc = false,
                Limit = 20
            };

            var response = await client.GetSnapshotGainersLosers(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.Tickers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetSnapshotGainersLosers_Throws_Exception_Returns_InternalServerError_Response()
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
            var request = new PolygonSnapshotGainersLosersRequest
            {
                Direction = "gainers",
                IncludeOtc = false,
                Limit = 20
            };

            var response = await client.GetSnapshotGainersLosers(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("InternalServerError");
            response.Tickers.Should().BeNullOrEmpty();
        }
    }
}
