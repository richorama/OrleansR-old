using Orleans;
using System.Threading.Tasks;

namespace OrleansR.GrainInterfaces
{
    public interface IPubSubManager : IGrain
    {
        Task<IPubSubGrain[]> Register(IPubSubGrain grainSubscriber);

        Task Unregister(IPubSubGrain grainSubscriber);

    }
}
