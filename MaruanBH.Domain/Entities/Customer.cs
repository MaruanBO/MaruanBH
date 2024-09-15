using System;
using System.Collections.Generic;

namespace MaruanBH.Domain.Entities
{
    /// <summary>
    /// Immutable entity with readonly properties
    /// </summary>
    public class Customer
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public decimal Balance { get; init; }
        public IReadOnlyList<Transaction> Transactions { get; init; }

        public Customer(string name, string surname, decimal balance)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Balance = balance;
            Transactions = new List<Transaction>();
        }
    }
}
