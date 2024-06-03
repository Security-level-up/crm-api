using Models;

namespace Api.Interfaces
{
    public interface IPipelineRepository
    {
        ICollection<PipelineStage> GetPipelineStages();
    }
}