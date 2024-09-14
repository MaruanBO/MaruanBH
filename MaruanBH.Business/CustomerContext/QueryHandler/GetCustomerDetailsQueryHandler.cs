using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using MaruanBH.Core.Services;
using MaruanBH.Core.CustomerContext.DTOs;
using MaruanBH.Core.CustomerContext.Queries;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Base.Error;
using MaruanBH.Domain.Entities;
using MaruanBH.Core.TransactionContext.DTOs;

namespace MaruanBH.Business.CustomerContext.QueryHandler
{
    public class GetCustomerDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, Result<CustomerDetailDto, Error>>
    {
        private readonly ICustomerService _customerService;
        private readonly ITransactionService _transactionService;

        public GetCustomerDetailsQueryHandler(ICustomerService customerService, ITransactionService transactionService)
        {
            _customerService = customerService;
            _transactionService = transactionService;
        }

        public async Task<Result<CustomerDetailDto, Error>> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
        {
            var customerResult = await _customerService.GetCustomerByIdAsync(request.Id);

            if (customerResult.IsFailure)
                return Result.Failure<CustomerDetailDto, Error>(customerResult.Error);

            var transactionsResult = await _transactionService.GetTransactionsForCustomer(request.Id);

            if (transactionsResult.IsFailure)
                return Result.Failure<CustomerDetailDto, Error>(Error.NotFound(transactionsResult.Error));

            return Result.Success<CustomerDetailDto, Error>(CreateCustomerDetailDto(customerResult.Value, transactionsResult.Value));
        }

        private static CustomerDetailDto CreateCustomerDetailDto(Customer customer, List<Transaction> transactions) =>
            new CustomerDetailDto
            {
                Name = customer.Name,
                Surname = customer.Surname,
                Balance = transactions.Sum(t => t.Amount) + customer.Balance,
                Transactions = transactions.Select(CreateTransactionDto).ToList()
            };

        private static TransactionDto CreateTransactionDto(Transaction t) =>
            new TransactionDto(t.Amount, t.Date);
    }
}
