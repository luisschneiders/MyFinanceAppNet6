using System.Globalization;

namespace MainApp.Components.Chart.Transaction;

public class ChartTransactionService : IChartTransactionService
{
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    public ChartTransactionService(ITransactionService<TransactionModel> transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<ChartConfigData> ConfigDataIOByMonth(DateTimeRange dateTimeRange)
    {
        try
        {
            List<TransactionIOGraphByMonthDTO> transactionIO = await _transactionService.GetIOByDateRangeGroupByMonth(dateTimeRange);
            ChartConfigData chartConfigData = await SetConfigDataIOByMonth(transactionIO);

            return chartConfigData;

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ChartConfigData> ConfigDataIOByDay(DateTimeRange dateTimeRange)
    {
        try
        {
            List<TransactionIOGraphByDayDTO> transactionIO = await _transactionService.GetIOByDateRangeGroupByDay(dateTimeRange);
            ChartConfigData chartConfigData = await SetConfigDataIOByDay(transactionIO);

            return chartConfigData;

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ChartConfigData> ConfigDataLast3Months()
    {
        try
        {
            List<TransactionIOLast3MonthsGraphDTO> transactions =  await _transactionService.GetRecordsLast3Months();
            ChartConfigData chartConfigData = await SetConfigDataLast3Months(transactions);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<ChartConfigData> SetConfigDataIOByMonth(List<TransactionIOGraphByMonthDTO> transactions)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await LabelHelper.GetMonths();
            TransactionChartData transactionChartData = await SetDataIOByMonth();

            List<TransactionIOGraphByMonthDTO> incomes = transactions.Where(t => t.Label == "C").ToList();
            List<TransactionIOGraphByMonthDTO> outcomes = transactions.Where(t => t.Label == "D").ToList();

            chartConfigData.Labels = chartLabels;

            if (transactions.Count > 0)
            {
                foreach (var (item, index) in chartLabels.Select((value, index) => (value, index)))
                {
                    var i = index + 1;
                    var record = incomes.Find(m => m.MonthNumber == i);

                    if (i == record?.MonthNumber)
                    {
                        transactionChartData.Income[index] = record.TotalAmount.ToString();
                    }
                }

                foreach (var (item, index) in chartLabels.Select((value, index) => (value, index)))
                {
                    var i = index + 1;
                    var record = outcomes.Find(m => m.MonthNumber == i);

                    if (i == record?.MonthNumber)
                    {
                        transactionChartData.Outcome[index] = record.TotalAmount.ToString();
                    }
                }

                //Incomes data
                chartConfigDataset.Label = "Income";
                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Blue);
                chartConfigDataset.BorderColor.Add(BorderColor.Blue);
                chartConfigDataset.Data = transactionChartData.Income;

                chartConfigData.Datasets.Add(chartConfigDataset);

                //Outcomes data
                chartConfigDataset = new()
                {
                    Label = "Outcome"
                };
                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Red);
                chartConfigDataset.BorderColor.Add(BorderColor.Red);
                chartConfigDataset.Data = transactionChartData.Outcome;

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

    private static async Task<ChartConfigData> SetConfigDataIOByDay(List<TransactionIOGraphByDayDTO> transactions)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await LabelHelper.GetDays();
            TransactionChartData transactionChartData = await SetDataIOByDay();

            List<TransactionIOGraphByDayDTO> incomes = transactions.Where(t => t.Label == "C").ToList();
            List<TransactionIOGraphByDayDTO> outcomes = transactions.Where(t => t.Label == "D").ToList();

            if (transactions.Count > 0)
            {
                foreach (var (item, index) in chartLabels.Select((value, index) => (value, index)))
                {
                    var i = index + 1;
                    var record = incomes.Find(m => m.DayNumber == i);

                    if (i == record?.DayNumber)
                    {
                        transactionChartData.Income[index] = record.TotalAmount.ToString();
                    }
                }

                foreach (var (item, index) in chartLabels.Select((value, index) => (value, index)))
                {
                    var i = index + 1;
                    var record = outcomes.Find(m => m.DayNumber == i);

                    if (i == record?.DayNumber)
                    {
                        transactionChartData.Outcome[index] = record.TotalAmount.ToString();
                    }
                }

                chartConfigData.Labels = chartLabels;

                //Incomes data
                chartConfigDataset.Label = "Income";
                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Blue);
                chartConfigDataset.BorderColor.Add(BorderColor.Blue);
                chartConfigDataset.Data = transactionChartData.Income;

                chartConfigData.Datasets.Add(chartConfigDataset);

                //Outcomes data
                chartConfigDataset = new()
                {
                    Label = "Outcome"
                };
                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Red);
                chartConfigDataset.BorderColor.Add(BorderColor.Red);
                chartConfigDataset.Data = transactionChartData.Outcome;

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

    private static async Task<ChartConfigData> SetConfigDataLast3Months(List<TransactionIOLast3MonthsGraphDTO> transactions)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await SetLast3MonthsLabel();
            TransactionChartData transactionChartData = await SetDataLast3Months();

            List<TransactionIOLast3MonthsGraphDTO> incomes = transactions.Where(t => t.Label == "C").ToList();
            List<TransactionIOLast3MonthsGraphDTO> outcomes = transactions.Where(t => t.Label == "D").ToList();

            if (transactions.Count > 0)
            {

                foreach (var (item, index) in chartLabels.Select((value, index) => (value, index)))
                {
                    DateTime date = DateTime.ParseExact(item, "M/yyyy", CultureInfo.InvariantCulture);
                    int month = date.Month;

                    var record = incomes.Find(m => m.MonthNumber == month);

                    if (month == record?.MonthNumber)
                    {
                        transactionChartData.Income[index] = record.TotalAmount.ToString();
                    }
                }

                foreach (var (item, index) in chartLabels.Select((value, index) => (value, index)))
                {
                    DateTime date = DateTime.ParseExact(item, "M/yyyy", CultureInfo.InvariantCulture);
                    int month = date.Month;

                    var record = outcomes.Find(m => m.MonthNumber == month);

                    if (month == record?.MonthNumber)
                    {
                        transactionChartData.Outcome[index] = record.TotalAmount.ToString();
                    }
                }

                chartConfigData.Labels = chartLabels;

                //Incomes data
                chartConfigDataset.Label = "Income";
                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Blue);
                chartConfigDataset.BorderColor.Add(BorderColor.Blue);
                chartConfigDataset.Data = transactionChartData.Income;

                chartConfigData.Datasets.Add(chartConfigDataset);

                //Outcomes data
                chartConfigDataset = new()
                {
                    Label = "Outcome"
                };
                chartConfigDataset.BackgroundColor.Add(BackgroundColor.Red);
                chartConfigDataset.BorderColor.Add(BorderColor.Red);
                chartConfigDataset.Data = transactionChartData.Outcome;

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

    private static async Task<TransactionChartData> SetDataIOByMonth()
    {
        TransactionChartData data = new();

        for (int i = 0; i <= 12; ++i)
        {
            data.Income.Add("0");
            data.Outcome.Add("0");
        }

        return await Task.FromResult(data);
    }

    private static async Task<TransactionChartData> SetDataIOByDay()
    {
        TransactionChartData data = new();

        for (int i = 1; i <= 31; ++i)
        {
            data.Income.Add("0");
            data.Outcome.Add("0");
        }

        return await Task.FromResult(data);
    }

    private static async Task<TransactionChartData> SetDataLast3Months()
    {
        TransactionChartData data = new();

        for (int i = 2; i >= 0; --i)
        {
            data.Income.Add("0");
            data.Outcome.Add("0");
        }

        return await Task.FromResult(data);
    }

    private static async Task<List<string>> SetLast3MonthsLabel()
    {
        List<string> labels = new();
        for (int i = 2; i >= 0; --i)
        {
            // previous 3 months
            labels.Add($"{DateTime.Now.AddMonths(-(i + 1)).Month}/{DateTime.Now.AddMonths(-(i + 1)).Year}");
        }

        return await Task.FromResult(labels);
    }

}
