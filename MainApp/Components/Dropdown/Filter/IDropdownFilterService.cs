namespace MainApp.Components.Dropdown.Filter;

public interface IDropdownFilterService
{
    Task<FilterModel> SetModel(ulong id, string description);
    Task<FilterModel> ResetModel();
    Task<string> UpdateLabel(string description);
}
