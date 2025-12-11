namespace Kashi_SmartBudget.Models.DTOs.AI
{
    public class AIResultDto
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public object? Data { get; set; }
    }
}
