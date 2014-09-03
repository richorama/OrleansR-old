using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using OrleansR.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansR.MessageBus
{
    public class OrleansMessageBus : ScaleoutMessageBus
    {
        IPubSubGrain grain;
        MessageObserver subscriber;
        IMessageObserver subscriberReference;

        protected override int StreamCount
        {
            get
            {
                return 1;
            }
        }

        public OrleansMessageBus(IDependencyResolver resolver, ScaleoutConfiguration configuration)
            : base(resolver, configuration)
        {
            grain = PubSubGrainFactory.GetGrain(Guid.NewGuid());

            subscriber = new MessageObserver((messages) =>
            {
                lock (grain)
                {
                    OnReceived(messages[0].StreamIndex, messages.Select(x => x.MappingId).Max(), new ScaleoutMessage(messages));
                }
            });
            subscriberReference = MessageObserverFactory.CreateObjectReference(subscriber).Result;
            grain.Subscribe(subscriberReference).Wait();
        }

        protected override Task Send(int streamIndex, IList<Message> messages)
        {
            return grain.Publish(messages.ToArray());
        }

        protected override Task Send(IList<Message> messages)
        {
            return grain.Publish(messages.ToArray());
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                grain.Unsubscribe(subscriberReference).Wait();
            }
            catch { }
            base.Dispose(disposing);
        }
    }

}
