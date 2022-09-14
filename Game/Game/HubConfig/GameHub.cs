using Game.model;
using Game.Service;
using Microsoft.AspNetCore.SignalR;

namespace Game.HubConfig
{
    public class GameHub:Hub
    {
        GameService gameService = new GameService();
        public async Task MyTurn(Turn[] turns)
        {
            await Clients.All.SendAsync("PlayerTurn", gameService.PlayTurn(turns));
        }
        public async Task RoleCubes(string toUser, string fromUser, string message)
        {
            //await Clients.All.SendAsync("RoleCubes", newMessage);
        }

    }
}
