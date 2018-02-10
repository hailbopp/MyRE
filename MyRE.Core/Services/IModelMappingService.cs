namespace MyRE.Core.Services
{
    public interface IDomainModelMappingService<TData, TDomain>
    {
        TDomain ToDomain(TData dataModel);
        TData ToData(TDomain domain);
    }

    public interface IProjectMappingService : IDomainModelMappingService<Core.Models.Data.Project, Core.Models.Domain.Project> { }
    public interface IProjectSourceMappingService : IDomainModelMappingService<Models.Data.ProjectSource, Models.Domain.ProjectSource> { }
}