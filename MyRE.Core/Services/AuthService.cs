using System;
using System.Threading.Tasks;
using MyRE.Core.Models;
using MyRE.Core.Models.Data;
using MyRE.Core.Repositories;

namespace MyRE.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAppInstanceRepository _appInstanceRepository;

        public AuthService(IAccountRepository accountRepository, IAppInstanceRepository appInstanceRepository)
        {
            _accountRepository = accountRepository;
            _appInstanceRepository = appInstanceRepository;
        }
        
        public async Task<AppInstance> CreateInstanceAsync(string accountId, string instanceName, string userId, string appId, string serverBaseUri, string accessToken)
        {
            var existingAccount = await _accountRepository.GetByRemoteIdAsync(accountId);
            if (existingAccount != null && existingAccount.User.Id != userId)
            {
                throw new ArgumentException("That account has already been added for a different user.");
            } else if (existingAccount == null)
            {
                existingAccount = await _accountRepository.CreateAsync(new Account()
                {
                    RemoteAccountId = accountId,
                    UserId = userId,
                });
            }
            
            var instance = new AppInstance()
            {
                AccessToken = accessToken,
                Name = instanceName,
                RemoteAppId = appId,
                Account = existingAccount,
                InstanceServerBaseUri = serverBaseUri,
            };

            var result = await _appInstanceRepository.CreateAsync(instance);

            return result;
        }
        
    }
}
