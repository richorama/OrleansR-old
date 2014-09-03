using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orleans.Host.SiloHost;
using OrleansR.GrainInterfaces;
using OrleansR.MessageBus;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OrleansR.UnitTests
{
    [TestClass]
    public class GrainTests
    {

        [TestMethod]
        public async Task SingleGrainTest()
        {
            Message[] messages = null;
            var grain = PubSubGrainFactory.GetGrain(0, "");
            await grain.Subscribe(await MessageObserverFactory.CreateObjectReference(new MessageObserver(x => messages = x)));
            await grain.Publish(new Message[] { new Message("x", "y", "Hello World") });
            Thread.Sleep(50);
            Assert.IsNotNull(messages);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual("x", messages[0].Source);
            Assert.AreEqual("y", messages[0].Key);
        }


        [TestMethod]
        public async Task DoubleGrainTest()
        {
            Message[] messages = null;
            var grain1 = PubSubGrainFactory.GetGrain(1);
            await grain1.Subscribe(await MessageObserverFactory.CreateObjectReference(new MessageObserver(x => messages = x)));

            var grain2 = PubSubGrainFactory.GetGrain(2);
            await grain2.Publish(new Message[] { new Message("a", "b", "Hello World") });

            Thread.Sleep(10);
            Assert.IsNotNull(messages);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual("a", messages[0].Source);
            Assert.AreEqual("b", messages[0].Key);
        }

        // code to initialize and clean up an Orleans Silo

        static OrleansSiloHost siloHost;
        static AppDomain hostDomain;

        static void InitSilo(string[] args)
        {
            siloHost = new OrleansSiloHost("test");
            siloHost.ConfigFileName = "DevTestServerConfiguration.xml";
            siloHost.DeploymentId = "1";
            siloHost.InitializeOrleansSilo();
            var ok = siloHost.StartOrleansSilo();
            if (!ok) throw new SystemException(string.Format("Failed to start Orleans silo '{0}' as a {1} node.", siloHost.SiloName, siloHost.SiloType));
        }

        [ClassInitialize]
        public static void GrainTestsClassInitialize(TestContext testContext)
        {

            hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            });

            Orleans.OrleansClient.Initialize("DevTestClientConfiguration.xml");
        }

        [ClassCleanup]
        public static void GrainTestsClassCleanUp()
        {
            try
            {
                hostDomain.DoCallBack(() =>
                {
                    siloHost.Dispose();
                    siloHost = null;
                    //AppDomain.Unload(hostDomain);
                });

                // vstest execution engine must die
                var startInfo = new ProcessStartInfo
                {
                    FileName = "taskkill",
                    Arguments = "/F /IM vstest.executionengine.x86.exe",
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                };
                //Process.Start(startInfo);
            }
            catch
            { }

        }
    }
}
