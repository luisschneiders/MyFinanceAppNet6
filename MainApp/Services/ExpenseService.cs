using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;

namespace MainApp.Services
{
    public class ExpenseService : IExpenseService
    {
        [Inject]
        IExpenseData _expenseData { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider _authProvider { get; set; } = default!;

        [Inject]
        private IUserData _userData { get; set; } = default!;

        private UserModel _loggedInUser { get; set; } = new();

        public ExpenseService(IExpenseData expenseData, IUserData userData, AuthenticationStateProvider authProvider)
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
                List<ExpenseModel> results = await _expenseData.GetExpenses(user.Id);
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
            return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
        }
    }
}
