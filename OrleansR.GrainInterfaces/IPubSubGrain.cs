using Microsoft.AspNet.SignalR.Messaging;
using Orleans;
using System.Threading.Tasks;

namespace OrleansR.GrainInterfaces
{
    //[StatelessWorker]
    //[ExtendedPrimaryKey]
    public interface IPubSubGrain : IGrain
    {

        Task Publish(Message[] messages);

        Task Subscribe(IMessageObserver observer);

        Task Unsubscribe(IMessageObserver observer);

        Task Notify(Message[] messages);

        Task TopologyChange(IPubSubGrain[] otherGrains);

    }
}
