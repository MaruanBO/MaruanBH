using System;
using System.Collections.Generic;

namespace MaruanBH.Domain.Entities
{
    /// <summary>
    /// Immutable entity with readonly properties
    /// </summary>
    public class Customer
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
        public string Surname { get; init; } = string.Empty;
        public decimal Balance { get; init; }
        public IReadOnlyList<Transaction> Transactions { get; init; } = new List<Transaction>();

        public Customer(string name, string surname, decimal balance)
        {
            Name = name;
            Surname = surname;
            Balance = balance;
        }
    }
}
