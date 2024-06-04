using Api.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
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
            return [.. _context.SalesOpportunities
                .Include(u => u.User)
                .Include(p => p.PipelineStage)
                .OrderBy(opportunity => opportunity.Title)];
        }
    }
}