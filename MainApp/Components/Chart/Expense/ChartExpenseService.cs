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

    public async Task<ChartConfigData> ConfigDataTop5(DateTimeRange dateTimeRange)
    {
        try
        {
            List<ExpenseTop5DTO> expenseTop5 = await _expenseService.GetRecordsTop5ByDateRange(dateTimeRange);
            ChartConfigData chartConfigData = await SetChartConfigDataTop5(expenseTop5);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ChartConfigData> ConfigDataExpenseByMonth(FilterExpenseByMonthDTO filter)
    {
        try
        {
            List<ExpenseListGroupByMonthDTO> expenses = new();
            List<ExpenseListGroupByMonthDTO> expensesFiltered = new();

            if (filter.IsDateChanged is true)
            {
                expenses = await _expenseService.GetRecordsGroupByMonth(filter.DateTimeRange);
            }

            if (filter.IsFilterChanged is true)
            {
                expensesFiltered = expenses.Where(e => e.ECategoryId == filter.ExpenseCategoryModel.Id).ToList();
            }
            else
            {
                expensesFiltered = new();
            }

            ChartConfigData chartConfigData = await SetChartConfigDataExpenseByMonth(expensesFiltered);

            return chartConfigData;

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<ChartConfigData> SetChartConfigDataExpenseByMonth(List<ExpenseListGroupByMonthDTO> expenses)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await LabelHelper.GetMonths();
            chartConfigData.Labels = chartLabels;

            if (expenses.Count > 0)
            {
                foreach (var (item, index) in expenses.Select((value, index) => (value,index)))
                {
                    chartConfigDataset.Label = item.ECategoryDescription;
                    chartConfigDataset.BackgroundColor.Add($"rgba({item.ECategoryColor},0.2)");
                    chartConfigDataset.BorderColor.Add($"rgb({item.ECategoryColor})");
                    chartConfigDataset.Data.Add(item.TotalAmount.ToString());
                }

                chartConfigData.Datasets.Add(chartConfigDataset);

                return chartConfigData;
            }
            else
            {
                chartConfigDataset.Label = "Select expense";
                chartConfigData.Datasets.Add(chartConfigDataset);

                return await Task.FromResult(chartConfigData);
            }
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
        List<string> backgroundColors = new()
        {
            BackgroundColor.Orange,
            BackgroundColor.Purple,
            BackgroundColor.Green
        };

        return await Task.FromResult(backgroundColors);
    }

    private static async Task<List<string>> SetChartBorderColorsLast3Months()
    {
        List<string> borderColors = new()
        {
            BorderColor.Orange,
            BorderColor.Purple,
            BorderColor.Green
        };

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
        List<string> backgroundColors = new()
        {
            BackgroundColor.Orange,
            BackgroundColor.Purple,
            BackgroundColor.Green,
            BackgroundColor.Blue,
            BackgroundColor.Yellow
        };

        return await Task.FromResult(backgroundColors);
    }

    private static async Task<List<string>> SetChartBorderColorsLast5Years()
    {
        List<string> borderColors = new()
        {
            BorderColor.Orange,
            BorderColor.Purple,
            BorderColor.Green,
            BorderColor.Blue,
            BorderColor.Yellow
        };

        return await Task.FromResult(borderColors);
    }

    private static async Task<ChartConfigData> SetChartConfigDataTop5(List<ExpenseTop5DTO> expenses)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            if (expenses.Count > 0)
            {
                foreach (var (item, index) in expenses.Select((value, index) => (value,index)))
                {
                    chartConfigDataset.Label = "Expenses";
                    chartConfigDataset.BackgroundColor.Add($"rgba({item.ECategoryColor},0.2)");
                    chartConfigDataset.BorderColor.Add($"rgb({item.ECategoryColor})");
                    chartConfigDataset.Data.Add(item.TotalAmount.ToString());
                    chartConfigData.Labels.Add(item.ECategoryDescription.Truncate((int)Truncate.ExpenseCategory)!);
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
}
