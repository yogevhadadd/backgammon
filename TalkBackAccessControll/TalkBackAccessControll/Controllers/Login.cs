using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using TalkBackAccessControll.Date.Models;
using TalkBackAccessControll.Date.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace TalkBackAccessControll.Controllers
{
    [Route("LoginRegistar")]
    public class LoginRegistar : Controller
    { 
        private readonly IUserRepository _repositoriy;
        private readonly ILoginService _loginService;
        readonly ProfilePic profilePic = new();
        public LoginRegistar(IUserRepository repositoriy,ILoginService loginService)
        {
            _repositoriy = repositoriy;
            _loginService = loginService;
        }

        [HttpPost]
        [Route("Registar")]
        public IActionResult Registar([FromBody] User value)
        {
            if (_repositoriy.Add(value).UserName != null)
            {
                return Login(new LoginRequest() { UserName = value.UserName, Password = value.Passwors });
            }
            else return Ok();
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadImg")]
        public async Task<IActionResult> UploadImg()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = "C:\\Users\\Yogev\\source\\repos\\ProjectBackGammon\\Client\\src\\assets\\img";
                var pathToSaveUser = "assets\\img";
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var fullPathUser = Path.Combine(pathToSaveUser, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    profilePic.profilePic = fullPathUser;
                    return Ok(profilePic);
                }
            }
            catch
            {
                return Ok(profilePic);
            }
            return Ok(profilePic);
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest value)
        {
            return Ok(_loginService.Login(value)); 
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] Token tokenApiModel)
        {
            var token = _loginService.Refresh(tokenApiModel);
            return Ok(token);
        }
    }
}
