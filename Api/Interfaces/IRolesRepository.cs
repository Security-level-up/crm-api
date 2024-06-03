using Models;

namespace Api.Interfaces
{
    public interface IRolesRepository
    {
        ICollection<Role> GetRoles();
    }
}