using GameBackGammon.model;
using Microsoft.AspNetCore.Mvc;
namespace GameBackGammon.Controllers
{
    [ApiController]
    [Route("GameController")]
    public class GameController : Controller
    {
        static List<OneGame> games = new List<OneGame>();
       
        [HttpPost]
        public void Post(OneGame status)
        {
            if (games.Find(a => a.Equals(status)) == null)
            {
                games.Add(status);
            }
        }
        [HttpPost]
        [Route("GetTheGame")]
        public OneGame Get(OneGame status)
        {
            var findGame = from game in games
                                where game.FirstName == status.FirstName || game.SecondName == status.FirstName
                                select game;
            if (findGame.Count() > 0)
            {
                return findGame.First();
            }
            return status;
        }
    }
}
