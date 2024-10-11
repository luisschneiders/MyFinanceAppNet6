namespace MyFinanceAppLibrary.DataAccess.Sql;

public class TaxCategoryData : ITaxCategoryData<TaxCategoryModel>
{
    private readonly IDataAccess _dataAccess;

    public TaxCategoryData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(TaxCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTaxCategory_Archive",
                new
                {
                    taxCategoryId = model.Id,
                    taxCategoryIsArchived = model.IsArchived,
                    taxCategoryUpdatedBy = model.UpdatedBy,
                    taxCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(TaxCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTaxCategory_Create",
                new
                {
                    taxCategoryDescription = model.Description,
                    taxCategoryRate = model.Rate,
                    taxCategoryUpdatedBy = model.UpdatedBy,
                    taxCategoryCreatedAt = model.CreatedAt,
                    taxCategoryUpdatedAt = model.UpdatedAt,
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

    public async Task<TaxCategoryModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<TaxCategoryModel, dynamic>(
                "myfinancedb.spTaxCategory_GetById",
                new { userId, taxCategoryId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TaxCategoryModel>> GetRecords(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<TaxCategoryModel, dynamic>(
                "myfinancedb.spTaxCategory_GetAll",
                new { userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TaxCategoryModel>> GetRecordsActive(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<TaxCategoryModel, dynamic>(
                "myfinancedb.spTaxCategory_GetAllActive",
                new { userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TaxCategoryModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<TaxCategoryModel, dynamic>(
                "myfinancedb.spTaxCategory_GetSearchResults",
                new
                {
                    userId,
                    searchTaxCategory = search,
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

    public async Task UpdateRecord(TaxCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTaxCategory_Update",
                new
                {
                    taxCategoryId = model.Id,
                    taxCategoryDescription = model.Description,
                    taxCategoryRate = model.Rate,
                    taxCategoryIsActive = model.IsActive,
                    taxCategoryUpdatedBy = model.UpdatedBy,
                    taxCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(TaxCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTaxCategory_UpdateStatus",
                new
                {
                    taxCategoryId = model.Id,
                    taxCategoryIsActive = model.IsActive,
                    taxCategoryUpdatedBy = model.UpdatedBy,
                    taxCategoryUpdatedAt = model.UpdatedAt,
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
