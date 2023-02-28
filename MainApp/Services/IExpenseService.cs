namespace MainApp.Services;

public interface IExpenseService<T> : IBaseService<T>
{
    Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<ExpenseModelByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
}
