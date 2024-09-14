using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Repositories;
using MaruanBH.Domain.Entities;

namespace MaruanBH.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public Task<Result<List<Transaction>>> GetTransactionsForCustomer(Guid accountId) =>
               _transactionRepository.GetTransactionsForCustomer(accountId)
                   .ContinueWith(task => Result.Success(task.Result));

        public Task<Result<Guid>> CreateTransactionAsync(Guid accountId, decimal amount) =>
            Result.Success(new Transaction
            (
                DateTime.UtcNow,
                accountId,
                amount
            ))
            .Ensure(transaction => transaction.Amount > 0, "Transaction amount must be positive")
            .Bind(transaction =>
                _transactionRepository.AddAsync(accountId, transaction)
                    .ContinueWith(task =>
                        task.IsCompletedSuccessfully
                            ? Result.Success(transaction.Id)
                            : Result.Failure<Guid>($"Failed to add transaction for account {accountId}")
                    )
            );
    }
}
