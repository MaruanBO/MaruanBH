using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaruanBH.Domain.Entities;
using MaruanBH.Persistance.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class TransactionRepositoryTests
{
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly TransactionRepository _transactionRepository;
    private readonly Mock<ICacheEntry> _mockCacheEntry;

    public TransactionRepositoryTests()
    {
        _mockCache = new Mock<IMemoryCache>();
        _mockCacheEntry = new Mock<ICacheEntry>();

        _mockCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(_mockCacheEntry.Object);

        _transactionRepository = new TransactionRepository(_mockCache.Object, Mock.Of<ILogger<TransactionRepository>>());
    }

    [Fact]
    public async Task GetTransactionsForCustomer_ReturnsTransactions_WhenTransactionsExist()
    {
        var accountId = Guid.NewGuid();
        var transaction1 = new Transaction(DateTime.Now, Guid.NewGuid(), 100m);
        var transaction2 = new Transaction(DateTime.Now, Guid.NewGuid(), 200m);

        var transactions = new List<Transaction> { transaction1, transaction2 };

        var cacheDict = new Dictionary<Guid, List<Transaction>> { { accountId, transactions } };
        object? outValue = cacheDict;

        _mockCache
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out outValue))
            .Returns(true);

        var retrievedTransactions = await _transactionRepository.GetTransactionsForCustomer(accountId);

        Assert.NotNull(retrievedTransactions);
        Assert.Equal(transactions, retrievedTransactions);
    }

    [Fact]
    public async Task GetTransactionsForCustomer_ReturnsEmptyList_WhenNoTransactionsExist()
    {
        var accountId = Guid.NewGuid();
        object? outValue = null;

        _mockCache
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out outValue))
            .Returns(false);

        var retrievedTransactions = await _transactionRepository.GetTransactionsForCustomer(accountId);

        Assert.NotNull(retrievedTransactions);
        Assert.Empty(retrievedTransactions);
    }
}
