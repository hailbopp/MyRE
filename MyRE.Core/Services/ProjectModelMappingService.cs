using System.Collections.Generic;
using MyRE.Core.Models.Domain;
using MyRE.Core.Models.Language;
using MyRE.Core.Repositories;
using Newtonsoft.Json;

namespace MyRE.Core.Services
{
    public class ProjectModelMappingService: IProjectMappingService, IProjectSourceMappingService
    {
        private readonly IProjectRepository _project;

        public ProjectModelMappingService(IProjectRepository project)
        {
            _project = project;
        }

        public Project ToDomain(Core.Models.Data.Project dataModel)
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
                Source = ToDomain(dataModel.Source),
            };
        }

        public Models.Data.Project ToData(Project domain)
        {
            return new Models.Data.Project()
            {
                ProjectId = domain.ProjectId,
                Description = domain.Description,
                Name = domain.Name,
                ParentInstanceId = domain.InstanceId,
                Source = ToData(domain.Source),
            };
        }

        public ProjectSource ToDomain(Models.Data.ProjectSource dataModel)
        {
            return new ProjectSource()
            {
                ProjectSourceId = dataModel.ProjectSourceId,
                ProjectId = dataModel.ProjectId,
                CreatedAt = dataModel.CreatedAt,
                Source = dataModel.Source,
                ExpressionTree = dataModel.ParsedExpressionTree,
            };
        }

        public Models.Data.ProjectSource ToData(ProjectSource domain)
        {
            return new Models.Data.ProjectSource()
            {
                ProjectSourceId = domain.ProjectSourceId,
                CreatedAt = domain.CreatedAt,
                ParsedExpressionTree = domain.ExpressionTree,
                Source = domain.Source,
                ProjectId = domain.ProjectId,
            };
        }
    }
}
