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
        
    }
}
