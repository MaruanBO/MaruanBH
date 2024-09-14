using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaruanBH.Business.Services;
using MaruanBH.Core.AccountContext.DTOs;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using CSharpFunctionalExtensions;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<ILogger<CustomerService>> _mockLogger;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockAccountRepository = new Mock<IAccountRepository>();
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _mockLogger = new Mock<ILogger<CustomerService>>();
        _customerService = new CustomerService(
            _mockCustomerRepository.Object,
            _mockAccountRepository.Object,
            _mockTransactionRepository.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsCustomer_WhenCustomerExists()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customer("Marouane", "Boukhriss Ouchab", 0);
        _mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId))
            .ReturnsAsync(customer);

        var result = await _customerService.GetCustomerByIdAsync(customerId);

        Assert.Equal(customer, result);
    }

    [Fact]
    public async Task CreateCustomerAsync_CreatesCustomerAndAccount_ReturnsCustomerId()
    {
        var customer = new Customer("Marouane", "Boukhriss Ouchab", 100m);

        _mockCustomerRepository.Setup(repo => repo.AddAsync(customer))
            .ReturnsAsync(Result.Success(true));

        var result = await _customerService.CreateCustomerAsync(customer);

        Assert.True(result.IsSuccess);
        Assert.Equal(customer.Id, result.Value);
        _mockCustomerRepository.Verify(repo => repo.AddAsync(customer), Times.Once);
    }

}

