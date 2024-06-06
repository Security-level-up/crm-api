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

        public ICollection<SalesOpportunity> GetSalesOpportunitiesByUserId(int userId)
        {
            return [.._context.SalesOpportunities
                .Include(u => u.User)
                .Include(p => p.PipelineStage)
                .Where(opportunity => opportunity.AssignedTo == userId)
                .OrderBy(opportunity => opportunity.Title)];
        }

        public SalesOpportunity? GetSalesOpportunityById(int opportunityId)
        {
            return _context.SalesOpportunities.FirstOrDefault(opportunity => opportunity.OpportunityID == opportunityId);
        }

        public void CreateSalesOpportunity(SalesOpportunity salesOpportunity)
        {
            _context.SalesOpportunities.Add(salesOpportunity);
        }

        public void DeleteSalesOpportunity(SalesOpportunity salesOpportunity)
        {
            _context.SalesOpportunities.Remove(salesOpportunity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
