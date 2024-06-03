public class UpdateSalesOpportunityModel
{
    public int OpportunityID { get; set; }
    public string Title { get; set; }
    public float ProbOfCompletion { get; set; }
    public float Amount { get; set; }
    public DateTimeOffset DateCreated { get; set; }  
    public DateTimeOffset? DateClosed { get; set; } 
    public int Stage { get; set; }
    public int AssignedTo { get; set; }
    public string Notes { get; set; }
}
