using Xunit;
using Moq;
using MaruanBH.Core.AccountContext.Commands;
using MaruanBH.Core.Services;
using MaruanBH.Domain.Entities;
using MaruanBH.Core.Base.Exceptions;
using Microsoft.Extensions.Logging;
using MaruanBH.Core.CustomerContext.DTOs;
using MaruanBH.Core.AccountContext.DTOs;
using MaruanBH.Business.AccountContext.CommandHandler;

public class CreateAccountCommandHandlerTests
{
    private readonly Mock<IAccountService> _accountServiceMock;
    private readonly Mock<ITransactionService> _transactionServiceMock;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<ILogger<CreateAccountCommandHandler>> _loggerMock;
    private readonly CreateAccountCommandHandler _handler;

    public CreateAccountCommandHandlerTests()
    {
        _accountServiceMock = new Mock<IAccountService>();
        _transactionServiceMock = new Mock<ITransactionService>();
        _customerServiceMock = new Mock<ICustomerService>();
        _loggerMock = new Mock<ILogger<CreateAccountCommandHandler>>();

        _handler = new CreateAccountCommandHandler(
            _accountServiceMock.Object,
            _transactionServiceMock.Object,
            _customerServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateAccount()
    {
        var customerId = Guid.NewGuid();
        var initialCredit = 100;
        var command = new CreateAccountCommand(new CreateAccountDto
        {
            CustomerId = customerId,
            InitialCredit = initialCredit
        });

        _customerServiceMock
            .Setup(s => s.GetCustomerByIdAsync(customerId))
            .ReturnsAsync(new Customer("Marouane", "Boukhriss", 0));

        var newAccountId = Guid.NewGuid();
        _accountServiceMock
            .Setup(s => s.CreateAccountAsync(It.IsAny<Account>()))
            .ReturnsAsync(newAccountId);

        var result = await _handler.Handle(command, CancellationToken.None);

        _accountServiceMock.Verify(s => s.CreateAccountAsync(It.IsAny<Account>()), Times.Once);
        _transactionServiceMock.Verify(s => s.CreateTransactionAsync(newAccountId, initialCredit), Times.Once);
        Assert.Equal(newAccountId, result);
    }

    [Fact]
    public async Task Handle_InitialCreditGreaterThanZero_ShouldCreateTransaction()
    {
        var customerId = Guid.NewGuid();
        var initialCredit = 200;
        var command = new CreateAccountCommand(new CreateAccountDto
        {
            CustomerId = customerId,
            InitialCredit = initialCredit
        });

        _customerServiceMock
            .Setup(s => s.GetCustomerByIdAsync(customerId))
            .ReturnsAsync(new Customer("Marouane", "Bo", 1000));

        var newAccountId = Guid.NewGuid();
        _accountServiceMock
            .Setup(s => s.CreateAccountAsync(It.IsAny<Account>()))
            .ReturnsAsync(newAccountId);

        await _handler.Handle(command, CancellationToken.None);

        _transactionServiceMock.Verify(s => s.CreateTransactionAsync(newAccountId, initialCredit), Times.Once);
    }
}
