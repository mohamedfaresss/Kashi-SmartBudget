namespace Kashi_SmartBudget.Models
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; } = default!;
        public string? Icon { get; set; }
    }
}
