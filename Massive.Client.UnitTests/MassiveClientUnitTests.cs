using FluentAssertions;
using Massive.Client.DependencyInjection;
using Massive.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Massive.Client.UnitTests
{
    public class MassiveClientUnitTests
    {
        [Fact]
        public void DefaultBaseUrl_Uses_Massive_Api()
        {
            MassiveClient.DefaultBaseUrl.Should().Be("https://api.massive.com");
        }

        [Fact]
        public void AddMassiveClient_Registers_IMassiveClient()
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

            var service = serviceProvider.GetRequiredService<TestService>();

            service.MassiveClient.Should().NotBeNull();
            service.MassiveClient.Should().BeAssignableTo<IMassiveClient>();
        }
    }
}
