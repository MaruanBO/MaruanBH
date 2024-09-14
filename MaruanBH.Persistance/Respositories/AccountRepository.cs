using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using MaruanBH.Core.Base.Exceptions;

namespace MaruanBH.Persistance.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<AccountRepository> _logger;
        private const string AccountCacheKey = "Accounts";

        public AccountRepository(IMemoryCache cache, ILogger<AccountRepository> logger)
        {
            _cache = cache;
            _logger = logger;
            InitializeCache();
        }

        public Task<Result> AddAsync(Account account) =>
            Result.Success()
                .Tap(() => _logger.LogInformation("Creating account with ID {AccountId}", account.Id))
                .Tap(() =>
                {
                    var accounts = GetAccountDictionary();
                    accounts[account.Id] = account;
                    _cache.Set(AccountCacheKey, accounts);
                    _logger.LogInformation("Added account with ID {AccountId}", account.Id);
                })
                .Finally(result =>
                {
                    if (result.IsFailure)
                    {
                        _logger.LogWarning("Failed to add account: {Error}", result.Error);
                    }
                    return Task.FromResult(result);
                });


        public Task<Maybe<Account>> GetByIdAsync(Guid accountId) =>
            Task.FromResult(
                Maybe<Account>
                    .From(GetAccountDictionary().TryGetValue(accountId, out var account) ? account : null)
                    .Match(
                        account =>
                        {
                            _logger.LogInformation($"Retrieved account with ID {accountId}");
                            return Maybe<Account>.From(account);
                        },
                        () =>
                        {
                            _logger.LogInformation($"No account found with ID {accountId}");
                            return Maybe<Account>.None;
                        }
                    )
            );

        private void InitializeCache() =>
            _cache.GetOrCreate(AccountCacheKey, _ => new Dictionary<Guid, Account>());

        private Dictionary<Guid, Account> GetAccountDictionary() =>
            _cache.Get<Dictionary<Guid, Account>>(AccountCacheKey)
            ?? new Dictionary<Guid, Account>();
    }
}
