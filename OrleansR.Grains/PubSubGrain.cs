using Orleans;
using OrleansR.GrainInterfaces;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Concurrency;

namespace OrleansR.Grains
{
    [Reentrant]
    public class PubSubGrain : Orleans.Grain, IPubSubGrain
    {
        ObserverSubscriptionManager<IMessageObserver> observers;
        IPubSubGrain[] otherGrains;
        ulong index = 0;

        public async override Task ActivateAsync()
        {
            this.observers = new ObserverSubscriptionManager<IMessageObserver>();
            this.otherGrains = new IPubSubGrain[] { };

            var manager = PubSubManagerFactory.GetGrain(0);
            this.otherGrains = await manager.Register(this);

            await base.ActivateAsync();
        }

        public async Task Publish(Microsoft.AspNet.SignalR.Messaging.Message[] messages)
        {
            await Task.WhenAll(this.otherGrains.Select(x => x.Notify(messages)).ToArray());
        }

        public Task Subscribe(IMessageObserver observer)
        {
            this.observers.Subscribe(observer);
            return TaskDone.Done;
        }

        public async Task Unsubscribe(IMessageObserver observer)
        {
            this.observers.Unsubscribe(observer);
            if (this.observers.Count == 0)
            {
                var manager = PubSubManagerFactory.GetGrain(0);
                await manager.Unregister(this);
                this.DeactivateOnIdle(); // remove this grain
            }
        }

        public Task Notify(Microsoft.AspNet.SignalR.Messaging.Message[] messages)
        {
            foreach (var message in messages)
            {
                message.MappingId = index++;
            }

            this.observers.Notify(x => x.Send(messages));
            return TaskDone.Done;
        }

        public Task TopologyChange(IPubSubGrain[] otherGrains)
        {
            this.otherGrains = otherGrains;
            return TaskDone.Done;
        }

        public override Task DeactivateAsync()
        {
            var manager = PubSubManagerFactory.GetGrain(0);
            return manager.Unregister(this);

        }
    }
}
