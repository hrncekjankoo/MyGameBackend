using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MyGame.Services
{
    //[Authorize]
    
    public class MatchHub : Hub
    {
        public MatchHub()
        {
        }

        public override async Task OnConnectedAsync()
        {                
            await Clients.Caller.SendAsync("ReceiveMessage","Connected to backend Succesfully.");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {                
            await Clients.Caller.SendAsync("ReceiveMessage","Disconnected succesfully.");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public async Task GetPlayers()
        {
            Random rnd = new Random();

            UInt16 Limit = (ushort)rnd.Next(0, 1000);
            
            await Clients.Caller.SendAsync("ReceivePlayers", Limit);
            
        }

        public async Task Move()
        {
            Random rnd = new Random();

            Vector3 Location = new Vector3();
            Location.X = rnd.Next(0, 1000);
            Location.Y = rnd.Next(0, 1000);
            Location.Z = rnd.Next(0, 1000);

            await Clients.Caller.SendAsync("ReceivePosition", Location);
        }

        public async Task Fire()
        {
            //Call some backend methods here
        }

        /*public async Task SendFixture(string homeTeam, string awayTeam, int homeGoals, int awayGoals)
        {
            await Clients.Caller.SendAsync("ReceiveFixture", $"{homeTeam}-{awayTeam} {homeGoals}:{awayGoals}");
        }*/
    }
}
