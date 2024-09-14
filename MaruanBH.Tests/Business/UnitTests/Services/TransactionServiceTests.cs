using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaruanBH.Business.Services;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using Moq;
using Xunit;
using CSharpFunctionalExtensions;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _transactionService = new TransactionService(_mockTransactionRepository.Object);
    }

    [Fact]
    public async Task GetTransactionsForCustomer_ReturnsTransactions_WhenTransactionsExist()
    {
        var accountId = Guid.NewGuid();
        var transactions = new List<Transaction>
        {
            new Transaction(DateTime.Now, accountId, 100m),
            new Transaction(DateTime.Now, accountId, 200m)
        };

        _mockTransactionRepository.Setup(repo => repo.GetTransactionsForCustomer(accountId))
            .ReturnsAsync(transactions);

        var result = await _transactionService.GetTransactionsForCustomer(accountId);

        Assert.True(result.IsSuccess);
        Assert.Equal(transactions, result.Value);
    }

    [Fact]
    public async Task CreateTransactionAsync_ReturnsTransactionId_WhenTransactionCreated()
    {
        var accountId = Guid.NewGuid();
        var amount = 100m;

        _mockTransactionRepository.Setup(repo => repo.AddAsync(It.IsAny<Guid>(), It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        var result = await _transactionService.CreateTransactionAsync(accountId, amount);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        _mockTransactionRepository.Verify(repo => repo.AddAsync(accountId,
            It.Is<Transaction>(t => t.AccountId == accountId && t.Amount == amount)), Times.Once);
    }
}
