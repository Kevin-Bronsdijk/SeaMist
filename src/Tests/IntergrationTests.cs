using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaMist;
using SeaMist.Http;
using System.Net;
using SeaMist.Model;
using ExifLib;
using System.Drawing;
using System.IO;

namespace Tests
{
    [TestClass]
    [DeploymentItem("Images")]
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
        public void KrakenClient_GetUserStatus_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.UserStatus();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success == true);

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.PlanName));
            Assert.IsTrue(result.Body.Active == true || result.Body.Active == false);
            Assert.IsTrue(result.Body.QuotaTotal > -0);
            Assert.IsTrue(result.Body.QuotaUsed > -0);
            Assert.IsTrue(result.Body.QuotaRemaining > -999999);
        }

        /// <summary>
        /// Must be a Reseller to run this test
        /// </summary>
        [TestMethod]
        public void KrakenClient_ResellerAccountRequest_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.ResellerAccount();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success == true);
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

            Assert.IsTrue(result.Body != null);

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

            Assert.IsTrue(result.Body != null);

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

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.EndsWith(".gif"));
        }

        [TestMethod]
        public void KrakenClient_CustomRequestChangeSize_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ResizeImage = new ResizeImage() { Height = 100, Width = 100 }
            };

            var response = krakenClient.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            Image img = Image.FromFile(localFile);
            Assert.IsTrue(img.Height == 100);
            Assert.IsTrue(img.Width == 100);
        }

        [TestMethod]
        public void KrakenClient_OptimizeRequestNoWait_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeRequest(new Uri(TestData.ImageOne), new Uri("http://requestb.in/yfxmpzyf"));

            var response = krakenClient.Optimize(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_OptimizeRequestNoWaitSandbox_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient(true);

            var request = new OptimizeRequest(new Uri(TestData.ImageOne), new Uri("http://requestb.in/yfxmpzyf"));

            var response = krakenClient.Optimize(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(result.Body.Id == null);
        }

        [TestMethod]
        public void KrakenClient_CustomRequestRemoveGeoData_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag));
   
            var response = krakenClient.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            try
            {
                using (ExifReader reader = new ExifReader(localFile))
                {
                }

                Assert.IsTrue(false, "No Exception");
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "No Exif data");
            }
        }

        [TestMethod]
        public void KrakenClient_CustomRequestKeepGeoData_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag))
            {
                PreserveMeta = new PreserveMeta[] { PreserveMeta.Geotag }
            };

            var response = krakenClient.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            try
            {
                using (ExifReader reader = new ExifReader(localFile))
                {
                    Double[] GpsLongArray;
                    Double[] GpsLatArray;
                    Double GpsLongDouble = 0;
                    Double GpsLatDouble = 0;

                    if (reader.GetTagValue<Double[]>(ExifTags.GPSLongitude, out GpsLongArray)
                        && reader.GetTagValue<Double[]>(ExifTags.GPSLatitude, out GpsLatArray))
                    {
                        GpsLongDouble = GpsLongArray[0] + GpsLongArray[1] / 60 + GpsLongArray[2] / 3600;
                        GpsLatDouble = GpsLatArray[0] + GpsLatArray[1] / 60 + GpsLatArray[2] / 3600;
                    }

                    Assert.IsTrue(GpsLongDouble == 120.7602005);
                    Assert.IsTrue(GpsLatDouble == 21.958606694444445);
                }
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "No Exif data");
            }
        }

        [TestMethod]
        public void KrakenClient_UploadImageWaitResult_IsTrue()
        {
            var testImageName = "test.png";
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes("Kraken.png"); // Read deployed file
            var task = krakenClient.OptimizeWait(image, testImageName);

            var result = task.Result;

            Assert.IsTrue(result.Success == true);
            Assert.IsTrue(result.Body != null);

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.FileName == testImageName);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.EndsWith(testImageName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes > 0);
        }

        [TestMethod]
        public void KrakenClient_UploadImageCallback_IsTrue()
        {
            var testImageName = "test.png";
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes("Kraken.png"); // Read deployed file
            var task = krakenClient.Optimize(image, testImageName, new Uri("http://requestb.in/q6wvgsq6"));

            var result = task.Result;

            Assert.IsTrue(result.Success == true);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        // Todo:
        // External storage + Wait and Callback
        // Upload and External storage Wait + Callback
    }
}
