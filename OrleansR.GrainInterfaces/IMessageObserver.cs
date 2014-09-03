using Microsoft.AspNet.SignalR.Messaging;
using Orleans;

namespace OrleansR.GrainInterfaces
{
    public interface IMessageObserver : IGrainObserver
    {
        void Send(Message[] message);
    }
}
