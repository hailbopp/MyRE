using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRE.Core.Models;
using MyRE.Core.Repositories;

namespace MyRE.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyREContext _myreContext;
        public AccountRepository(MyREContext myreContext)
        {
            _myreContext = myreContext;
        }

        public async Task<Account> CreateAsync(Account entity)
        {
            var result = await _myreContext.Accounts.AddAsync(entity);
            await _myreContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Account> GetByRemoteIdAsync(string remoteAccountId)
        {
            return await _myreContext.Accounts.FirstOrDefaultAsync(a => a.RemoteAccountId == remoteAccountId);
        }
    }
}
