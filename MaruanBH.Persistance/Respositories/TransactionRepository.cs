using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MaruanBH.Core.Base.Exceptions;

namespace MaruanBH.Persistance.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<TransactionRepository> _logger;
        private const string TransactionCacheKey = "Transactions";

        public TransactionRepository(IMemoryCache cache, ILogger<TransactionRepository> logger)
        {
            _cache = cache;
            _logger = logger;
            InitializeCache();
        }

        public Task<List<Transaction>> GetTransactionsForCustomer(Guid accountId) =>
            Task.FromResult(
                Maybe<List<Transaction>>
                    .From(GetTransactionDictionary().TryGetValue(accountId, out var transactions) ? transactions : null)
                    .ToResult($"No transactions found for account {accountId}")
                    .Tap(t => _logger.LogInformation($"Retrieved {t.Count} transactions for account {accountId}"))
                    .GetValueOrDefault(new List<Transaction>())
            );

        public Task AddAsync(Guid accountId, Transaction transaction) =>
            Result.SuccessIf(transaction.Amount > 0, "Transaction amount must be positive")
                .Tap(() => _logger.LogInformation("Creating transaction for account {AccountId} with amount {Amount}", accountId, transaction.Amount))
                .Bind(() =>
                {
                    var dict = GetTransactionDictionary();
                    if (!dict.TryGetValue(accountId, out var accountTransactions))
                    {
                        dict[accountId] = new List<Transaction>();
                    }
                    dict[accountId].Add(transaction);
                    _cache.Set(TransactionCacheKey, dict);
                    _logger.LogInformation($"Added transaction for account {accountId}: Amount={transaction.Amount}, Date={transaction.Date}");
                    return Result.Success();
                })
                .Finally(result =>
                {
                    if (result.IsFailure)
                    {
                        _logger.LogWarning($"Failed to add transaction: {result.Error}");
                    }
                    return result.IsSuccess ? Task.CompletedTask : Task.FromException(new CustomException(result.Error));
                });

        private void InitializeCache() =>
            _cache.GetOrCreate(TransactionCacheKey, _ => new Dictionary<Guid, List<Transaction>>());

        private Dictionary<Guid, List<Transaction>> GetTransactionDictionary() =>
            _cache.Get<Dictionary<Guid, List<Transaction>>>(TransactionCacheKey)
            ?? new Dictionary<Guid, List<Transaction>>();
    }
}
