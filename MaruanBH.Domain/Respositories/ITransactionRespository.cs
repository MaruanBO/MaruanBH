using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaruanBH.Domain.Entities;

namespace MaruanBH.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactionsForCustomer(Guid id);
        Task AddAsync(Guid accountId, Transaction transaction);
    }
}