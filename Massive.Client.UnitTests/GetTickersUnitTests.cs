using AutoFixture;
using FluentAssertions;
using Massive.Client.DependencyInjection;
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
    public class GetTickersUnitTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<HttpMessageHandler> _handler;
        private readonly TestService _testHarness;

        public GetTickersUnitTests()
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
        public async Task GetTickers_With_Default_Request_Returns_OK_Response()
        {
            var response = await _testHarness.MassiveClient.GetTickers(new MassiveGetTickersRequest());

            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Results.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetTickers_With_Valid_Request_Returns_OK_Response()
        {
            var request = new MassiveGetTickersRequest
            {
                Type = "CS"
            };

            var response = await _testHarness.MassiveClient.GetTickers(request);

            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Results.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetTickers_With_BadRequest_Response_Returns_OK_Response()
        {
            var request = new MassiveGetTickersRequest
            {
                Ticker = "asdfahg",
                Exchange = "asdf"
            };

            var response = await _testHarness.MassiveClient.GetTickers(request);

            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTickers_With_Forced_BadRequest_Returns_BadRequest_Response()
        {
            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("asdf")
                });

            var httpClient = new HttpClient(_handler.Object)
            {
                BaseAddress = new Uri("https://api.massive.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Environment.GetEnvironmentVariable("MASSIVE_TOKEN")}");

            var client = new MassiveClient(httpClient, new NullLogger<MassiveClient>());

            var response = await client.GetTickers(new MassiveGetTickersRequest());

            response.Should().NotBeNull();
            response.Status.Should().Be("BadRequest");
            response.Results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTickers_With_Valid_And_Invalid_Responses_Returns_OK_Response()
        {
            var json = JsonSerializer.Serialize(_fixture.Create<MassiveGetTickersResponse>());

            _handler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                }).ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("asdf")
                });

            var httpClient = new HttpClient(_handler.Object)
            {
                BaseAddress = new Uri("https://api.massive.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer test");

            var client = new MassiveClient(httpClient, new NullLogger<MassiveClient>());

            var response = await client.GetTickers(new MassiveGetTickersRequest());

            response.Should().NotBeNull();
            response.Status.Should().Be("OK");
            response.Results.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetTickers_Throws_Exception_Returns_InternalServerError_Response()
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

            var response = await client.GetTickers(new MassiveGetTickersRequest());

            response.Should().NotBeNull();
            response.Status.Should().Be("InternalServerError");
            response.Results.Should().BeEmpty();
        }
    }
}
