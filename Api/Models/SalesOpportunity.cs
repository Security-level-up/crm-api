using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Models
{
    [Table("SalesOpportunity")]
    public class SalesOpportunity
    {
        [SwaggerSchema(ReadOnly = true)]
        [Key]
        public int OpportunityID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Range(0, 100, ErrorMessage = "Probability of completion must be between 0 and 100")]
        public float ProbOfCompletion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "Date created is required")]
        public DateTime DateCreated { get; set; }

        public DateTime? DateClosed { get; set; }

        [Required(ErrorMessage = "Stage is required")]
        public int Stage { get; set; }

        [Required(ErrorMessage = "Assigned to is required")]
        public int AssignedTo { get; set; }

        [ForeignKey("AssignedTo")]
        public User? User { get; set; } = null!;

        [ForeignKey("Stage")]
        public PipelineStage? PipelineStage { get; set; } = null!;

        [Required(ErrorMessage = "Notes are required")]
        public string Notes { get; set; }
    }
}
