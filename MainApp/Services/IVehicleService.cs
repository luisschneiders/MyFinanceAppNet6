namespace MainApp.Services;

public interface IVehicleService<T> : IBaseService<T>
{
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
}
