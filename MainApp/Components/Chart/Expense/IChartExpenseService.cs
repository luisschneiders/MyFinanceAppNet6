namespace MainApp.Components.Chart.Expense;

public interface IChartExpenseService
{
    Task<ChartConfigData> ConfigDataLast3Months();
    Task<ChartConfigData> ConfigDataLast5Years();
}
