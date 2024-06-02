namespace Models{
      public class SalesOpportunity
    {
        public int OpportunityID { get; set; }
        public string Title { get; set; }
        public float ProbOfCompletion { get; set; }
        public float Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }
        public int Stage { get; set; }
        public int AssignedTo { get; set; }
        public string Notes { get; set; }
        public User AssignedUser { get; set; }
        public PipelineStage PipelineStage { get; set; }
    }
}