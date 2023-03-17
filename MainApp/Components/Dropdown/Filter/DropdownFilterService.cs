namespace MainApp.Components.Dropdown.Filter;

public class DropdownFilterService : IDropdownFilterService
{
    public DropdownFilterService()
    {
    }

    public async Task<FilterModel> ResetModel()
    {
        var model = new FilterModel();
        return await Task.FromResult(model);
    }

    public async Task<FilterModel> SetModel(ulong id, string description)
    {
        FilterModel model = new();
        model.Id = id;
        model.Description = description;
        return await Task.FromResult(model);
    }

    public async Task<string> UpdateLabel(string description)
    {
        string label = string.Empty;
        label = description;
        return await Task.FromResult(label);
    }
}
