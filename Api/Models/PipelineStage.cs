namespace Models{
public class PipelineStage
    {
        public int StageID { get; set; }
        public string StageName { get; set; }
        public ICollection<SalesOpportunity> SalesOpportunities { get; set; }
    }
}