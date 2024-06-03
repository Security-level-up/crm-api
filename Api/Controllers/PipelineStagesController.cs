using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Data;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class PipelineStagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PipelineStagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PipelineStages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PipelineStage>>> GetPipelineStages()
        {
            return await _context.PipelineStages.ToListAsync();
        }
    }
}
