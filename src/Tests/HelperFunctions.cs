using SeaMist;
using SeaMist.Http;

namespace Tests
{
    public static class HelperFunctions
    {
        public static KrakenClient CreateWorkingClient(bool debug = false)
        {
            var connection = KrakenConnection.Create(Settings.ApiKey, Settings.ApiSecret, debug);
            var krakenClient = new KrakenClient(connection);

            return krakenClient;
        }
    }
}
