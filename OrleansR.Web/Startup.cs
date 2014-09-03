using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using OrleansR.MessageBus;
using Owin;

namespace OrleansR.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new ScaleoutConfiguration { MaxQueueLength = 10, QueueBehavior = QueuingBehavior.Always };
            var omb = new OrleansMessageBus(GlobalHost.DependencyResolver, config);
            GlobalHost.DependencyResolver.Register(typeof(IMessageBus), () => omb);
            app.MapSignalR();
        }
    }
}