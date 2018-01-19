using MyRE.Core.Models.Data;
using MyRE.Core.Services;

namespace MyRE.Data.Services
{
    public class RoutineModelMappingService : IDomainModelMappingService<Routine, Core.Models.Domain.Routine>
    {
        public RoutineModelMappingService()
        {
        }
        
        public Core.Models.Domain.Routine ToDomainModel(Core.Models.Data.Routine dataModel)
        {
            return new Core.Models.Domain.Routine()
            {
                RoutineId = dataModel.RoutineId,
                Name = dataModel.Name,
                Description = dataModel.Description,
                ProjectId = dataModel.ProjectId,
                BlockId = dataModel.BlockId,
                ExecutionMethod = dataModel.ExecutionMethod.ToString(),
            };
        }
    }
}