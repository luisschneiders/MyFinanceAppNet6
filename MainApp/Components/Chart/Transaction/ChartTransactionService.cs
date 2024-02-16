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
            ChartConfigData chartConfigData = await SetChartConfigDataIOByMonth(transactionIO);

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
            ChartConfigData chartConfigData = await SetChartConfigDataIOByDay(transactionIO);

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
            ChartConfigData chartConfigData = await SetChartConfigDataLast3Months(transactions);

            return chartConfigData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<ChartConfigData> SetChartConfigDataIOByMonth(List<TransactionIOGraphByMonthDTO> transactions)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await SetChartIOLabelByMonth();
            TransactionChartData transactionChartData = await SetChartIODataByMonth();

            List<TransactionIOGraphByMonthDTO> incomes = transactions.Where(t => t.Label == "C").ToList();
            List<TransactionIOGraphByMonthDTO> outcomes = transactions.Where(t => t.Label == "D").ToList();

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

    private static async Task<ChartConfigData> SetChartConfigDataIOByDay(List<TransactionIOGraphByDayDTO> transactions)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await SetChartIOLabelByDay();
            TransactionChartData transactionChartData = await SetChartIODataByDay();

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

    private static async Task<List<string>> SetChartIOLabelByMonth()
    {
        List<string> labels = new()
        {
            Months.January.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.February.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.March.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.April.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.May.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.June.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.July.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.August.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.September.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.October.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.November.ToString().Truncate((int)Truncate.ShortMonthName, "")!,
            Months.December.ToString().Truncate((int)Truncate.ShortMonthName, "")!
        };

        return await Task.FromResult(labels);
    }

    private static async Task<List<string>> SetChartIOLabelByDay()
    {
        List<string> labels = new();

        for (int i = 1; i <= 31; ++i)
        {
            labels.Add(i.ToString());
        }

        return await Task.FromResult(labels);
    }

    private static async Task<TransactionChartData> SetChartIODataByMonth()
    {
        TransactionChartData data = new();

        for (int i = 0; i <= 12; ++i)
        {
            data.Income.Add("0");
            data.Outcome.Add("0");
        }

        return await Task.FromResult(data);
    }

    private static async Task<TransactionChartData> SetChartIODataByDay()
    {
        TransactionChartData data = new();

        for (int i = 1; i <= 31; ++i)
        {
            data.Income.Add("0");
            data.Outcome.Add("0");
        }

        return await Task.FromResult(data);
    }

    private static async Task<ChartConfigData> SetChartConfigDataLast3Months(List<TransactionIOLast3MonthsGraphDTO> transactions)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await SetChartLast3MonthsLabel();
            TransactionChartData transactionChartData = await SetChartLast3MonthsData();

            List<TransactionIOLast3MonthsGraphDTO> incomes = transactions.Where(t => t.Label == "C").ToList();
            List<TransactionIOLast3MonthsGraphDTO> outcomes = transactions.Where(t => t.Label == "D").ToList();

            if (transactions.Count > 0)
            {
                for (int index = 0; index <= 2; index++)
                {
                    if (incomes.Count > 0)
                    {
                        transactionChartData.Income[index] = incomes[index].TotalAmount.ToString();
                    }

                    if (outcomes.Count > 0)
                    {
                        transactionChartData.Outcome[index] = outcomes[index].TotalAmount.ToString();
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
                chartConfigDataset = new();

                chartConfigDataset.Label = "Outcome";
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

    private static async Task<List<string>> SetChartLast3MonthsLabel()
    {
        List<string> labels = new();
        for (int i = 2; i >= 0; --i)
        {
            // previous 3 months
            labels.Add($"{DateTime.Now.AddMonths(-(i + 1)).Month}/{DateTime.Now.AddMonths(-(i + 1)).Year}");
        }

        return await Task.FromResult(labels);
    }

    private static async Task<TransactionChartData> SetChartLast3MonthsData()
    {
        TransactionChartData data = new();

        for (int i = 2; i >= 0; --i)
        {
            data.Income.Add("0");
            data.Outcome.Add("0");
        }

        return await Task.FromResult(data);
    }
}
