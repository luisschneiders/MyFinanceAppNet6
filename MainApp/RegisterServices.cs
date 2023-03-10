using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Components.Chart;
using MainApp.Components.OffCanvas;
using MainApp.Components.Dropdown;

namespace MainApp;

public static class RegisterServices
{
    public static void AddDefaultServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
    }

    public static void AddCachingServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
    }

    public static void AddMicrosoftServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

        // Get Azure AD B2C
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

        builder.Services.AddAuthorization(opts =>
        {
            opts.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("jobTitle", "Admin");
            });
        });
    }

    public static void AddSingletonServices(this WebApplicationBuilder builder)
    {
        //MongoDB
        builder.Services.AddSingleton<IDbConnection, MongoDbConnection>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();

        //Mysql
        builder.Services.AddSingleton<IDataAccess, MysqlDataAccess>();
        builder.Services.AddSingleton<IBankData<BankModel>, BankData>();
        builder.Services.AddSingleton<ICompanyData<CompanyModel>, CompanyData>();
        builder.Services.AddSingleton<IExpenseData<ExpenseModel>, ExpenseData>();
        builder.Services.AddSingleton<IExpenseCategoryData<ExpenseCategoryModel>, ExpenseCategoryData>();
        builder.Services.AddSingleton<ITimesheetData<TimesheetModel>, TimesheetData>();
        builder.Services.AddSingleton<ITransactionData<TransactionModel>, TransactionData>();
        builder.Services.AddSingleton<ITransactionCategoryData<TransactionCategoryModel>, TransactionCategoryData>();
        builder.Services.AddSingleton<ITripData<TripModel>, TripData>();
        builder.Services.AddSingleton<IVehicleData<VehicleModel>, VehicleData>();
    }

    public static void AddScopedServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SpinnerService>();
        builder.Services.AddScoped<ToastService>();
        builder.Services.AddScoped<IChartService, ChartService>();
        builder.Services.AddScoped<IDateTimeService, DateTimeService>();
        builder.Services.AddScoped<IDropdownDateRangeService, DropdownDateRangeService>();
        builder.Services.AddScoped<IOffCanvasService, OffCanvasService>();
        builder.Services.AddScoped<IBankService<BankModel>, BankService>();
        builder.Services.AddScoped<ICompanyService<CompanyModel>, CompanyService>();
        builder.Services.AddScoped<IExpenseService<ExpenseModel>, ExpenseService>();
        builder.Services.AddScoped<IExpenseCategoryService<ExpenseCategoryModel>, ExpenseCategoryService>();
        builder.Services.AddScoped<ITimesheetService<TimesheetModel>, TimesheetService>();
        builder.Services.AddScoped<ITransactionService<TransactionModel>, TransactionService>();
        builder.Services.AddScoped<ITransactionCategoryService<TransactionCategoryModel>, TransactionCategoryService>();
        builder.Services.AddScoped<ITripService<TripModel>, TripService>();
        builder.Services.AddScoped<IVehicleService<VehicleModel>, VehicleService>();

        //State Container
        builder.Services.AddScoped<TimesheetStateService>();
    }
}
