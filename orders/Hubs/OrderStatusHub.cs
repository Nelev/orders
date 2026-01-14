using Microsoft.AspNetCore.SignalR;

namespace orders.Hubs
{
    public class OrderStatusHub: Hub
    {
        public async Task SendMessage(string user, string message) { 
            await Clients.All.SendAsync("ReceiveMessage", user, message); 
        }

        public async Task Ping() {
            Console.WriteLine("ping recieved");
            await Clients.Caller.SendAsync("Pong", "pong from server"); 
        }
    }
}
