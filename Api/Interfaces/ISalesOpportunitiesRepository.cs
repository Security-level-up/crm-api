using Models;

namespace Api.Interfaces
{
    public interface ISalesOpportunitiesRepository
    {
        ICollection<SalesOpportunity> GetSalesOpportunities();
    }
}