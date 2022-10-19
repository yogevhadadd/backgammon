using HomeAccessControll.Data.Interface;
using HomeAccessControll.Data.Model;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeAccessControll.Controllers
{
    [ApiController]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IHomeRepositoriy _repositoriy;
        public HomeController(IHomeRepositoriy repositoriy)
        {
            _repositoriy = repositoriy;
        }
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _repositoriy.GetAllRegistarUser();
        }
        [HttpGet]
        [Route("GetAllConnectedUsers")]
        public IEnumerable<User> GetAllConnectedUsers()
        {
            return _repositoriy.GetAllConnectedUsers();
        }
        [HttpPost]
        [Route("PostChat")]
        public void PostChat([FromBody] DisplayName value)
        {
            var findGame = new OneChat() { FirstName = value.displayName };
            var client = new RestClient("https://localhost:44347/");
            var request = new RestRequest("Chat/GetTheChat", Method.Post);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(findGame);
            var myGame = client.Execute(request);
            //var options = new JsonSerializerOptions
            //{
            //    NumberHandling = JsonNumberHandling.AllowReadingFromString
            //};
            //var result = JsonSerializer.Deserialize<OneChat>(myGame.Content);
        }
    }
}
