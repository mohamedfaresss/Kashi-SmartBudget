namespace Kashi_SmartBudget.Models.DTOs.AI
{
    public class ForecastResultDto
    {
        public decimal SpentSoFar { get; set; }
        public decimal DailyAverage { get; set; }
        public int DaysPassed { get; set; }
        public int DaysRemaining { get; set; }

        public decimal ExpectedRemainingExpense { get; set; }
        public decimal ExpectedTotalExpense { get; set; }

    }
}
