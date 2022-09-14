using HomeAccessControll.Data.DB;
using HomeAccessControll.Data.Interface;
using HomeAccessControll.Data.Model;

namespace HomeAccessControll.Data.Service
{
    public class HomeRepositoriy : IHomeRepositoriy
    {
        private readonly HomeDbContext _context;
        public HomeRepositoriy(HomeDbContext _context)
        {
            this._context = _context;
        }
        public User AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User DeleteUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Remove(user);
            }
            return user;
        }

        public IEnumerable<User> GetAllConnectedUsers()
        {
            return _context.Users;
        }

        public IQueryable<User> GetAllRegistarUser()
        {
            return _context.Users;
        }

        public User GetUserById(Guid id)
        {
            return _context.Users.Find(id);
        }

        public User UpdateUser(User user)
        {
            user = _context.Users.Update(user).Entity;
            _context.SaveChanges();
            return user;
        }
    }
}
