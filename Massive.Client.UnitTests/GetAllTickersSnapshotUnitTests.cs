using AutoFixture;
using FluentAssertions;
using Massive.Client.DependencyInjection;
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
    public class GetAllTickersSnapshotUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<HttpMessageHandler> _handler;
        private readonly TestService _testHarness;

        public GetAllTickersSnapshotUnitTests()
        {
            _fixture = new Fixture();
            _handler = new Mock<HttpMessageHandler>();

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
        public async Task GetAllTickersSnapshot_Returns_OK_Response()
        {
            var response = await _testHarness.MassiveClient.GetAllTickersSnapshot("SPY,META", false);

            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Tickers.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetAllTickersSnapshot_With_Unrecognized_Ticker_Response_Returns_OK_Response()
        {
            var response = await _testHarness.MassiveClient.GetAllTickersSnapshot("ASDF,QWER");

            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Tickers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAllTickersSnapshot_With_Null_Ticker_Returns_OK_Response()
        {
            var response = await _testHarness.MassiveClient.GetAllTickersSnapshot(null);

            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Tickers.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetAlltickersSnapshot_Returns_BadRequest_Response()
        {
            var json = JsonSerializer.Serialize(_fixture.Create<MassiveSnapshotResponse>());

            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(_handler.Object)
            {
                BaseAddress = new Uri("https://api.massive.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("MASSIVE_TOKEN")}");

            var client = new MassiveClient(httpClient, new NullLogger<MassiveClient>());

            var response = await client.GetAllTickersSnapshot(null);

            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.Tickers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAllTickersSnapshot_Throws_Exception_Returns_InternalServerError_Response()
        {
            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("asdf")
                });

            var httpClient = new HttpClient(_handler.Object)
            {
                BaseAddress = new Uri("https://api.massive.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("MASSIVE_TOKEN")}");

            var client = new MassiveClient(httpClient, new NullLogger<MassiveClient>());

            var response = await client.GetAllTickersSnapshot(null);

            response.Should().NotBeNull();
            response.Status.Should().Be("InternalServerError");
            response.Tickers.Should().BeNullOrEmpty();
        }
    }
}
