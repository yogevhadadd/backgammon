using HomeAccessControll.Data.Model;

namespace HomeAccessControll.Data.Interface
{
    public interface IHomeRepositoriy
    {
        IQueryable<User> GetAllRegistarUser();
        User AddUser(User user);
        User UpdateUser(User user);
        User DeleteUser(Guid id);
        User GetUserById(Guid id);
        IEnumerable<User> GetAllConnectedUsers();
    }
}
