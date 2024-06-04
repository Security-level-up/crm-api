using Api.Interfaces;
using Data;
using Models;

namespace Api.Repository
{
    public class RolesRepository : IRolesRepository
    {
        private readonly ApplicationDbContext _context;

        public RolesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<Role> GetRoles()
        {
            return [.. _context.Roles.OrderBy(role => role.RoleName)];
        }
    }
}