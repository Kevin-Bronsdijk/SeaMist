using SeaMist;
using SeaMist.Http;
using System;
using System.Net;
using System.IO;

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
            string fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".jpg";
   
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(fileLocation, fileName);
            }

            return fileName;
        } 
    }
}
