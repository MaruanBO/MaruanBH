using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaruanBH.Domain.Entities;
using MaruanBH.Persistance.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using MaruanBH.Domain.Repositories;
using Microsoft.Extensions.Logging;

public class AccountRepositoryTests
{
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly AccountRepository _accountRepository;
    private readonly Mock<ILogger<AccountRepository>> _loggerMock;

    private readonly Mock<ICacheEntry> _mockCacheEntry;

    public AccountRepositoryTests()
    {
        _mockCache = new Mock<IMemoryCache>();
        _mockCacheEntry = new Mock<ICacheEntry>();
        _loggerMock = new Mock<ILogger<AccountRepository>>();

        _mockCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(_mockCacheEntry.Object);

        _accountRepository = new AccountRepository(_mockCache.Object, _loggerMock.Object);
    }


    [Fact]
    public async Task AddAsync_AddsAccountToCache()
    {
        var account = new Account(Guid.NewGuid(), 100m);
        var accountsCache = new Dictionary<Guid, Account>();

        object? outValue = accountsCache;
        _mockCache
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out outValue))
            .Returns(true);

        _mockCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(_mockCacheEntry.Object);

        await _accountRepository.AddAsync(account);

        _mockCache.Verify(m => m.CreateEntry(It.IsAny<object>()), Times.Exactly(2));
        _mockCacheEntry.VerifySet(e => e.Value = It.Is<Dictionary<Guid, Account>>(d => d.ContainsKey(account.Id)));
        Assert.Contains(account.Id, accountsCache.Keys);
        Assert.Equal(account, accountsCache[account.Id]);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAccount_WhenAccountExistsInCache()
    {
        var accountId = Guid.NewGuid();
        var account = new Account(Guid.NewGuid(), 100m);
        var accountsCache = new Dictionary<Guid, Account> { { accountId, account } };

        object? outValue = accountsCache;
        _mockCache
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out outValue))
            .Returns(true);

        var retrievedAccount = await _accountRepository.GetByIdAsync(accountId);

        Assert.True(retrievedAccount.HasValue);
        Assert.Equal(account, retrievedAccount.Value);
    }

}
