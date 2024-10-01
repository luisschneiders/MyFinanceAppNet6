namespace MainApp.Components.Dropdown.Filter;

public interface IDropdownFilterService
{
    public Task<FilterModel> SetModel(ulong id, string description);
    public Task<FilterModel> ResetModel();
    public Task<string> UpdateLabel(string description);
}
