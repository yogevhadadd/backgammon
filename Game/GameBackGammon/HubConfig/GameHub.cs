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
            await StartNewGame();
            await base.OnConnectedAsync();
        }
        public async Task Delete()
        {
            Start();
            if (ChackMyTurn())
            {
                gameService.Reset();
                await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("Delete", gameService.boardTemporary.board,
                    gameService.whiteListTemporary, gameService.blackListTemporary, gameService.cubesTemporary);
            }
        }
        public async Task MyTurn(int from, int to)
        {
            Start();
            if (ChackMyTurn())
            {
                Turn turn = new() { From = from, To = to };
                gameService.PlayTurn(turn);
                await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("PlayerTurn", gameService.boardTemporary.board,
                    gameService.whiteListTemporary, gameService.blackListTemporary, gameService.cubesTemporary,
                    oneGame.FirstName, gameService.showOnMoveTemp);
                if (gameService.EndGame())
                {
                    await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("EndGame", gameService.colorPlayer);
                }
                if (gameService.FinishTurn())
                {
                    await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("FinishTurn");
                    SaveGame();
                }
                else
                {
                    await PassTurn();
                }
            }
        }
        public async Task RoleCubes()
        {
            Start();
            if (ChackMyTurn())
            {
                await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("role", gameService.RoleCube());
                Thread.Sleep(3000);
                await PassTurn();
            }
        }
        public async Task OnMove(int num)
        {
            Start();
            if (ChackMyTurn())
            {
                gameService.OnMove(num);
                await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("OnMove", gameService.showOnMoveTemp);
            }
        }
        private void Start()
        {
            if (Context != null)
            {
                var getOneGame = from oneGame in listOfGames
                                 where oneGame.FirstName == Context.User.Identity.Name || oneGame.SecondName == Context.User.Identity.Name
                                 select oneGame;
                if (getOneGame.Count() > 0)
                {
                    oneGame = getOneGame.First();
                    gameService = oneGame.GameService;
                    return;
                }
                else
                {
                    oneGame = GetOneGame();
                    gameService = new GameService();
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
        private OneGame GetOneGame()
        {
            var findGame = new OneGame() { FirstName = Context.User.Identity.Name };
            var client = new RestClient("https://localhost:7195/");
            var request = new RestRequest("GameController/GetTheGame", Method.Post);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(findGame);
            var myGame = client.Execute(request);
            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            return JsonSerializer.Deserialize<OneGame>(myGame.Content, options);
        }
        private async Task PassTurn()
        {
            Start();
            if (!gameService.CanMove())
            {
                SaveGame();
                await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("CanMove", gameService.cubesTemporary);
            }
        }
        private async Task StartNewGame()
        {
            Start();
            await Clients.Users(oneGame.FirstUserName, oneGame.SecondUserName).SendAsync("startGame",gameService.boardTemporary.board,
                gameService.whiteListTemporary, gameService.blackListTemporary, gameService.cubesTemporary, oneGame.FirstName,
                oneGame.FirstPic,oneGame.SecondPic);
        }
        private bool ChackMyTurn()
        {
            if(gameService.colorPlayer == "black" && Context.User.Identity.Name == oneGame.FirstName)
            {
                return true;
            }
            if (gameService.colorPlayer == "white" && Context.User.Identity.Name == oneGame.SecondName)
            {
                return true;
            }
            return false;
        }
    }
}
