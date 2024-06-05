using Models;

namespace Api.Interfaces
{
    public interface ISalesOpportunitiesRepository
    {
        ICollection<SalesOpportunity> GetSalesOpportunities();
        ICollection<SalesOpportunity> GetSalesOpportunitiesByUserId(int userId);
        SalesOpportunity? GetSalesOpportunityById(int opportunityId); 
        void SaveChanges(); 
    }
}