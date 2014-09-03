# OrleansR

A SignalR backplane implemented in Orleans

## Motivation

When running SignalR on more than one web server (a common scenario in the cloud) you need a backplane to connect together each of the servers. This allows messages broadcast from a user connected to one server, to reach the users on the other servers.

There are already some backplane options in Azure:

* SQL Server
* Service Bus Topics/Subscriptions
* Redis (you should probably use this)

I thought it would be interesting to implement a backplane in Orleans.

## Project Structure

The projects required to implement a system:

* __OrleansR.GrainInterfaces__ The Oreans grain interfaces.
* __OrleansR.Grains__ The Orleans grain implementations.
* __OrleansR.MessageBus__ The `IMessageBus` implementation which plugs into SignalR, and connects to the Orleans Silos.

Sample/test projects included in the solution:

* __OrleansR.Cloud__ A sample project to deploy the solution to Azure.
* __OrleansR.UnitTests__ Unit tests for the grains.
* __OrleansR.Web__ A sample web application.
* __OrleansR.WorkerRoleSiloHost__ A sample Worker Role to host the Orleans Silo.

## Usage

Configure Orleans in the usual way, by starting up a Silo, and connecting a web application to it as described [here](http://orleans.codeplex.com/wikipage?title=Front%20Ends%20for%20Orleans%20Services&referringTitle=Step-by-step%20Tutorials).

When configuring SignalR in the `Startup` class of your web application, register the `OrleanMessageBus` class like this:

```cs
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
 			// configure the OrleansR message bus
            var config = new ScaleoutConfiguration { MaxQueueLength = 10, QueueBehavior = QueuingBehavior.Always };
            var omb = new OrleansMessageBus(GlobalHost.DependencyResolver, config);
            GlobalHost.DependencyResolver.Register(typeof(IMessageBus), () => omb);

            // register SignalR as ususal
            app.MapSignalR();
        }
    }
}
```

## License 

MIT