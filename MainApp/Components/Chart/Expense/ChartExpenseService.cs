using MyFinanceAppLibrary.Models;

namespace MainApp.Components.Chart.Expense; 

public class ChartExpenseService : IChartExpenseService
{
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    public ChartExpenseService(IExpenseService<ExpenseModel> expenseService)
    {
        _expenseService = expenseService;
    }

    public async Task<ChartConfigData> ConfigDataLast3Months()
    {
        try
        {
            List<ExpenseLast3MonthsGraphDTO> expenseLast3Months = await _expenseService.GetRecordsLast3Months();
            ChartConfigData chartConfigData = await SetChartLast3MonthsAsync(expenseLast3Months);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ChartConfigData> ConfigDataLast5Years()
    {
        try
        {
            List<ExpenseLast5YearsGraphDTO> expenseLast5Years = await _expenseService.GetRecordsLast5Years();
            ChartConfigData chartConfigData = await SetChartLast5YearsAsync(expenseLast5Years);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<ChartConfigData> SetChartLast3MonthsAsync(List<ExpenseLast3MonthsGraphDTO> expenseLast3Months)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();
            List<string> chartBackgroundColors = await SetChartBackgroundColorLast3Months();
            List<string> chartBorderColors = await SetChartBorderColorsLast3Months();

            if (expenseLast3Months.Count > 0)
            {

                foreach (var (item, index) in expenseLast3Months.Select((value, index) => (value, index)))
                {
                    chartConfigDataset.Label = "Total expenses";
                    chartConfigDataset.BackgroundColor.Add(chartBackgroundColors[index]);
                    chartConfigDataset.BorderColor.Add(chartBorderColors[index]);
                    chartConfigDataset.Data.Add(item.TotalAmount.ToString());
                    chartConfigData.Labels.Add($"{item.MonthNumber}/{item.YearNumber}");
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

    private static async Task<List<string>> SetChartBackgroundColorLast3Months()
    {
        List<string> backgroundColors = new();
        backgroundColors.Add(BackgroundColor.Orange);
        backgroundColors.Add(BackgroundColor.Purple);
        backgroundColors.Add(BackgroundColor.Green);

        return await Task.FromResult(backgroundColors);
    }

    private static async Task<List<string>> SetChartBorderColorsLast3Months()
    {
        List<string> borderColors = new();
        borderColors.Add(BorderColor.Orange);
        borderColors.Add(BorderColor.Purple);
        borderColors.Add(BorderColor.Green);

        return await Task.FromResult(borderColors);
    }

    private static async Task<ChartConfigData> SetChartLast5YearsAsync(List<ExpenseLast5YearsGraphDTO> expenseLast5Years)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();
            List<string> chartBackgroundColors = await SetChartBackgroundColorLast5Years();
            List<string> chartBorderColors = await SetChartBorderColorsLast5Years();

            if (expenseLast5Years.Count > 0)
            {

                foreach (var (item, index) in expenseLast5Years.Select((value, index) => (value, index)))
                {
                    chartConfigDataset.Label = "Total expenses";
                    chartConfigDataset.BackgroundColor.Add(chartBackgroundColors[index]);
                    chartConfigDataset.BorderColor.Add(chartBorderColors[index]);
                    chartConfigDataset.Data.Add(item.TotalAmount.ToString());
                    chartConfigData.Labels.Add($"{item.YearNumber}");
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

    private static async Task<List<string>> SetChartBackgroundColorLast5Years()
    {
        List<string> backgroundColors = new();
        backgroundColors.Add(BackgroundColor.Orange);
        backgroundColors.Add(BackgroundColor.Purple);
        backgroundColors.Add(BackgroundColor.Green);
        backgroundColors.Add(BackgroundColor.Blue);
        backgroundColors.Add(BackgroundColor.Yellow);

        return await Task.FromResult(backgroundColors);
    }

    private static async Task<List<string>> SetChartBorderColorsLast5Years()
    {
        List<string> borderColors = new();
        borderColors.Add(BorderColor.Orange);
        borderColors.Add(BorderColor.Purple);
        borderColors.Add(BorderColor.Green);
        borderColors.Add(BorderColor.Blue);
        borderColors.Add(BorderColor.Yellow);

        return await Task.FromResult(borderColors);
    }
}
