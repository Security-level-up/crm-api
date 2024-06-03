using Microsoft.AspNetCore.Mvc;
using Models;
using Api.Interfaces;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
