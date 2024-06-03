using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    public class PipelineStage
    {
        [SwaggerSchema(ReadOnly = true)]
        public int StageID { get; set; }
        public required string StageName { get; set; }
        public ICollection<SalesOpportunity> SalesOpportunities { get; set; } = null!;
    }
}