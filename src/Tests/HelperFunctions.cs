using System;
using System.IO;
using System.Net;
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

        public static string DownloadImage(string fileLocation)
        {
            // Todo: enable using of all types
            var fileName = Path.GetTempPath() + Guid.NewGuid() + ".jpg";

            using (var client = new WebClient())
            {
                client.DownloadFile(fileLocation, fileName);
            }

            return fileName;
        }
    }
}