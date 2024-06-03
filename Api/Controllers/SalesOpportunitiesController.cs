using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Data;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Controllers
{
    [Authorize]
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
            return await _context.SalesOpportunities
                .Include(so => so.AssignedUser)
                .Include(so => so.PipelineStage)
                .ToListAsync();
        }

        // GET: api/salesopportunities/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSalesOpportunitiesForUser(int userId)
        {
            var salesOpportunities = await _context.SalesOpportunities
                .Where(so => so.AssignedTo == userId)
                .Include(so => so.PipelineStage)
                .Select(so => new
                {
                    so.OpportunityID,
                    so.Title,
                    so.ProbOfCompletion,
                    so.Amount,
                    so.DateCreated,
                    so.DateClosed,
                    so.Stage,
                    so.AssignedTo,
                    so.Notes,
                    StageID = so.PipelineStage.StageID,
                    StageName = so.PipelineStage.StageName
                })
                .ToListAsync();

            if (salesOpportunities == null || salesOpportunities.Count == 0)
            {
                return NotFound();
            }

            return salesOpportunities;
        }

        // GET: api/SalesOpportunities/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetSalesOpportunityById(int id)
        {
            var salesOpportunity = await _context.SalesOpportunities
                .Where(so => so.OpportunityID == id)
                .Select(so => new
                {
                    so.OpportunityID,
                    so.Title,
                    PipelineStage = new
                    {
                        so.PipelineStage.StageID,
                        so.PipelineStage.StageName
                    }
                })
                .FirstOrDefaultAsync();

            if (salesOpportunity == null)
            {
                return NotFound();
            }

            return salesOpportunity;
        }

        // POST: api/SalesOpportunities
        [HttpPost]
        public async Task<ActionResult<SalesOpportunity>> CreateSalesOpportunity(CreateSalesOpportunityModel model)
        {
            if (model == null)
            {
                return BadRequest("Sales opportunity data is null");
            }

            try            {
                var newOpportunity = new SalesOpportunity
                {
                    Title = model.Title,
                    ProbOfCompletion = model.ProbOfCompletion,
                    Amount = model.Amount,
                    DateCreated = model.DateCreated,
                    Stage = model.Stage,
                    AssignedTo = model.AssignedTo,
                    Notes = model.Notes
                };

                _context.SalesOpportunities.Add(newOpportunity);
                await _context.SaveChangesAsync();

                var createdOpportunity = await _context.SalesOpportunities
                    .Include(so => so.AssignedUser)
                    .Include(so => so.PipelineStage)
                    .FirstOrDefaultAsync(so => so.OpportunityID == newOpportunity.OpportunityID);

                return CreatedAtAction(nameof(GetSalesOpportunityById), new { id = createdOpportunity.OpportunityID }, createdOpportunity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // PATCH: api/SalesOpportunities/{id}
        [HttpPatch("{id}")]

        public async Task<IActionResult> UpdateSalesOpportunity(int id, [FromBody] JsonPatchDocument<UpdateSalesOpportunityModel> patchDocument)
        {
            var salesOpportunity = await _context.SalesOpportunities.FindAsync(id);
            if (salesOpportunity == null)
            {
                return NotFound();
            }

            var modelToUpdate = new UpdateSalesOpportunityModel
            {
                OpportunityID = salesOpportunity.OpportunityID,
                Title = salesOpportunity.Title,
                ProbOfCompletion = salesOpportunity.ProbOfCompletion,
                Amount = salesOpportunity.Amount,
                DateCreated = salesOpportunity.DateCreated,
                DateClosed = salesOpportunity.DateClosed,
                Stage = salesOpportunity.Stage,
                AssignedTo = salesOpportunity.AssignedTo,
                Notes = salesOpportunity.Notes
            };

            patchDocument.ApplyTo(modelToUpdate, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                salesOpportunity.Title = modelToUpdate.Title;
                salesOpportunity.ProbOfCompletion = modelToUpdate.ProbOfCompletion;
                salesOpportunity.Amount = modelToUpdate.Amount;
                salesOpportunity.DateCreated = modelToUpdate.DateCreated.UtcDateTime;  
                salesOpportunity.DateClosed = modelToUpdate.DateClosed?.UtcDateTime;  
                salesOpportunity.Stage = modelToUpdate.Stage;
                salesOpportunity.AssignedTo = modelToUpdate.AssignedTo;
                salesOpportunity.Notes = modelToUpdate.Notes;

                _context.Entry(salesOpportunity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

}

           
