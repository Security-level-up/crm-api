using Api.Interfaces;
using Data;
using Models;

namespace Api.Repository
{
    public class SalesOpportunitiesRepository : ISalesOpportunitiesRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesOpportunitiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<SalesOpportunity> GetSalesOpportunities()
        {
            return [.. _context.SalesOpportunities.OrderBy(opportunity => opportunity.Title)];
        }
    }
}