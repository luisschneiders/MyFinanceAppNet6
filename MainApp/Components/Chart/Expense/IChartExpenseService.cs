namespace MainApp.Components.Chart.Expense;

public interface IChartExpenseService
{
    Task<ChartConfigData> ConfigDataLast3Months();
    Task<ChartConfigData> ConfigDataLast5Years();
    Task<ChartConfigData> ConfigDataTop5(DateTimeRange dateTimeRange);
    Task<ChartConfigData> ConfigDataExpenseByMonth(FilterExpenseByMonthDTO filter);
}
