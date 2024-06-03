public class SalesOpportunityUpdateDto
{
    public string Title { get; set; }
    public double? ProbOfCompletion { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? DateClosed { get; set; }
    public int? Stage { get; set; }
    public int? AssignedTo { get; set; }
    public string Notes { get; set; }
}