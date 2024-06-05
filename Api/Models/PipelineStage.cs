using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    [Table("PipelineStage")]
    public class PipelineStage
    {
        [SwaggerSchema(ReadOnly = true)]
        [Key]
        public int StageID { get; set; }
        [Required(ErrorMessage = "StageName is required")]
        public required string StageName { get; set; }
    }
}