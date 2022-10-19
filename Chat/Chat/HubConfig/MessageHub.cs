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
        private static readonly List<OneChat> chats= new ();
        private static OneChat chat= new ();
        
        private void GetSpecificChat(string displayName)
        {
            if(Context != null)
            {
                var getOneChat = from oneChat in chats
                                 where oneChat.FirstName == Context.User.Identity.Name && oneChat.SecondName == displayName ||
                                 oneChat.FirstName == displayName && oneChat.SecondName == Context.User.Identity.Name
                                 select oneChat;
                if (getOneChat.Count() > 0)
                {
                    chat = getOneChat.First();
                    return;
                }
                else
                {
                    chat = new OneChat() { FirstName = Context.User.Identity.Name, SecondName = displayName };

                    chats.Add(chat);
                }
            }
        }
        
        public async Task SendMessage(string message, string displayName)
        {
            GetSpecificChat(displayName);
            chat.Chat.Add( new ChatMessage() {fromUser = Context.User.Identity.Name, Message = message});
            await Clients.Users(Context.User.Identity.Name, displayName).SendAsync("ReceiveMessage", chat.Chat, Context.User.Identity.Name, displayName);
        }

        public async Task GetChat(string message,string img)
        {
            GetSpecificChat(message);
            chat.FirstPic = img;
            await Clients.Users(Context.User.Identity.Name).SendAsync("SendChat", chat.Chat, chat.FirstPic, chat.SecondName);
        }
    }
}
