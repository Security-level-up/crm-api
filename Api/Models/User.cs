using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    public class User
    {
        [SwaggerSchema(ReadOnly = true)]
        public int UserID { get; set; }
        public required string Username { get; set; }
        public required int RoleID { get; set; }
        public Role Role { get; set; } = null!;
    }
}