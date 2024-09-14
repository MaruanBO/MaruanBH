using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Base.Error;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Domain.Repositories;
using MaruanBH.Domain.Entities;
using Microsoft.Extensions.Logging;
using MaruanBH.Core.AccountContext.DTOs;
using MaruanBH.Core.CustomerContext.DTOs;

namespace MaruanBH.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        protected ILogger<CustomerService> Logger { get; }

        public CustomerService(ICustomerRepository customerRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            Logger = logger;
        }

        public Task<Result<Customer, Error>> GetCustomerByIdAsync(Guid id) =>
            Result.SuccessIf(id != Guid.Empty, id, Error.BadRequest("Invalid customer ID"))
                .Bind(async validId =>
                    (await _customerRepository.GetCustomerByIdAsync(validId))
                        .ToResult(Error.NotFound("Customer not found")))
                .TapError(error => throw new CustomException(error));



        public async Task<Result<Guid>> CreateCustomerAsync(Customer customer) =>

            await Result.Success(customer)
                .Ensure(cust => cust != null, "Customer cannot be null")
                .Bind(async cust =>
                {
                    var addResult = await _customerRepository.AddAsync(cust);

                    return addResult
                        .Map(() => cust.Id);
                });


    }
}
