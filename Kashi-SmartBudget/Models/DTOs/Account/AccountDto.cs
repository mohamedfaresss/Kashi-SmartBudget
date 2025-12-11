namespace Kashi_SmartBudget.Models.DTOs.Account
{
    public class AccountDto
    {
        public AccountDto(Guid id, string name, decimal balance, string currency, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Balance = balance;
            Currency = currency;
            CreatedAt = createdAt;
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
