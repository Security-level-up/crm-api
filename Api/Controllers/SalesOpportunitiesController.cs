
using System.Security.Claims;
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
        private readonly IUserRepository _userRepository;

        public SalesOpportunitiesController(ISalesOpportunitiesRepository salesOpportunitiesRepository, IUserRepository userRepository)
        {
            _salesOpportunitiesRepository = salesOpportunitiesRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SalesOpportunity>> GetSalesOpportunities()
        {
            return Ok(_salesOpportunitiesRepository.GetSalesOpportunities());
        }

        [HttpGet("byUser/{userId}")]
        public ActionResult<IEnumerable<SalesOpportunity>> GetSalesOpportunitiesByUser(int userId)
        {
            var salesOpportunities = _salesOpportunitiesRepository.GetSalesOpportunitiesByUserId(userId);

            if (salesOpportunities == null || salesOpportunities.Count == 0)
            {
                return NotFound($"No sales opportunities found for user with ID {userId}.");
            }

            return Ok(salesOpportunities);
        }

        [HttpGet("byCurrentUser")]
        public ActionResult<IEnumerable<SalesOpportunity>> GetSalesOpportunitiesByCurrentUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? email = claimsIdentity?.FindFirst(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest("Email claim not found.");
            }

            int userId = _userRepository.GetUserId(email.Value);
            if (userId == -1)
            {
                return NotFound($"User with email {email} not found.");
            }

            var salesOpportunities = _salesOpportunitiesRepository.GetSalesOpportunitiesByUserId(userId);
            if (salesOpportunities == null || salesOpportunities.Count == 0)
            {
                return NotFound($"No sales opportunities found for user with ID {userId}.");
            }

            return Ok(salesOpportunities);
        }
    }
}
