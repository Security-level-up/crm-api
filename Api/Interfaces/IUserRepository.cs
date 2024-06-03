using Models;

namespace Api.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User AddOrGetUser(string email);
    }
}