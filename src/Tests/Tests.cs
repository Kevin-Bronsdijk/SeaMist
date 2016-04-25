using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaMist;
using SeaMist.Http;
using SeaMist.Model;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ConnectionCreate_EmptyKeyError_IsTrue()
        {
            try
            {
                KrakenConnection.Create("", "secret");
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "Exception");
            }
        }

        [TestMethod]
        public void ConnectionCreate_NullKeyError_IsTrue()
        {
            try
            {
                KrakenConnection.Create(null, "secret");
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "Exception");
            }
        }

        [TestMethod]
        public void ConnectionCreate_EmptySecretError_IsTrue()
        {
            try
            {
                KrakenConnection.Create("key", "");
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "Exception");
            }
        }

        [TestMethod]
        public void ConnectionCreate_NullSecretError_IsTrue()
        {
            try
            {
                KrakenConnection.Create("key", null);
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "Exception");
            }
        }

        [TestMethod]
        public void KrakenClient_NullConnectionError_IsTrue()
        {
            try
            {
                new KrakenClient(null);
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "Exception");
            }
        }

        [TestMethod]
        public void KrakenClient_NoErrors_IsTrue()
        {
            try
            {
                var connection = KrakenConnection.Create("key", "secret");
                var krakenClient = new KrakenClient(connection);

                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "Exception");
            }
        }

        [TestMethod]
        public void KrakenClient_IsSandboxMode_IsTrue()
        {
            var connection = KrakenConnection.Create("key", "secret", true);

            Assert.IsTrue(connection.SandboxMode);
        }

        [TestMethod]
        public void KrakenClient_NotInSandboxMode_IsTrue()
        {
            var connection = KrakenConnection.Create("key", "secret");

            Assert.IsTrue(connection.SandboxMode == false);
        }

        [TestMethod]
        public void KrakenClient_NotInSandboxModeExpl_IsTrue()
        {
            var connection = KrakenConnection.Create("key", "secret", false);

            Assert.IsTrue(connection.SandboxMode == false);
        }

        [TestMethod]
        public void ConnectionCreate_Dispose_IsTrue()
        {
            var krakenConnection = KrakenConnection.Create("key", "secret");

            try
            {
                krakenConnection.Dispose();

                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "Exception");
            }
        }

        [TestMethod]
        public void KrakenClient_MustProvideAConnection_IsTrue()
        {
            try
            {
                var krakenClient = new KrakenClient(null);

                Assert.IsTrue(false, "No exception");
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void KrakenClient_Dispose_IsTrue()
        {
            var krakenConnection = KrakenConnection.Create("key", "secret");
            var krakenClient = new KrakenClient(krakenConnection);

            try
            {
                krakenClient.Dispose();

                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "Exception");
            }
        }

        [TestMethod]
        public void KrakenClient_RequestUploadWaitNoFileNameError_IsTrue()
        {
            var krakenConnection = KrakenConnection.Create("key", "secret");
            var krakenClient = new KrakenClient(krakenConnection);

            try
            {
                krakenClient.OptimizeWait(
                    null,
                    string.Empty,
                    new OptimizeUploadWaitRequest()
                    );

                Assert.IsTrue(false, "No exception");
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void KrakenClient_RequestUploadCallbackNoFileNameError_IsTrue()
        {
            var krakenConnection = KrakenConnection.Create("key", "secret");
            var krakenClient = new KrakenClient(krakenConnection);

            try
            {
                krakenClient.Optimize(
                    null,
                    string.Empty,
                    new OptimizeUploadRequest()
                    );

                Assert.IsTrue(false, "No exception");
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

    }
}