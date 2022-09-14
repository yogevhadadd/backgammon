using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TalkBackAccessControll.Date.Models;
using TalkBackAccessControll.Date.Services;

namespace TalkBackAccessControll.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserasController : ControllerBase
    {
        private readonly IUserRepository _repositoriy;
        public UserasController(IUserRepository repositoriy)
        {
            _repositoriy = repositoriy;
        }
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _repositoriy.GetAll();
        }
        [HttpGet("{displayName}")]
        public User DisplayName(string displayName)
        {
            return _repositoriy.DisplayName(displayName);
        }
        [HttpPost]
        public User Post(User value)
        {
            return _repositoriy.Add(value);
        }
    }
}


