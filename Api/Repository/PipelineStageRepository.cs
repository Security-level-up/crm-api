using Api.Interfaces;
using Data;
using Models;

namespace Api.Repository
{
    public class PipelineStageRepository : IPipelineRepository
    {
        private readonly ApplicationDbContext _context;

        public PipelineStageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<PipelineStage> GetPipelineStages()
        {
            return [.. _context.PipelineStages.OrderBy(stage => stage.StageName)];
        }
    }
}