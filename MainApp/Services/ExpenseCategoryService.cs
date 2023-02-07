﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;

namespace MainApp.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService<ExpenseCategoryModel>
    {
        [Inject]
        private IExpenseCategoryData<ExpenseCategoryModel> _expenseCategoryData { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider _authProvider { get; set; } = default!;

        [Inject]
        private IUserData _userData { get; set; } = default!;

        private UserModel _loggedInUser { get; set; } = new();

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
                var user = await GetLoggedInUser();
                List<ExpenseCategoryModel> results = await _expenseCategoryData.GetRecords(user.Id);
                return results;
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
                var user = await GetLoggedInUser();
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
                var user = await GetLoggedInUser();
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
                var user = await GetLoggedInUser();
                ExpenseCategoryModel recordModel = new()
                {
                    Description = model.Description,
                    UpdatedBy = user.Id
                };

                await _expenseCategoryData.CreateRecord(recordModel);
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
                var user = await GetLoggedInUser();
                ExpenseCategoryModel recordModel = new()
                {
                    Id = model.Id,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    UpdatedBy = user.Id,
                    UpdatedAt = DateTime.Now,
                };

                await _expenseCategoryData.UpdateRecord(recordModel);
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
                var user = await GetLoggedInUser();

                ExpenseCategoryModel recordModel = model;
                recordModel.IsActive = !model.IsActive;
                recordModel.UpdatedBy = user.Id;
                recordModel.UpdatedAt = DateTime.Now;

                await _expenseCategoryData.UpdateRecordStatus(recordModel);
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
                var user = await GetLoggedInUser();

                ExpenseCategoryModel recordModel = model;
                recordModel.IsArchived = true;
                recordModel.UpdatedBy = user.Id;
                recordModel.UpdatedAt = DateTime.Now;

                await _expenseCategoryData.ArchiveRecord(recordModel);
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

        public Task<List<ExpenseCategoryModel>> GetRecordsActive()
        {
            throw new NotImplementedException();
        }
    }
}
