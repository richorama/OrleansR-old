using Microsoft.AspNet.SignalR.Messaging;
using OrleansR.GrainInterfaces;
using System;

namespace OrleansR.MessageBus
{
    public class MessageObserver : IMessageObserver
    {
        Action<Message[]> action;

        public MessageObserver(Action<Message[]> action)
        {
            this.action = action;
        }

        public void Send(Message[] message)
        {
            this.action(message);
        }
    }
}
