using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyRE.Core.Models;

namespace MyRE.Core.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(Account entity);
        Task<Account> GetByRemoteIdAsync(string remoteAccountId);
    }
}
