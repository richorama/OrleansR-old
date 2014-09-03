using Microsoft.AspNet.SignalR;

namespace OrleansR.Web.Controllers
{
    public class TestHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.broadcastMessage(message);
        }
    }
}