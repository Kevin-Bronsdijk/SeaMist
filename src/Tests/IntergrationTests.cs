using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaMist;
using SeaMist.Http;
using System.Net;
using SeaMist.Model;

namespace Tests
{
    [TestClass]
    public class IntergrationTests
    {
        [TestMethod]
        public void KrakenClient_Unauthorized_IsTrue()
        {
            var connection = KrakenConnection.Create("key", "secret");
            var krakenClient = new KrakenClient(connection);

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void KrakenClient_UnauthorizedSandbox_IsTrue()
        {
            var connection = KrakenConnection.Create("key", "secret");
            var krakenClient = new KrakenClient(connection);

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void KrakenClient_InvalidResource404_IsTrue()
        { 
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.Image404));

            var result = response.Result;

            Assert.IsTrue((int)result.StatusCode == 422);

            Assert.IsTrue(result.Success == false);
        }

        [TestMethod]
        public void KrakenClient_IgnoreInvalidResource404Sandbox_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient(true);

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.Image404));

            var result = response.Result;

            // Should still work, file ignored gets random results
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes > 0);
        }

        [TestMethod]
        public void KrakenClient_SimpleOptimizeWaitOk_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);

            Assert.IsTrue(result.Success == true);
        }

        [TestMethod]
        public void KrakenClient_SimpleOptimizeCheckResultBody_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes > 0);
        }

        [TestMethod]
        public void KrakenClient_CustomRequestOptimizeCheckResultBody_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                Lossy = true
            };

            var response = krakenClient.OptimizeWait(request);
            var result = response.Result;

            // Can only check if we have data, cant check if lossy has been applied
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes > 0);
        }

        [TestMethod]
        public void KrakenClient_CustomRequestConvertImageFormat_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ConvertImage = new ConvertImage(ImageFormat.gif)
            };

            var response = krakenClient.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body.KrakedUrl.EndsWith(".gif"));
        }

        // Todo:
        // ResizeImage = new ResizeImage() { Height = 100, Width = 100}
        // Callback
        // Preserve meta
        // External storage
    }
}
