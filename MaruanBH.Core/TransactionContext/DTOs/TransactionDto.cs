using System;

namespace MaruanBH.Core.TransactionContext.DTOs
{
    public record TransactionDto
    {
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }

        public TransactionDto(decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }
    }
}