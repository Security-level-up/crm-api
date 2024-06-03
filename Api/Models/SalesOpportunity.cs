using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    [Table("SalesOpportunity")]
    public class SalesOpportunity
    {
        [SwaggerSchema(ReadOnly = true)]
        [Key]
        public int OpportunityID { get; set; }
        public required string Title { get; set; }
        public float ProbOfCompletion { get; set; }
        public float Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }
        public int Stage { get; set; }
        public int AssignedTo { get; set; }
        public required string Notes { get; set; }
    }
}