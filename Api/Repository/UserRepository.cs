using Api.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User AddOrGetUser(string email)
        {
            User? userObj = null;
            bool userExists = _context.Users.Any(user => user.Username == email);

            if (!userExists)
            {
                _context.Users.Add(new User { Username = email, RoleID = 1 });
                _context.SaveChanges();
            }

            userObj = _context.Users.First(user => user.Username == email);
            return userObj;
        }
        public int GetUserId(string email)
        {
            var user = _context.Users.FirstOrDefault(user=> user.Username == email);
            return user != null ? user.UserID : -1; 
        }
        public ICollection<User> GetUsers()
        {
            return [.. _context.Users.Include(r => r.Role).OrderBy(user => user.Username)];
        }
    }
}