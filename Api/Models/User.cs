using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    [Table("User")]
    public class User
    {
        [SwaggerSchema(ReadOnly = true)]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }        
        [Required(ErrorMessage = "RoleID is required")]
        public required int RoleID { get; set; }
        public Role Role { get; set; } = null!;
    }
}