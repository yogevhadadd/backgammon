using Game.model;
using Game.Service;
using GameBackGammon.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Game.HubConfig
{
    [Authorize]
    public class GameHub :Hub
    {
        public static List<OneGame> listOfGames  = new List<OneGame>();
        private GameService gameService;
        private OneGame oneGame = new OneGame();
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public async Task Delete(string displayName)
        {
            Start(displayName);
            if (ChackMyTurn() )
            {
                gameService.Reset();
                await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("Delete", gameService.boardTemporary.board,
                    gameService.whiteListTemporary, gameService.blackListTemporary, gameService.cubesTemporary);
            }
        }
        public async Task MyTurn(int from, int to, string displayName)
        {
            Start(displayName);
            if (ChackMyTurn())
            {
                Turn turn = new() { From = from, To = to };
                gameService.PlayTurn(turn);
                await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("PlayerTurn", gameService.boardTemporary.board,
                    gameService.whiteListTemporary, gameService.blackListTemporary, gameService.cubesTemporary,
                    oneGame.FirstName, gameService.showOnMoveTemp);
                if (gameService.EndGame())
                {
                    await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("EndGame", gameService.colorPlayer);
                }
                if (gameService.FinishTurn())
                {
                    await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("FinishTurn");
                    SaveGame();
                }
                else
                {
                    await PassTurn(displayName);
                }
            }
        }
        public async Task RoleCubes(string displayName)
        {
            Start(displayName);
            for (int i = 0; i < 4; i++)
            {
                if (gameService.cubesTemporary[1] != 0) return;
            }
            if (ChackMyTurn())
            {
                await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("role", gameService.RoleCube());
                Thread.Sleep(3000);
                await PassTurn(displayName);
            }
        }
        public async Task OnMove(int num, string displayName)
        {
            Start(displayName);
            if (ChackMyTurn())
            {
                gameService.OnMove(num);
                await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("OnMove", gameService.showOnMoveTemp);
            }
        }
        private void Start(string displayName)
        {
            if (Context != null)
            {
                var getOneGame = from oneGame in listOfGames
                                 where oneGame.FirstName == Context.User.Identity.Name && oneGame.SecondName == displayName ||
                                    oneGame.FirstName == displayName && oneGame.SecondName == Context.User.Identity.Name
                                 select oneGame;
                if (getOneGame.Count() > 0)
                {
                    oneGame = getOneGame.First();
                    gameService = oneGame.GameService;
                    return;
                }
                else
                {
                    oneGame = new OneGame { FirstName = displayName ,SecondName = Context.User.Identity.Name, Name = Context.User.Identity.Name};
                    gameService = new GameService();
                    gameService.displayNameFirst = displayName;
                    gameService.displayNameSecond = Context.User.Identity.Name;
                    gameService.displayNameTurn = Context.User.Identity.Name;
                    oneGame.GameService = gameService;
                   
                    listOfGames.Add(oneGame);
                }
            }
        }
        private void SaveGame()
        {
            var getOneGame = from oneGame in listOfGames
                             where oneGame.FirstName == Context.User.Identity.Name || oneGame.SecondName == Context.User.Identity.Name
                             select oneGame;
            if (getOneGame.Count() > 0)
            {
                var aaaaa = getOneGame.First();
                aaaaa = oneGame;
                return;
            }
        }
        private async Task PassTurn(string displayName)
        {
            Start(displayName);
            if (!gameService.CanMove())
            {
                gameService.FinishTurn();
                await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("CanMove", gameService.cubesTemporary);
            }
        }
        public async Task StartNewGame(string displayName)
        {
            Start(displayName);
            await Clients.Users(oneGame.FirstName, oneGame.SecondName).SendAsync("startGame",gameService.boardTemporary.board,
                gameService.whiteListTemporary, gameService.blackListTemporary, gameService.cubesTemporary, oneGame.SecondName);
        }
        private bool ChackMyTurn()
        {
            if(gameService.colorPlayer == "black" && Context.User.Identity.Name == oneGame.SecondName && gameService.GetTurn(Context.User.Identity.Name))
            {
                return true;
            }
            if (gameService.colorPlayer == "white" && Context.User.Identity.Name == oneGame.FirstName && gameService.GetTurn(Context.User.Identity.Name))
            {
                return true;
            }
            return false;
        }
    }
}
