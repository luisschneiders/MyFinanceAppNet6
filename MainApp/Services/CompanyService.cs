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

    private UserModel _loggedInUser { get; set; } = new();

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

            CompanyModel recordModel = model;
            recordModel.IsArchived = true;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _companyData.ArchiveRecord(recordModel);
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
            CompanyModel recordModel = new()
            {
                Description = model.Description,
                Rate = model.Rate,
                CType = model.CType,
                UpdatedBy = user.Id
            };

            await _companyData.CreateRecord(recordModel);
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
            CompanyModel recordModel = new()
            {
                Id = model.Id,
                Description = model.Description,
                Rate = model.Rate,
                CType = model.CType,
                IsActive = model.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _companyData.UpdateRecord(recordModel);
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

            CompanyModel recordModel = model;
            recordModel.IsActive = !model.IsActive;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _companyData.UpdateRecordStatus(recordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }

    public Task<List<CompanyModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }
}
