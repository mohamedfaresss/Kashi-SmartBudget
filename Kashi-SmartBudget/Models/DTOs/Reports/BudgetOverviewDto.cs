namespace Kashi_SmartBudget.Models.DTOs.Reports
{
    public class BudgetOverviewDto
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public decimal BudgetAmount { get; set; }
        public string Period { get; set; } = "Monthly";

        public decimal Spent { get; set; }
        public decimal Remaining => BudgetAmount - Spent;

        // نسبة الصرف من الميزانية
        public decimal SpentPercentage =>
            BudgetAmount <= 0 ? 0 : Math.Round((Spent / BudgetAmount) * 100, 2);

        // عدّيت الميزانية؟
        public bool IsExceeded => Spent > BudgetAmount;

        // قربت تخلص (مثلاً ≥ 80%)
        public bool IsNearLimit =>
            !IsExceeded && SpentPercentage >= 80;
    }
}
