using Massive.Client.Interfaces;

namespace Massive.Client.UnitTests
{
    public class TestService(IMassiveClient massiveClient)
    {
        public IMassiveClient MassiveClient { get; } = massiveClient;
    }
}
