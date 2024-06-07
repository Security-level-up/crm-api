using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Models;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = UserRoles.SalesRep)]
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
        [Authorize(Roles = UserRoles.SalesRep)]
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

        [HttpPatch("{opportunityId}")]
        [Authorize(Roles = UserRoles.SalesRep)]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult UpdateSalesOpportunity(int opportunityId, [FromBody] Dictionary<string, object> fieldsToUpdate)
        {
            var salesOpportunity = _salesOpportunitiesRepository.GetSalesOpportunityById(opportunityId);
            if (salesOpportunity == null)
            {
                return NotFound($"Sales opportunity with ID {opportunityId} not found.");
            }

            foreach (var field in fieldsToUpdate)
            {
                switch (field.Key.ToLower())
                {
                    case "title":
                        salesOpportunity.Title = field.Value.ToString();
                        break;
                    case "probofcompletion":
                        if (float.TryParse(field.Value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out float probOfCompletion))
                        {
                            salesOpportunity.ProbOfCompletion = probOfCompletion;
                        }
                        else
                        {
                            return BadRequest($"Invalid value for ProbOfCompletion: {field.Value}");
                        }
                        break;
                    case "amount":
                        if (float.TryParse(field.Value.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out float amount))
                        {
                            salesOpportunity.Amount = amount;
                        }
                        else
                        {
                            return BadRequest($"Invalid value for Amount: {field.Value}");
                        }
                        break;
                    case "datecreated":
                        if (DateTime.TryParse(field.Value.ToString(), out DateTime dateCreated))
                        {
                            salesOpportunity.DateCreated = DateTime.SpecifyKind(dateCreated, DateTimeKind.Utc);
                        }
                        else
                        {
                            return BadRequest($"Invalid value for DateCreated: {field.Value}");
                        }
                        break;
                    case "dateclosed":
                        if (DateTime.TryParse(field.Value.ToString(), out DateTime dateClosed))
                        {
                            salesOpportunity.DateClosed = DateTime.SpecifyKind(dateClosed, DateTimeKind.Utc);
                        }
                        else
                        {
                            return BadRequest($"Invalid value for DateClosed: {field.Value}");
                        }
                        break;

                    case "stage":
                        if (int.TryParse(field.Value.ToString(), out int stage))
                        {
                            salesOpportunity.Stage = stage;
                        }
                        else
                        {
                            return BadRequest($"Invalid value for Stage: {field.Value}");
                        }
                        break;
                    case "assignedto":
                        if (int.TryParse(field.Value.ToString(), out int assignedTo))
                        {
                            salesOpportunity.AssignedTo = assignedTo;
                        }
                        else
                        {
                            return BadRequest($"Invalid value for AssignedTo: {field.Value}");
                        }
                        break;
                    case "notes":
                        salesOpportunity.Notes = field.Value.ToString();
                        break;
                    default:
                        return BadRequest($"Invalid field: {field.Key}");
                }
            }

            _salesOpportunitiesRepository.SaveChanges();

            return Ok(salesOpportunity);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult CreateSalesOpportunity(SalesOpportunity salesOpportunity)
        {
            if (salesOpportunity.Stage is < 1 or > 6)
            {
                return BadRequest("Invalid stage id entered. Values must range from 1 to 6 inclusive");
            }

            _salesOpportunitiesRepository.CreateSalesOpportunity(salesOpportunity);
            _salesOpportunitiesRepository.SaveChanges();

            return CreatedAtAction(nameof(GetSalesOpportunities), new { id = salesOpportunity.OpportunityID }, salesOpportunity);
        }

        [HttpPost("personalSalesOpportunity")]
        [Authorize(Roles = UserRoles.SalesRep)]
        public IActionResult CreatePersonalSalesOpportunity(SalesOpportunity salesOpportunity)
        {
            if (salesOpportunity.Stage is < 1 or > 6)
            {
                return BadRequest("Invalid stage id entered. Values must range from 1 to 6 inclusive");
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? email = claimsIdentity?.FindFirst(ClaimTypes.Email);

            if (email == null)
            {
                return BadRequest("Unable to access email of user");
            }

            int userId = _userRepository.GetUserId(email.Value);
            salesOpportunity.AssignedTo = userId;

            _salesOpportunitiesRepository.CreateSalesOpportunity(salesOpportunity);
            _salesOpportunitiesRepository.SaveChanges();

            return CreatedAtAction(nameof(GetSalesOpportunities), new { id = salesOpportunity.OpportunityID }, salesOpportunity);
        }

        [Authorize(Roles = UserRoles.Manager)]
        [HttpDelete("{opportunityId}")]
        public IActionResult DeleteSalesOpportunity(int opportunityId)
        {
            var salesOpportunity = _salesOpportunitiesRepository.GetSalesOpportunityById(opportunityId);
            if (salesOpportunity == null)
            {
                return NotFound($"Sales opportunity with ID {opportunityId} not found.");
            }

            _salesOpportunitiesRepository.DeleteSalesOpportunity(salesOpportunity);
            _salesOpportunitiesRepository.SaveChanges();

            return NoContent();
        }
    }
}
