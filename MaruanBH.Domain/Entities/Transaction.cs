using System;

namespace MaruanBH.Domain.Entities
{
    /// <summary>
    /// Immutable entity with readonly properties
    /// </summary>
    public record Transaction
    {
        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public Guid AccountId { get; init; } // foreign key
        public decimal Amount { get; init; }

        public Transaction(DateTime date, Guid accountId, decimal amount)
        {
            Id = Guid.NewGuid();
            Date = date;
            AccountId = accountId;
            Amount = amount;
        }
    }
}