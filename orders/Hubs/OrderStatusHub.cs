using Microsoft.AspNetCore.SignalR;
using orders.Model;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace orders.Hubs
{
    public class OrderStatusHub: Hub
    {
        double baseX = 30.0; 
        double baseY = 30.0;
        public async Task SendMessage(string user, string message) { 
            await Clients.All.SendAsync("ReceiveMessage", user, message); 
        }


        public async Task SendRandomPosition() {
            while (true) 
            { 
                var pos = GetRandomPosition(); 
                await Clients.Caller.SendAsync("ReceivePosition", pos);
                await Task.Delay(2000); 
            } 
        }

        private object GetRandomPosition()
        {
            var rand = new Random();
            double offsetX = (rand.NextDouble() - 0.5) * 0.01;
            double offsetY = (rand.NextDouble() - 0.5) * 0.01; 
            double newX = baseX + offsetX;
            double newY = baseY + offsetY;
            return new Position
            {
                X = newX,
                Y = newY
            };
        }
    }
}
