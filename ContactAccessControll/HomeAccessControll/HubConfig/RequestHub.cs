using HomeAccessControll.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestSharp;

namespace HomeAccessControll.HubConfig
{
    [Authorize]
    public class RequestHub : Hub
    {
        private static List<SendUser> connectList;
        public RequestHub()
        {
            if (connectList == null)
            {
                connectList = new();
                var client = new RestClient("https://localhost:7048/");
                var request = new RestRequest("user", Method.Get);
                connectList = client.Execute<List<User>>(request).Data.Select(name => new SendUser(){DisplayName = name.DisplayName, ProfilePic= name.ProfilePic, Online = false, UserName = name.UserName }).ToList();
            }
        }
        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            var result = connectList.Find(x => x.DisplayName == username);
            if (result == null)
            {
                var client = new RestClient("https://localhost:7048/");
                var request = new RestRequest("user", Method.Get);
                connectList.Add(client.Execute<List<User>>(request).Data.Select(name => new SendUser() { DisplayName = name.DisplayName, ProfilePic = name.ProfilePic, Online = true, UserName = name.UserName })
                    .Where(name => name.DisplayName == username).First());
            }
            else
            {
                result.Online = true;
            }
            await GetListConnect();
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            var result = connectList.Find(user => user.DisplayName == username);
            result.Online = false;
            await GetListConnect();
            await base.OnDisconnectedAsync(exception);
        }
        private async Task PostGameAsync(OneGame oneGame)
        {
            var client = new RestClient("https://localhost:7195/");
            var request = new RestRequest("GameController", Method.Post);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(oneGame);
            client.Execute(request);
        }
        private async Task PostChatAsync(OneChat oneChat)
        {
            var client = new RestClient("https://localhost:44347/");
            var request = new RestRequest("Chat", Method.Post);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(oneChat);
            client.Execute(request);
        }
        public async Task GetListConnect()
        {
                await Clients.All.SendAsync("SendListConnect", connectList);
        }
        public async Task SendGameRequest(string nickName)
        {
                await Clients.User(GetUserWithNickName(nickName)).SendAsync("ReceiveGameRequest", Context.User.Identity.Name);
        }
        public async Task AcceptRequestGame(string nickName)
        {
            var client = new RestClient("https://localhost:7048/");
            var request = new RestRequest("user", Method.Get);
            var picMyUser = client.Execute<List<User>>(request).Data
                .Select(name => new SendUser() { DisplayName = name.DisplayName, ProfilePic = name.ProfilePic, Online = true })
                .Where(name => name.DisplayName == Context?.User?.Identity?.Name).First();
            var picSecondUser = client.Execute<List<User>>(request).Data
                .Select(name => new SendUser() { DisplayName = name.DisplayName, ProfilePic = name.ProfilePic, Online = true })
                .Where(name => name.DisplayName == nickName).First();
            OneGame oneGame = new OneGame() { FirstName = Context.User.Identity.Name, SecondName = nickName, FirstPic = picMyUser.ProfilePic, SecondPic = picSecondUser.ProfilePic ,FirstUserName = GetUserWithNickName(Context?.User?.Identity?.Name), SecondUserName = GetUserWithNickName(nickName)};
            OneChat oneChat = new OneChat() { FirstName = Context?.User?.Identity?.Name, SecondName = nickName, FirstPic = picMyUser.ProfilePic, SecondPic = picSecondUser.ProfilePic, FirstUserName = GetUserWithNickName(Context?.User?.Identity?.Name), SecondUserName = GetUserWithNickName(nickName)};
            await PostGameAsync(oneGame);
            await PostChatAsync(oneChat);
            await Clients.Users(GetUserWithNickName(Context.User.Identity.Name), GetUserWithNickName(nickName)).SendAsync("AcceptGameRequest");
        }
        private static string GetUserWithNickName(string nickNamePlayer)
        {
            return connectList.Where(user => user.DisplayName == nickNamePlayer).First().UserName;
        }
    }
}