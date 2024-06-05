using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    [Table("Role")]
    public class Role
    {
        [SwaggerSchema(ReadOnly = true)]
        [Key]
        public int RoleID { get; set; }
        [Required(ErrorMessage = "RoleName is required")]
        public required string RoleName { get; set; }
    }
}