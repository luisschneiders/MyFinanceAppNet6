namespace MyFinanceAppLibrary.DataAccess.Sql;

public class LocationExpenseData : ILocationExpenseData<LocationExpenseModel>
{
    private readonly IDataAccess _dataAccess;

    public LocationExpenseData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<List<LocationExpenseDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange)
    {
        try
        {
            var results = await _dataAccess.LoadData<LocationExpenseDTO, dynamic>(
                "myfinancedb.spLocationExpense_GetRecordsByDateRange",
                new
                {
                    userId = userId,
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
