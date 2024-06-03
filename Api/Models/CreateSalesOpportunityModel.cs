namespace Models
{
    public class CreateSalesOpportunityModel
    {
        public string Title { get; set; }
        public float ProbOfCompletion { get; set; }
        public float Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public int Stage { get; set; }
        public int AssignedTo { get; set; }
        public string Notes { get; set; }
    }
}