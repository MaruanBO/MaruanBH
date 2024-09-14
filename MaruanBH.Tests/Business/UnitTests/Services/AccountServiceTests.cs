using System;
using System.Threading.Tasks;
using MaruanBH.Business.Services;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using Moq;
using Xunit;
using CSharpFunctionalExtensions;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _mockAccountRepository = new Mock<IAccountRepository>();
        _accountService = new AccountService(_mockAccountRepository.Object);
    }

    [Fact]
    public async Task CreateAccountAsync_ReturnsAccountId_WhenAccountCreated()
    {
        var account = new Account(Guid.NewGuid(), 100m);

        _mockAccountRepository.Setup(repo => repo.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync(Result.Success());

        var result = await _accountService.CreateAccountAsync(account);

        Assert.True(result.IsSuccess);
        Assert.Equal(account.Id, result.Value);
        _mockAccountRepository.Verify(repo => repo.AddAsync(account), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAccount_WhenAccountExists()
    {
        var accountId = Guid.NewGuid();
        var account = new Account
        (
            Guid.NewGuid(),
            100m
        );

        _mockAccountRepository.Setup(repo => repo.GetByIdAsync(accountId))
            .ReturnsAsync(Maybe<Account>.From(account));

        var result = await _accountService.GetByIdAsync(accountId);

        Assert.True(result.IsSuccess);
        Assert.Equal(account, result.Value);
    }
}