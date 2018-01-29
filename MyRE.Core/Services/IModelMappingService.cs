namespace MyRE.Core.Services
{
    public interface IDomainModelMappingService<TData, TDomain>
    {
        TDomain ToDomainModel(TData dataModel);
    }

    public interface IProjectMappingService : IDomainModelMappingService<Core.Models.Data.Project, Core.Models.Domain.Project> { }
}