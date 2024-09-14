using System;
using System.Threading.Tasks;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Base.Error;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Domain.Repositories;
using MaruanBH.Domain.Entities;
using CSharpFunctionalExtensions;

namespace MaruanBH.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<Result<Guid>> CreateAccountAsync(Account account) =>
             Result.Success(account)
                 .Ensure(acc => acc != null, "Account cannot be null")
                 .Tap(async acc => await _accountRepository.AddAsync(acc))
                 .Map(acc => acc.Id);


        public Task<Result<Account>> GetByIdAsync(Guid id) =>
            Result.Success(id)
                .Ensure(accountId => accountId != Guid.Empty, "Invalid account ID")
                .Bind(async accountId => await _accountRepository.GetByIdAsync(accountId)
                    .ToResult("Account not found"));

    }
}
