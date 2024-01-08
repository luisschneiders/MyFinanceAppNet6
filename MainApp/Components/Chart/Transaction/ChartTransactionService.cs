namespace MainApp.Components.Chart.Transaction;

public class ChartTransactionService : IChartTransactionService
{
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    public ChartTransactionService(ITransactionService<TransactionModel> transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<ChartConfigData> ConfigDataIO(DateTimeRange dateTimeRange)
    {
        try
        {
            List<TransactionIOGraphByDateDTO> transactionIO = await _transactionService.GetIOByDateRange(dateTimeRange);
            ChartConfigData chartConfigData = await SetChartConfigDataIO(transactionIO);

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

    private static async Task<ChartConfigData> SetChartConfigDataIO(List<TransactionIOGraphByDateDTO> transactions)
    {
        try
        {
            ChartConfigData chartConfigData = new();
            ChartConfigDataset chartConfigDataset = new();

            List<string> chartLabels = await SetChartIOLabel();
            TransactionChartData transactionChartData = await SetChartIOData();

            List<TransactionIOGraphByDateDTO> incomes = transactions.Where(t => t.Label == "C").ToList();
            List<TransactionIOGraphByDateDTO> outcomes = transactions.Where(t => t.Label == "D").ToList();

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

    private static async Task<List<string>> SetChartIOLabel()
    {
        List<string> labels = new();
        labels.Add(Months.January.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.February.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.March.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.April.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.May.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.June.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.July.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.August.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.September.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.October.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.November.ToString().Truncate((int)Truncate.ShortMonthName, "")!);
        labels.Add(Months.December.ToString().Truncate((int)Truncate.ShortMonthName, "")!);

        return await Task.FromResult(labels);
    }

    private static async Task<TransactionChartData> SetChartIOData()
    {
        TransactionChartData data = new();

        for (int i = 0; i <= 12; ++i)
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
