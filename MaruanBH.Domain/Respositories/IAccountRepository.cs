using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Base.Error;

namespace MaruanBH.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<Result> AddAsync(Account account);
        Task<Maybe<Account>> GetByIdAsync(Guid accountId);
    }
}
