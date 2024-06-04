using Microsoft.AspNetCore.Mvc;
using Models;
using Api.Interfaces;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOpportunitiesController : ControllerBase
    {
        private readonly ISalesOpportunitiesRepository _salesOpportunitiesRepository;

        public SalesOpportunitiesController(ISalesOpportunitiesRepository salesOpportunitiesRepository)
        {
            _salesOpportunitiesRepository = salesOpportunitiesRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SalesOpportunity>> GetSalesOpportunities()
        {
            return Ok(_salesOpportunitiesRepository.GetSalesOpportunities());
        }
    }
}
