using System;
using System.Drawing;
using System.IO;
using System.Net;
using ExifLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaMist;
using SeaMist.Http;
using SeaMist.Model;
using SeaMist.Model.Azure;
using OptimizeRequest = SeaMist.Model.OptimizeRequest;
using OptimizeUploadRequest = SeaMist.Model.OptimizeUploadRequest;
using OptimizeUploadWaitRequest = SeaMist.Model.OptimizeUploadWaitRequest;
using OptimizeWaitRequest = SeaMist.Model.OptimizeWaitRequest;

namespace Tests
{
    [TestClass]
    [DeploymentItem("Images")]
    public class IntergrationTests
    {
        private readonly Uri callbackUri = new Uri("http://requestb.in/15gm5dz1");

        [TestInitialize]
        public void Initialize()
        {
        }

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

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);


            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.PlanName));
            Assert.IsTrue(result.Body.Active || result.Body.Active == false);
            Assert.IsTrue(result.Body.QuotaTotal > -0);
            Assert.IsTrue(result.Body.QuotaUsed > -0);
            Assert.IsTrue(result.Body.QuotaRemaining > -999999);
            Assert.IsTrue(result.Body.Success);
        }

        [TestMethod]
        [Ignore]
        public void KrakenClient_ResellerAccountRequest_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.ResellerAccount();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void KrakenClient_ResellerAccountRequestNotAReseller_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.ResellerAccount();

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Forbidden);
            Assert.IsTrue(result.Success == false);
        }

        [TestMethod]
        public void KrakenClient_InvalidResource404_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.Image404));

            var result = response.Result;

            Assert.IsTrue((int) result.StatusCode == 422);
            Assert.IsTrue(result.Success == false);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Error));
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
            Assert.IsTrue(result.Success);

            Assert.IsTrue(result.Body.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes > 0);
        }

        [TestMethod]
        public void KrakenClient_OptimizeWait_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void KrakenClient_OptimizeCheckResultBody_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes > 0);
        }

        [TestMethod]
        public void KrakenClient_OptimizeCallbackCheckResultBody_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.Optimize(
                new Uri(TestData.ImageOne), callbackUri);

            var result = response.Result;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
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

            Assert.IsTrue(result.Success);
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
        public void KrakenClient_CustomRequestConvertImageFormatEmptyConstructor_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var convertImage = new ConvertImage()
            {
                BackgroundColor = "#ffffff",
                Format = ImageFormat.gif
            };

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ConvertImage = convertImage
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
                ResizeImage = new ResizeImage {Height = 100, Width = 100,
                    BackgroundColor = "#ffffff" , Strategy = Strategy.exact}
            };

            var response = krakenClient.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            var img = Image.FromFile(localFile);
            Assert.IsTrue(img.Height == 100);
            Assert.IsTrue(img.Width == 100);
        }

        [TestMethod]
        public void KrakenClient_OptimizeRequestCallback_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeRequest(new Uri(TestData.ImageOne), callbackUri);

            var response = krakenClient.Optimize(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_OptimizeRequestCallbackSandbox_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient(true);

            var request = new OptimizeRequest(new Uri(TestData.ImageOne), callbackUri);

            var response = krakenClient.Optimize(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            // No id in sandbox mode
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
                using (var reader = new ExifReader(localFile))
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
                PreserveMeta = new[] {PreserveMeta.Geotag}
            };

            var response = krakenClient.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            try
            {
                using (var reader = new ExifReader(localFile))
                {
                    double[] GpsLongArray;
                    double[] GpsLatArray;
                    double GpsLongDouble = 0;
                    double GpsLatDouble = 0;

                    if (reader.GetTagValue(ExifTags.GPSLongitude, out GpsLongArray)
                        && reader.GetTagValue(ExifTags.GPSLatitude, out GpsLatArray))
                    {
                        GpsLongDouble = GpsLongArray[0] + GpsLongArray[1]/60 + GpsLongArray[2]/3600;
                        GpsLatDouble = GpsLatArray[0] + GpsLatArray[1]/60 + GpsLatArray[2]/3600;
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
            var testImageName = TestData.TestImageName;
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                testImageName,
                new OptimizeUploadWaitRequest()
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
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
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(callbackUri)
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitUnknownDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            try
            {
                var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne),
                new FakeDataStore()
                );

                var result = response.Result;

                Assert.IsTrue(false, "No exception");
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void KrakenClient_OptimizeUnknownDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            try
            {
                var response = krakenClient.Optimize(
                 new Uri(TestData.ImageOne),
                 callbackUri,
                 new FakeDataStore()
                 );

                var result = response.Result;

                Assert.IsTrue(false, "No exception");
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitAzure_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne),
                new DataStore(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [TestMethod]
        public void KrakenClient_OptimizeCallbackAzure_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.Optimize(
                new Uri(TestData.ImageOne),
                callbackUri,
                new DataStore(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitAmazon_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new Uri(TestData.ImageOne),
                new SeaMist.Model.S3.DataStore(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void KrakenClient_OptimizeCallbackAmazon_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.Optimize(
                new Uri(TestData.ImageOne),
                callbackUri,
                new SeaMist.Model.S3.DataStore(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeWaitAzure_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadWaitRequest(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeWaitAzureDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadWaitRequest(
                    new DataStore(
                        Settings.AzureAccount,
                        Settings.AzureKey,
                        Settings.AzureContainer)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeWaitAzureWithPath_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadWaitRequest(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer,
                    "/test/" + TestData.TestImageName
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
            // Check path
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeCallbackAzure_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadRequest(
                    callbackUri,
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeCallbackAzureDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadRequest(
                    callbackUri,
                    new DataStore(
                        Settings.AzureAccount,
                        Settings.AzureKey,
                        Settings.AzureContainer)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeCallbackAzureWithPath_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadRequest(
                    callbackUri,
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer,
                    "/test/" + TestData.TestImageName
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
            // Check path
        }


        [TestMethod]
        public void KrakenClient_UploadOptimizeWaitAmazon_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                TestData.TestImageName,
                new SeaMist.Model.S3.OptimizeUploadWaitRequest(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeWaitAmazonDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                TestData.TestImageName,
                new SeaMist.Model.S3.OptimizeUploadWaitRequest(new SeaMist.Model.S3.DataStore(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeCallbackAmazon_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new SeaMist.Model.S3.OptimizeUploadRequest(
                    callbackUri,
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_UploadOptimizeCallbackAmazonDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new SeaMist.Model.S3.OptimizeUploadRequest(
                    callbackUri, new SeaMist.Model.S3.DataStore(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_CustomRequestUploadWait_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest
                {
                    ResizeImage = new ResizeImage {Height = 100, Width = 100},
                    WebP = true
                }
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
        }

        [TestMethod]
        public void KrakenClient_CustomRequestUploadCallback_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest
                {
                    CallbackUrl = callbackUri,
                    ResizeImage = new ResizeImage {Height = 100, Width = 100},
                    WebP = true
                }
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_CustomRequestUploadWaitAzure_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.OptimizeWait(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadWaitRequest(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                {
                    ResizeImage = new ResizeImage {Height = 100, Width = 100},
                    WebP = true
                }
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [TestMethod]
        public void KrakenClient_CustomRequestUploadCallbackAzure_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = krakenClient.Optimize(
                image,
                TestData.TestImageName,
                new SeaMist.Model.Azure.OptimizeUploadRequest(
                    callbackUri,
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                {
                    ResizeImage = new ResizeImage {Height = 100, Width = 100},
                    WebP = true
                }
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitAzureUsingIOptimizeWaitRequest_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new SeaMist.Model.Azure.OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitAzureUsingIOptimizeWaitRequestWithPath_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new SeaMist.Model.Azure.OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer,
                    "/test/" + TestData.TestImageName
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitAzureUsingIOptimizeWaitRequestDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new SeaMist.Model.Azure.OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    new DataStore(
                        Settings.AzureAccount,
                        Settings.AzureKey,
                        Settings.AzureContainer)
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitAmazonUsingIOptimizeWaitRequestDataStore_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new SeaMist.Model.S3.OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    new SeaMist.Model.S3.DataStore(
                        Settings.AmazonKey,
                        Settings.AmazonSecret,
                        Settings.AmazonBucket,
                        string.Empty
                        )
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void KrakenClient_OptimizeWaitAmazonUsingIOptimizeWaitRequest_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response = krakenClient.OptimizeWait(
                new SeaMist.Model.S3.OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }


        [TestMethod]
        public void KrakenClient_DocsSampleCodeOld_IsTrue()
        {
            var krakenClient = HelperFunctions.CreateWorkingClient();

            var response =  krakenClient.OptimizeWait(new SeaMist.Model.Azure.OptimizeWaitRequest()
            {
                ImageUrl = new Uri(TestData.ImageOne),
                Lossy = true,
                BlobStore = new DataStore(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
               )
            });

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }
    }
}