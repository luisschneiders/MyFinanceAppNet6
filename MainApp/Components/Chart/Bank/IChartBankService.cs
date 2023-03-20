namespace MainApp.Components.Chart.Bank;

public interface IChartBankService
{
    Task<ChartConfigData> ConfigDataAccountActive();
    Task<ChartConfigData> ConfigDataBalanceSum();
}
