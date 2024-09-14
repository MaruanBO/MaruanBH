using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Entities;

namespace MaruanBH.Core.Services
{
    public interface IAccountService
    {
        Task<Result<Guid>> CreateAccountAsync(Account account);
        Task<Result<Account>> GetByIdAsync(Guid id);
    }
}