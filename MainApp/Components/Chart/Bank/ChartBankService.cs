using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Bank;

public class ChartBankService : IChartBankService
{
   
    private IBankService<BankModel> _bankService { get; set; } = default!;

    public ChartBankService(IBankService<BankModel> bankService)
    {
        _bankService = bankService;
    }

    public async Task<ChartConfigData> ConfigDataAccountActive()
    {
        try
        {
            List<BankModel> banks = await _bankService.GetRecordsActive();
            ChartConfigData chartConfigData = await SetChartAccountActiveAsync(banks);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ChartConfigData> ConfigDataBalanceSum()
    {
        try
        {
            BankBalanceSumDTO bankBalanceSum = await _bankService.GetBankBalancesSum();
            ChartConfigData chartConfigData = await SetChartBalanceSumAsync(bankBalanceSum);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<ChartConfigData> SetChartAccountActiveAsync(List<BankModel>banks)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            if (banks.Count > 0)
            {
                foreach (var bank in banks)
                {
                    chartConfigData.Labels.Add(bank.Description);

                    if (bank.CurrentBalance >= 0 && bank.CurrentBalance <= 1000)
                    {
                        chartConfigDataset.BackgroundColor.Add(BackgroundColor.Gray);
                        chartConfigDataset.BorderColor.Add(BorderColor.Gray);
                    }
                    else if (bank.CurrentBalance >= 1000 && bank.CurrentBalance <= 20000)
                    {
                        chartConfigDataset.BackgroundColor.Add(BackgroundColor.Green);
                        chartConfigDataset.BorderColor.Add(BorderColor.Green);
                    }
                    else
                    {
                        chartConfigDataset.BackgroundColor.Add(BackgroundColor.Blue);
                        chartConfigDataset.BorderColor.Add(BorderColor.Blue);
                    }

                    chartConfigDataset.Data.Add(bank.CurrentBalance.ToString());
                }

                chartConfigData.Datasets.Add(chartConfigDataset);

                return chartConfigData;
            }
            else
            {
                return await Task.FromResult(chartConfigData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<ChartConfigData> SetChartBalanceSumAsync(BankBalanceSumDTO bankBalanceSum)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            if (bankBalanceSum is not null)
            {
                chartConfigData.Labels.Add(Graphic.BankBalanceInitialSum);
                chartConfigData.Labels.Add(Graphic.BankBalanceCurrentSum);

                chartConfigDataset.Label = "Balances";

                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Gray);
                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Green);

                chartConfigDataset.BorderColor.Add(BorderColor.Gray);
                chartConfigDataset.BorderColor.Add(BorderColor.Green);

                chartConfigDataset.Data.Add(bankBalanceSum.BankTotalInitialBalance.ToString());
                chartConfigDataset.Data.Add(bankBalanceSum.BankTotalCurrentBalance.ToString());

                chartConfigData.Datasets.Add(chartConfigDataset);

                return chartConfigData;
            }
            else
            {
                return await Task.FromResult(chartConfigData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
