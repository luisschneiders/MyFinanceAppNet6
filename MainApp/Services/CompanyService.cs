using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class CompanyService : ICompanyService<CompanyModel>
{
    [Inject]
    private ICompanyData<CompanyModel> _companyData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    public CompanyService(ICompanyData<CompanyModel> companyData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _companyData = companyData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task ArchiveRecord(CompanyModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.IsArchived = true;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _companyData.ArchiveRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(CompanyModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _companyData.CreateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _companyData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task<CompanyModel> GetRecordById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            CompanyModel result = await _companyData.GetRecordById(user.Id, modelId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    // TODO: Add pagination capabilities
    public async Task<List<CompanyModel>> GetRecords()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<CompanyModel> results = await _companyData.GetRecords(user.Id);
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
            var user = await GetLoggedInUser();
            List<CompanyModel> results = await _companyData.GetRecords(user.Id);

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

    public async Task<List<CompanyModel>> GetRecordsActive()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<CompanyModel> results = await _companyData.GetRecordsActive(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<CompanyModel>> GetSearchResults(string search)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<CompanyModel> results = await _companyData.GetSearchResults(user.Id, search);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecord(CompanyModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _companyData.UpdateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(CompanyModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();

            model.IsActive = !model.IsActive;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _companyData.UpdateRecordStatus(model);
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

    public async Task<decimal> GetHourRate(string modelId)
    {
        try
        {
            CompanyModel companyModel = await GetRecordById(modelId);
            return companyModel.Rate;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
