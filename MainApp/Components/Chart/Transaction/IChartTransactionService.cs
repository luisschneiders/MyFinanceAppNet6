namespace MainApp.Components.Chart.Transaction;

public interface IChartTransactionService
{
    Task<ChartConfigData> ConfigDataIO(DateTimeRange dateTimeRange);
    Task<ChartConfigData> ConfigDataLast3Months();
}
