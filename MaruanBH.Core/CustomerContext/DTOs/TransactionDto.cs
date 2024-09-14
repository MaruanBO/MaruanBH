using System;

namespace MaruanBH.Core.CustomerContext.DTOs
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