namespace MainApp.Components.Chart.Expense; 

public class ChartExpenseService : IChartExpenseService
{
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;
    
    public ChartExpenseService(
        IExpenseService<ExpenseModel> expenseService, 
        IExpenseCategoryService<ExpenseCategoryModel> expenseCategoryService)
    {
        _expenseService = expenseService;
        _expenseCategoryService = expenseCategoryService;
    }

    public async Task<ChartConfigData> ConfigDataLast3Months()
    {
        try
        {
            List<ExpenseLast3MonthsGraphDTO> expenseLast3Months = await _expenseService.GetRecordsLast3Months();
            ChartConfigData chartConfigData = await SetConfigDataLast3Months(expenseLast3Months);

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
            ChartConfigData chartConfigData = await SetConfigDataLast5Years(expenseLast5Years);

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
            List<ExpenseTop5DTO> expenseTop5 = await _expenseService.GetRecordsTop5ByDate(dateTimeRange);
            ChartConfigData chartConfigData = await SetConfigDataTop5(expenseTop5);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ChartConfigData> ConfigDataByMonth(MultiFilterExpenseByMonthDTO filter)
    {
        try
        {
            List<ExpenseListGroupByMonthDTO> expenses = new();

            expenses = await _expenseService.GetRecordsGroupByMonth(filter.DateTimeRange);

            ChartConfigData chartConfigData = await SetConfigDataByMonth(expenses, filter);

            return chartConfigData;

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<ChartConfigData> SetConfigDataByMonth(List<ExpenseListGroupByMonthDTO> expenses, MultiFilterExpenseByMonthDTO filter)
    {
        try
        {
            List<ExpenseListGroupByMonthDTO> expensesFiltered = new();
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new()
            {
                Label = "Select expense",
                Data = new List<string>(),
            };

            List<string> chartLabels = await LabelHelper.GetMonths();
            ExpenseChartData expenseChartData = new();

            chartConfigData.Labels = chartLabels;

            if (filter.IsFilterChanged is true)
            {
                chartConfigData.Datasets.Remove(chartConfigDataset);

                if (expenses.Count > 0)
                {
                    foreach (var id in filter.ECategoryId)
                    {
                        chartConfigDataset = new();
                        expenseChartData = await SetByMonth();
                        expensesFiltered = expenses.Where(e => e.ECategoryId == id).ToList();

                        if (expensesFiltered.Count > 0)
                        {
                            foreach (var (item, index) in chartLabels.Select((value, index) => (value, index)))
                            {
                                var i = index + 1;
                                var record = expensesFiltered.Find(e => e.MonthNumber == i);

                                if (i == record?.MonthNumber)
                                {
                                    expenseChartData.Records[index] = record.TotalAmount.ToString();
                                }
                            }

                            chartConfigDataset.Label = expensesFiltered[0].ECategoryDescription;
                            chartConfigDataset.BackgroundColor.Add($"rgba({expensesFiltered[0].ECategoryColor},0.2)");
                            chartConfigDataset.BorderColor.Add($"rgb({expensesFiltered[0].ECategoryColor})");
                            chartConfigDataset.Data = expenseChartData.Records;

                            chartConfigData.Datasets.Add(chartConfigDataset);

                        }
                        else
                        {
                            ExpenseCategoryModel expenseCategory = await _expenseCategoryService.GetRecordById(id.ToString());

                            chartConfigDataset.Label = expenseCategory.Description;
                            chartConfigDataset.BackgroundColor.Add($"rgba({expenseCategory.Color},0.2)");
                            chartConfigDataset.BorderColor.Add($"rgb({expenseCategory.Color})");
                            chartConfigDataset.Data = new List<string>();
                            chartConfigData.Datasets.Add(chartConfigDataset);
                        }

                    }

                    return await Task.FromResult(chartConfigData);
                }
                else
                {
                    return await Task.FromResult(chartConfigData);
                }
            }
            else
            {
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

    private static async Task<ChartConfigData> SetConfigDataLast3Months(List<ExpenseLast3MonthsGraphDTO> expenseLast3Months)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();
            List<string> chartBackgroundColors = await SetBackgroundColorLast3Months();
            List<string> chartBorderColors = await SetBorderColorsLast3Months();

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

    private static async Task<ChartConfigData> SetConfigDataLast5Years(List<ExpenseLast5YearsGraphDTO> expenseLast5Years)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();
            List<string> chartBackgroundColors = await SetBackgroundColorLast5Years();
            List<string> chartBorderColors = await SetBorderColorsLast5Years();

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

    private static async Task<ChartConfigData> SetConfigDataTop5(List<ExpenseTop5DTO> expenses)
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

    private static async Task<List<string>> SetBackgroundColorLast3Months()
    {
        List<string> backgroundColors = new()
        {
            BackgroundColor.Orange,
            BackgroundColor.Purple,
            BackgroundColor.Green
        };

        return await Task.FromResult(backgroundColors);
    }

    private static async Task<List<string>> SetBorderColorsLast3Months()
    {
        List<string> borderColors = new()
        {
            BorderColor.Orange,
            BorderColor.Purple,
            BorderColor.Green
        };

        return await Task.FromResult(borderColors);
    }

    private static async Task<List<string>> SetBackgroundColorLast5Years()
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

    private static async Task<List<string>> SetBorderColorsLast5Years()
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

    private static async Task<ExpenseChartData> SetByMonth()
    {
        ExpenseChartData data = new();

        for (int i = 0; i <= 12; ++i)
        {
            data.Records.Add("0");
        }

        return await Task.FromResult(data);
    }
}
