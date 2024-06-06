using Microsoft.AspNetCore.Mvc;
using Models;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PipelineStagesController : ControllerBase
    {
        private readonly IPipelineRepository _pipelineRepository;

        public PipelineStagesController(IPipelineRepository pipelineRepository)
        {
            _pipelineRepository = pipelineRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PipelineStage>> GetPipelineStages()
        {
            return Ok(_pipelineRepository.GetPipelineStages());
        }
    }
}
