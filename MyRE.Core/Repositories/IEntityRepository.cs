using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Extensions;
using Optional;

namespace MyRE.Core.Repositories
{
    public interface IEntityRepository<TEntity, TId>
    {
        Task<IEnumerable<TId>> GetKeysAsync();
        Task<Option<TEntity>> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> EnumerateAsync();
        Task<Option<TEntity>> AddAsync(TEntity entity);
        Task DeleteAsync(TId id);
    }
}
