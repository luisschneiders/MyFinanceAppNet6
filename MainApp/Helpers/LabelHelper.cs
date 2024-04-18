namespace MainApp.Helpers;

public static class LabelHelper
{
    public static async Task<List<string>> GetMonths()
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public static async Task<List<string>> GetDays()
    {
        List<string> labels = new();

        for (int i = 1; i <= 31; ++i)
        {
            labels.Add(i.ToString());
        }

        return await Task.FromResult(labels);
    }
}
