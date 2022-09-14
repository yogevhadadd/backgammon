using Chat.Model;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiController]
    [Route("Chat")]
    public class ChatController : Controller
    {
        static readonly List<OneChat> chates = new List<OneChat>();
        [HttpPost]
        public void Post(OneChat oneChat)
        {
            if (chates.Find(a => a.Equals(oneChat)) == null)
            {
                chates.Add(oneChat);
            }
        }
        [HttpPost]
        [Route("GetTheChat")]
        public OneChat Get(OneChat status)
        {
            var findChat = from chat in chates
                                       where chat.FirstName == status.FirstName || chat.SecondName == status.FirstName
                                       select chat;
            if (findChat.Count() > 0)
            {
                return findChat.First();
            }
            return status;
        }
    }
}
