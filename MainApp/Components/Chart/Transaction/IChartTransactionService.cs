namespace MainApp.Components.Chart.Transaction;

public interface IChartTransactionService
{
    Task<ChartConfigData> ConfigDataIOByMonth(DateTimeRange dateTimeRange);
    Task<ChartConfigData> ConfigDataIOByDay(DateTimeRange dateTimeRange);
    Task<ChartConfigData> ConfigDataLast3Months();
}
