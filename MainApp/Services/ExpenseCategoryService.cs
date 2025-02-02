using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class ExpenseCategoryService : IExpenseCategoryService<ExpenseCategoryModel>
{
    [Inject]
    private IExpenseCategoryData<ExpenseCategoryModel> _expenseCategoryData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    public ExpenseCategoryService(IExpenseCategoryData<ExpenseCategoryModel> expenseCategoryData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _expenseCategoryData = expenseCategoryData;
        _userData = userData;
        _authProvider = authProvider;
    }

    // TODO: Add pagination capabilities
    public async Task<List<ExpenseCategoryModel>> GetRecords()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<ExpenseCategoryModel> results = await _expenseCategoryData.GetRecords(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<CheckboxItemModel>> GetRecordsForFilter()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<ExpenseCategoryModel> results = await _expenseCategoryData.GetRecords(user.Id);

            List<CheckboxItemModel> filter = new();

            foreach (var item in results)
            {
                CheckboxItemModel filterItem = new()
                {
                    Id = item.Id,
                    Description = item.Description,
                };
                filter.Add(filterItem);
            }

            return filter;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseCategoryModel>> GetSearchResults(string search)
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<ExpenseCategoryModel> results = await _expenseCategoryData.GetSearchResults(user.Id, search);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ExpenseCategoryModel> GetRecordById(string modelId)
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            ExpenseCategoryModel result = await _expenseCategoryData.GetRecordById(user.Id, modelId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _expenseCategoryData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task CreateRecord(ExpenseCategoryModel model)
    {
        try
        {
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.Color = await SetRandomColor();

            await _expenseCategoryData.CreateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecord(ExpenseCategoryModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _expenseCategoryData.UpdateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(ExpenseCategoryModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            UserModel user = await GetLoggedInUser();

            model.IsActive = !model.IsActive;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _expenseCategoryData.UpdateRecordStatus(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveRecord(ExpenseCategoryModel model)
    {
        try
        {
            UserModel user = await GetLoggedInUser();

            model.IsArchived = true;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _expenseCategoryData.ArchiveRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseCategoryModel>> GetRecordsActive()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<ExpenseCategoryModel> results = await _expenseCategoryData.GetRecordsActive(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return await _authProvider.GetUserFromAuth(_userData);
    }

    private static async Task<string> SetRandomColor()
    {
        Random r = new();
        Color randonColor = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

        return await Task.FromResult($"{randonColor.R},{randonColor.G},{randonColor.B}");
    }
}
