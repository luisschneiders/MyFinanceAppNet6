
namespace MainApp.Components.Dropdown.MultiSelect;

public class DropdownMultiSelectService : IDropdownMultiSelectService
{
    public DropdownMultiSelectService()
    {
    }

    public async Task<List<CheckboxItemModel>> UncheckAll(List<CheckboxItemModel> checkboxItems)
    {
        try
        {
            foreach (var item in checkboxItems)
            {
                item.IsChecked = false;
            }

            return await Task.FromResult(checkboxItems);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
