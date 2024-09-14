using System;
using System.Threading;
using System.Threading.Tasks;
using MaruanBH.Business.CustomerContext.CommandHandler;
using MaruanBH.Core.CustomerContext.Commands;
using MaruanBH.Core.CustomerContext.DTOs;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using Moq;
using Xunit;
using CSharpFunctionalExtensions;


public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockAccountRepository = new Mock<IAccountRepository>();
        _handler = new CreateCustomerCommandHandler(_mockCustomerRepository.Object, _mockAccountRepository.Object);
    }

    [Fact]
    public async Task Handle_CreatesCustomerAndAccount_ReturnsCustomerId()
    {
        var customerDto = new CreateCustomerDto
        {
            Name = "Marouane",
            Surname = "Boukhriss Ouchab",
            Balance = 100m
        };
        var command = new CreateCustomerCommand(customerDto);

        Customer? capturedCustomer = null;
        Account? capturedAccount = null;

        _mockCustomerRepository.Setup(r => r.AddAsync(It.IsAny<Customer>()))
      .Callback<Customer>(c => capturedCustomer = c)
      .ReturnsAsync(Result.Success());

        _mockAccountRepository.Setup(r => r.AddAsync(It.IsAny<Account>()))
            .Callback<Account>(a => capturedAccount = a)
            .ReturnsAsync(Result.Success());


        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        Assert.NotNull(capturedCustomer);
        Assert.Equal(result, capturedCustomer.Id);
        Assert.Equal(customerDto.Name, capturedCustomer.Name);
        Assert.Equal(customerDto.Surname, capturedCustomer.Surname);
        Assert.Equal(customerDto.Balance, capturedCustomer.Balance);
        Assert.NotNull(capturedAccount);
        Assert.NotEqual(Guid.Empty, capturedAccount.Id);
        Assert.Equal(result, capturedAccount.CustomerId);
        Assert.Equal(customerDto.Balance, capturedAccount.InitialCredit);
    }
}

