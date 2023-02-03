namespace MyFinanceAppLibrary.DataAccess.Sql;

public class CompanyData : ICompanyData<CompanyModel>
{
    private readonly IDataAccess _dataAccess;

    public CompanyData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(CompanyModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spCompany_Archive",
                new
                {
                    companyId = model.Id,
                    companyIsArchived = model.IsArchived,
                    companyUpdatedBy = model.UpdatedBy,
                    companyUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
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
            await _dataAccess.LoadData<CompanyModel, dynamic>(
                "myfinancedb.spCompany_Create",
                new
                {
                    companyDescription = model.Description,
                    companyRate = model.Rate,
                    companyCType = model.CType,
                    companyUpdatedBy = model.UpdatedBy,
                    companyCreatedAt = model.CreatedAt,
                    companyUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _dataAccess.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task<CompanyModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<CompanyModel, dynamic>(
                "myfinancedb.spCompany_GetById",
                new { userId = userId, companyId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<CompanyModel>> GetRecords(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<CompanyModel, dynamic>(
                "myfinancedb.spCompany_GetAll",
                new { userId = userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<CompanyModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<CompanyModel, dynamic>(
                "myfinancedb.spCompany_GetSearchResults",
                new
                {
                    userId = userId,
                    searchCompany = search,
                },
                "Mysql");

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
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spCompany_Update",
                new
                {
                    companyId = model.Id,
                    companyDescription = model.Description,
                    companyRate = model.Rate,
                    companyCType = model.CType,
                    companyIsActive = model.IsActive,
                    companyUpdatedBy = model.UpdatedBy,
                    companyUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(CompanyModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spCompany_UpdateStatus",
                new
                {
                    companyId = model.Id,
                    companyIsActive = model.IsActive,
                    companyUpdatedBy = model.UpdatedBy,
                    companyUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
