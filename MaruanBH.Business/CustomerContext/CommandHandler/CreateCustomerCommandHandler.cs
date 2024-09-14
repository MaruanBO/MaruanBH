using MediatR;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Entities;
using MaruanBH.Core.CustomerContext.Commands;
using MaruanBH.Domain.Repositories;

namespace MaruanBH.Business.CustomerContext.CommandHandler
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IAccountRepository accountRepository)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            (
                request.CustomerDto.Name,
                request.CustomerDto.Surname,
                request.CustomerDto.Balance
            );

            await _customerRepository.AddAsync(customer);

            var account = new Account
            (
                customer.Id,
                customer.Balance
            );

            await _accountRepository.AddAsync(account);

            return customer.Id;
        }
    }

}