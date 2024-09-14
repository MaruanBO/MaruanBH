using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MaruanBH.Business.CustomerContext.QueryHandler;
using MaruanBH.Core.CustomerContext.DTOs;
using MaruanBH.Core.CustomerContext.Queries;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Entities;
using Moq;
using Xunit;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Base.Error;

public class GetCustomerDetailsQueryHandlerTests
{
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly Mock<ITransactionService> _mockTransactionService;
    private readonly GetCustomerDetailsQueryHandler _handler;

    public GetCustomerDetailsQueryHandlerTests()
    {
        _mockCustomerService = new Mock<ICustomerService>();
        _mockTransactionService = new Mock<ITransactionService>();
        _handler = new GetCustomerDetailsQueryHandler(
            _mockCustomerService.Object,
            _mockTransactionService.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsCustomerDetails_WhenCustomerExists()
    {
        var customerId = Guid.NewGuid();
        var query = new GetCustomerDetailsQuery(customerId);

        var customer = new Customer
        (
            "Marouane",
            "Boukhriss Ouchab",
            100m
        );

        var transactions = new List<Transaction>
            {
                new Transaction (DateTime.Now.AddDays(-1), Guid.NewGuid(), 50m),
                new Transaction (DateTime.Now, Guid.NewGuid(), 20m)
            };

        _mockCustomerService.Setup(s => s.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);
        _mockTransactionService.Setup(s => s.GetTransactionsForCustomer(customerId)).ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var customerDetails = result.Value;
        Assert.NotNull(customerDetails);
        Assert.Equal(customer.Name, customerDetails.Name);
        Assert.Equal(customer.Surname, customerDetails.Surname);
        Assert.Equal(170m, customerDetails.Balance);
        Assert.Equal(2, customerDetails.Transactions.Count);
        Assert.Equal(50m, customerDetails.Transactions[0].Amount);
        Assert.Equal(20m, customerDetails.Transactions[1].Amount);
    }
}

