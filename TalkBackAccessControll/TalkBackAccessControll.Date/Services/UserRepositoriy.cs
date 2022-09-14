using TalkBackAccessControll.Date.DB;
using TalkBackAccessControll.Date.Models;

namespace TalkBackAccessControll.Date.Services
{
    public class UserRepositoriy : IUserRepository
    {
        private readonly TalkBackDbContext _context;
        public UserRepositoriy(TalkBackDbContext talkBackDBContext)
        {
            _context = talkBackDBContext;
        }
        public User Add(User user)
        {
            if(ChackProperty(user))
            {
                if(_context.Users.FirstOrDefault(name => name.UserName == user.UserName) == null)
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return user;
                }
            }
            return new User();
        }
        public User Delete(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            return user;
        }
        public User DisplayName(string displayName)
        {
            var user = _context.Users.Single(name => name.DisplayName == displayName);

            if (user != null)
            {
                return user;
            }
            return user;
        }
        public User Get(string username)
        {
            return _context.Users.FirstOrDefault(user => user.UserName == username);
        }
        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }
        public User GetById(Guid id)
        {
            return _context.Users.Find(id);
        }
        public User Update(User user)
        {
            user = _context.Users.Update(user).Entity;
            _context.SaveChanges();
            return user;
        }
        private bool ChackProperty(User user)
        {
            return user.UserName != "" && user.DisplayName != "" && user.Passwors != ""
                && user.UserName.Length > 4 && user.UserName.Length < 11 && user.DisplayName?.Length > 4
                && user.DisplayName.Length < 11 && user.Passwors.Length > 4 && user.Passwors.Length < 11;
        }

    }
}
