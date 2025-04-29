namespace MyFinanceAppLibrary.DataAccess.Sql;

public class LocationExpenseData : ILocationExpenseData<LocationExpenseModel>
{
    private readonly IDataAccess _dataAccess;

    public LocationExpenseData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<List<LocationModel>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange)
    {
        try
        {
            var results = await _dataAccess.LoadData<LocationModel, dynamic>(
                "myfinancedb.spLocationExpense_GetRecordsByDateRange",
                new
                {
                    userId,
                    startDate = dateTimeRange.Start,
                    endDate = dateTimeRange.End
                },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
