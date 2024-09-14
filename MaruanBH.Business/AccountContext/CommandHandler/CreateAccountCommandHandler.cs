using MediatR;
using MaruanBH.Core.AccountContext.Commands;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Domain.Base.Error;
using Microsoft.Extensions.Logging;
using MaruanBH.Core.CustomerContext.DTOs;
using MaruanBH.Core.AccountContext.DTOs;
using FluentValidation.Results;

namespace MaruanBH.Business.AccountContext.CommandHandler
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ICustomerService _customerService;
        protected ILogger<CreateAccountCommandHandler> Logger { get; }

        public CreateAccountCommandHandler(IAccountService accountService, ITransactionService transactionService, ICustomerService customerService, ILogger<CreateAccountCommandHandler> logger)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _customerService = customerService;
            Logger = logger;
        }

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {

            var customer = await _customerService.GetCustomerByIdAsync(request.AccountDto.CustomerId);

            var initialCredit = request.AccountDto.InitialCredit;

            var account = new Account
            (
                request.AccountDto.CustomerId,
                initialCredit
            );

            var accountResult = await _accountService.CreateAccountAsync(account);

            if (accountResult.IsFailure)
            {
                throw new CustomException(accountResult.Error);
            }

            var accountId = accountResult.Value;

            if (initialCredit > 0)
            {
                Logger.LogWarning("Initial credit is higher than 0, creating transaction");
                await _transactionService.CreateTransactionAsync(accountId, initialCredit);
            }

            return accountId;
        }
    }
}
