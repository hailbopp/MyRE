namespace MyRE.Core.Services
{
    public interface IDomainModelMappingService<TData, TDomain>
    {
        TDomain ToDomainModel(TData dataModel);
    }
}