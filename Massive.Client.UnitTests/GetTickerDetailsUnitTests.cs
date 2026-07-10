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
    public class GetTickerDetailsUnitTests
    {
        private readonly Mock<HttpMessageHandler> _handler;
        private readonly TestService _testHarness;

        public GetTickerDetailsUnitTests()
        {
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
        public async Task GetTickerDetails_Returns_OK_Response()
        {
            var ticker = "META";

            var response = await _testHarness.MassiveClient.GetTickerDetails(ticker);

            response.Should().NotBeNull();
            response.TickerDetails.Should().NotBeNull();
            response.TickerDetails.Ticker.Should().Be("META");
        }

        [Fact]
        public async Task GetTickerDetails_With_Unrecognized_Ticker_Response_Returns_NotFound_Response()
        {
            var ticker = "ASDKFM";

            var response = await _testHarness.MassiveClient.GetTickerDetails(ticker);

            response.Should().NotBeNull();
            response.Status.Should().Be("NotFound");
            response.TickerDetails.Should().BeNull();
        }

        [Fact]
        public async Task GetTickerDetails_With_Null_Ticker_BadRequest_Response()
        {
            var response = await _testHarness.MassiveClient.GetTickerDetails(null);

            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.TickerDetails.Should().BeNull();
        }

        [Fact]
        public async Task GetTickerDetails_Throws_Exception_Returns_InternalServerError_Response()
        {
            var ticker = "ASDKFM";

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

            var response = await client.GetTickerDetails(ticker);

            response.Should().NotBeNull();
            response.Status.Should().Be("InternalServerError");
            response.TickerDetails.Should().BeNull();
        }
    }
}
