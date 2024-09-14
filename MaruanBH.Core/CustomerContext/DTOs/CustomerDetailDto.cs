using MaruanBH.Domain.Entities;

namespace MaruanBH.Core.CustomerContext.DTOs
{
    public class CustomerDetailDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
    }
}