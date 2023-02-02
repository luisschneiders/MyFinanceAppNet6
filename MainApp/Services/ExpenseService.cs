using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;

namespace MainApp.Services
{
    public class ExpenseService : IExpenseService
    {
        [Inject]
        private IExpenseData<ExpenseModel> _expenseData { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider _authProvider { get; set; } = default!;

        [Inject]
        private IUserData _userData { get; set; } = default!;

        private UserModel _loggedInUser { get; set; } = new();

        public ExpenseService(IExpenseData<ExpenseModel> expenseData, IUserData userData, AuthenticationStateProvider authProvider)
        {
            _expenseData = expenseData;
            _userData = userData;
            _authProvider = authProvider;
        }

        // TODO: Add pagination capabilities
        public async Task<List<ExpenseModel>> GetExpenses()
        {
            try
            {
                var user = await GetLoggedInUser();
                List<ExpenseModel> results = await _expenseData.GetRecords(user.Id);
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                throw;
            }
        }

        public async Task<List<ExpenseModel>> GetSearchResults(string search)
        {
            try
            {
                var user = await GetLoggedInUser();
                List<ExpenseModel> results = await _expenseData.GetSearchResults(user.Id, search);
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                throw;
            }
        }

        public async Task<ExpenseModel> GetExpenseById(string modelId)
        {
            try
            {
                var user = await GetLoggedInUser();
                ExpenseModel result = await _expenseData.GetRecordById(user.Id, modelId);
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
            var lastInsertedId = await _expenseData.GetLastInsertedId();
            return await Task.FromResult(lastInsertedId);
        }

        public async Task CreateExpense(ExpenseModel model)
        {
            try
            {
                var user = await GetLoggedInUser();
                ExpenseModel newExpense = new()
                {
                    Description = model.Description,
                    UpdatedBy = user.Id
                };

                await _expenseData.CreateRecord(newExpense);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                throw;
            }
        }

        public async Task UpdateExpense(ExpenseModel model)
        {
            // TODO: check if record is not archived in Mysql Stored Procedure
            try
            {
                var user = await GetLoggedInUser();
                ExpenseModel newExpense = new()
                {
                    Id = model.Id,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    UpdatedBy = user.Id,
                    UpdatedAt = DateTime.Now,
                };

                await _expenseData.UpdateRecord(newExpense);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                throw;
            }
        }

        public async Task UpdateExpenseStatus(ExpenseModel model)
        {
            // TODO: check if record is not archived in Mysql Stored Procedure
            try
            {
                var user = await GetLoggedInUser();

                ExpenseModel expenseStatusUpdate = model;
                expenseStatusUpdate.IsActive = !model.IsActive;
                expenseStatusUpdate.UpdatedBy = user.Id;
                expenseStatusUpdate.UpdatedAt = DateTime.Now;

                await _expenseData.UpdateRecordStatus(expenseStatusUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                throw;
            }
        }

        public async Task ArchiveExpense(ExpenseModel model)
        {
            try
            {
                var user = await GetLoggedInUser();

                ExpenseModel expenseStatusUpdate = model;
                expenseStatusUpdate.IsArchived = true;
                expenseStatusUpdate.UpdatedBy = user.Id;
                expenseStatusUpdate.UpdatedAt = DateTime.Now;

                await _expenseData.ArchiveRecord(expenseStatusUpdate);
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
    }
}
