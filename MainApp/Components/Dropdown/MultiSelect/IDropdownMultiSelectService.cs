namespace MainApp.Components.Dropdown.MultiSelect;

public interface IDropdownMultiSelectService
{
    public Task<List<CheckboxItemModel>> UncheckAll(List<CheckboxItemModel> checkboxItems);
}
