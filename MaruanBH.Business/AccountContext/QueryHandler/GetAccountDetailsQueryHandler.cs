using MaruanBH.Core.AccountContext.DTOs;
using MaruanBH.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MaruanBH.Business.Services;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Domain.Base.Error;
using MaruanBH.Core.Services;
using MaruanBH.Core.CustomerContext.DTOs;
using MaruanBH.Core.AccountContext.Queries;

namespace MaruanBH.Business.AccountContext.QueryHandler
{
    public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQuery, AccountDetailsDto>
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly ITransactionService _transactionService;

        public GetAccountDetailsQueryHandler(
            IAccountService accountService,
            ICustomerService customerService,
            ITransactionService transactionService)
        {
            _accountService = accountService;
            _customerService = customerService;
            _transactionService = transactionService;
        }

        public async Task<AccountDetailsDto> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
        {
            var accountResult = await _accountService.GetByIdAsync(request.AccountId);

            if (accountResult.IsFailure)
            {
                throw new CustomException(accountResult.Error);
            }

            var account = accountResult.Value;

            var customerResult = await _customerService.GetCustomerByIdAsync(account.CustomerId);

            if (customerResult.IsFailure)
            {
                throw new CustomException(customerResult.Error);
            }

            var customer = customerResult.Value;

            var transactionResult = await _transactionService.GetTransactionsForCustomer(request.AccountId);

            if (transactionResult.IsFailure)
            {
                throw new CustomException(transactionResult.Error);
            }

            var transactions = transactionResult.Value;

            var balance = transactions.Sum(t => t.Amount) + account.InitialCredit + customer.Balance;

            var transactionDtos = transactions.Select(t => new TransactionDto(
                t.Amount,
                t.Date
            )).ToList();

            return new AccountDetailsDto
            {
                Id = account.Id,
                Name = customer.Name,
                Surname = customer.Surname,
                Balance = balance,
                Transactions = transactionDtos,
                CustomerId = account.CustomerId,
                InitialCredit = account.InitialCredit
            };
        }

    }
}
