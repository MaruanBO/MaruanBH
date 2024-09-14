using System;
using System.Collections.Generic;
using MaruanBH.Core.CustomerContext.DTOs;

namespace MaruanBH.Core.AccountContext.DTOs
{
    public class AccountDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public decimal InitialCredit { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
    }
}