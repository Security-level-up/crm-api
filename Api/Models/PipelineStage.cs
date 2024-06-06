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
        public required string StageName { get; set; }
    }
}