using Chat.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Chat.HubConfig
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static List<OneChat> chats= new List<OneChat>();
        private static OneChat chat= new OneChat();
        public override Task OnConnectedAsync()
        {
            GetSpecificChat();
            Start();
            return Task.CompletedTask;
        }
        private async void Start()
        {
            await Clients.Users(chat.FirstUserName, chat.SecondUserName).SendAsync("ReceiveMessage", chat.Chat,chat.FirstPic,chat.SecondPic,chat.FirstName,chat.SecondName);
        }
        private void GetSpecificChat()
        {
            if(Context != null)
            {
                var getOneChat = from oneChat in chats
                                 where oneChat.FirstName == Context.User.Identity.Name || oneChat.SecondName == Context.User.Identity.Name
                                 select oneChat;
                if (getOneChat.Count() > 0)
                {
                    chat = getOneChat.First();
                    return;
                }
                else
                {
                    chat = GetOneChat();
                    chats.Add(chat);
                }
            }
        }
        private OneChat GetOneChat()
        {
            var findGame = new OneChat() { FirstName = Context.User.Identity.Name };
            var client = new RestClient("https://localhost:44347/");
            var request = new RestRequest("Chat/GetTheChat", Method.Post);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(findGame);
            var myGame = client.Execute(request);
            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            var result = JsonSerializer.Deserialize<OneChat>(myGame.Content);
            return result;
        }
        private void Save()
        {
            var getOneChat = from oneChat in chats
                             where oneChat.FirstName == Context.User.Identity.Name || oneChat.SecondName == Context.User.Identity.Name
                             select oneChat;
            if (getOneChat.Count() > 0)
            {
                var aaaaa = getOneChat.First();
                aaaaa = chat;
                return;
            }
        }
        public async Task SendMessage(string message)
        {
            GetSpecificChat();
            chat.Chat.Add( new ChatMessage() {fromUser = Context.User.Identity.Name, Message = message});
            Save();
            await Clients.Users(chat.FirstUserName, chat.SecondUserName).SendAsync("ReceiveMessage", chat.Chat, chat.FirstPic, chat.SecondPic, chat.FirstName, chat.SecondName);
        }
    }
}
