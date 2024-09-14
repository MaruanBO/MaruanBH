using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MaruanBH.Business.AccountContext.QueryHandler;
using MaruanBH.Core.AccountContext.DTOs;
using MaruanBH.Core.AccountContext.Queries;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Entities;
using Moq;
using Xunit;

public class GetAccountDetailsQueryHandlerTests
{
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly Mock<ITransactionService> _mockTransactionService;
    private readonly GetAccountDetailsQueryHandler _handler;

    public GetAccountDetailsQueryHandlerTests()
    {
        _mockAccountService = new Mock<IAccountService>();
        _mockCustomerService = new Mock<ICustomerService>();
        _mockTransactionService = new Mock<ITransactionService>();
        _handler = new GetAccountDetailsQueryHandler(
            _mockAccountService.Object,
            _mockCustomerService.Object,
            _mockTransactionService.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsAccountDetails_WhenAccountAndCustomerExist()
    {
        var accountId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var query = new GetAccountDetailsQuery(accountId);

        var account = new Account(customerId, 100m);
        var customer = new Customer("Marouane", "Boukhriss Ouchab", 50m);
        var transactions = new List<Transaction>
            {
                new Transaction (DateTime.Now.AddDays(-1), Guid.NewGuid(), 25m),
            };

        _mockAccountService.Setup(s => s.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mockCustomerService.Setup(s => s.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);
        _mockTransactionService.Setup(s => s.GetTransactionsForCustomer(accountId)).ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(account.Id, result.Id);
        Assert.Equal(customer.Name, result.Name);
        Assert.Equal(customer.Surname, result.Surname);
        Assert.Equal(175m, result.Balance);
        Assert.Single(result.Transactions);
        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal(account.InitialCredit, result.InitialCredit);
    }
}

