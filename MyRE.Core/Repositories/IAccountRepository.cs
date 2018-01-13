using System.Threading.Tasks;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(Account entity);
        Task<Account> GetByRemoteIdAsync(string remoteAccountId);
    }
}
