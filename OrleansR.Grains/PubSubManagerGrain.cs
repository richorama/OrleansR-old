using Orleans;
using OrleansR.GrainInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansR.Grains
{
    [Reentrant]
    public class PubSubManagerGrain : GrainBase, IPubSubManager
    {
        List<IPubSubGrain> subscribers;

        public override Task ActivateAsync()
        {
            this.subscribers = new List<IPubSubGrain>();
            return base.ActivateAsync();
        }

        public async Task<IPubSubGrain[]> Register(IPubSubGrain grainSubscriber)
        {
            if (!this.subscribers.Contains(grainSubscriber))
            {
                this.subscribers.Add(grainSubscriber);
            }
            var y = subscribers.ToArray();
            await Task.WhenAll(this.subscribers.Where(x => x != grainSubscriber).Select(x => x.TopologyChange(y)).ToArray());
            return y;
        }


        public Task Unregister(IPubSubGrain grainSubscriber)
        {
            if (!this.subscribers.Contains(grainSubscriber))
            {
                this.subscribers.Remove(grainSubscriber);
            }
            var y = subscribers.ToArray();
            return Task.WhenAll(this.subscribers.Select(x => x.TopologyChange(y)).ToArray());
        }
    }
}
