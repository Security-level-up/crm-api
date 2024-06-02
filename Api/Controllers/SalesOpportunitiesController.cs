using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Data;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOpportunitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalesOpportunitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SalesOpportunities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOpportunity>>> GetSalesOpportunities()
        {
            return await _context.SalesOpportunities.ToListAsync();
        }
    }
}
