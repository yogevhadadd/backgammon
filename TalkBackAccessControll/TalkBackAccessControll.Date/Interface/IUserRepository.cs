using TalkBackAccessControll.Date.Models;

namespace TalkBackAccessControll.Date.Services
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();
        User Get(string username);
        User Add(User user);
        User DisplayName(string displayName);
    }
}
