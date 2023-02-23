namespace MainApp.Services;

public interface IExpenseService<T> : IBaseService<T>
{
    Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel);
    Task<List<ExpenseModelByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRangeModel dateTimeRangeModel);
}
