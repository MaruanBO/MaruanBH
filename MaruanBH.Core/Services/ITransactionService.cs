using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Entities;

namespace MaruanBH.Core.Services
{
    public interface ITransactionService
    {
        Task<Result<List<Transaction>>> GetTransactionsForCustomer(Guid accountId);
        Task<Result<Guid>> CreateTransactionAsync(Guid accountId, decimal amount);
    }
}