using System;
using TalkBackAccessControll.Date.Models;

namespace TalkBackAccessControll.Date.Services
{
    public interface ILoginService
    {
        Token Login(LoginRequest loginRequest);
        Token Refresh(Token tokenApiModel);
    }
}
