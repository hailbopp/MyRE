using MyRE.Core.Models.Domain;
using MyRE.Core.Repositories;

namespace MyRE.Core.Services
{
    public class ProjectModelMappingService: IProjectMappingService
    {
        private readonly IProjectRepository _project;

        public ProjectModelMappingService(IProjectRepository project)
        {
            _project = project;
        }

        public Project ToDomainModel(Core.Models.Data.Project dataModel)
        {
            if (dataModel.Source == null)
            {
                dataModel.Source = _project.GetSourceById(dataModel.ProjectId).Result ?? _project.SetProjectSource(dataModel.ProjectId, "", "[]").Result;
            }

            return new Project()
            {
                ProjectId = dataModel.ProjectId,
                Name = dataModel.Name,
                Description = dataModel.Description,
                InstanceId = dataModel.ParentInstanceId,
                Source = dataModel.Source.Source,
                ExpressionTree = dataModel.Source.ExpressionTree,
            };
        }
    }
}
