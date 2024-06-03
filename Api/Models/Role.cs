using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    public class Role
    {
        [SwaggerSchema(ReadOnly = true)]
        public int RoleID { get; set; }
        public required string RoleName { get; set; }
        public ICollection<User> Users { get; set; } = null!;
    }
}