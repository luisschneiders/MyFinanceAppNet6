using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Components.Chart;
using MainApp.Components.OffCanvas;

namespace MainApp;

public static class RegisterServices
{
    public static void AddDefaultServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddHttpClient<EssentialsAPIService>("essentials-api", opts =>
        {
            opts.BaseAddress = new Uri(builder.Configuration.GetValue<string>("EssentialsApiUrl"));
        });

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
        builder.Services.AddSingleton<IServiceScopeFactory<IDateTimeService>, ServiceScopeFactory<IDateTimeService>>();

        //MongoDB
        builder.Services.AddSingleton<IDbConnection, MongoDbConnection>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();

        //Mysql
        builder.Services.AddSingleton<IDataAccess, MysqlDataAccess>();
        builder.Services.AddSingleton<IBankData<BankModel>, BankData>();
        builder.Services.AddSingleton<ICompanyData<CompanyModel>, CompanyData>();
        builder.Services.AddSingleton<IExpenseData<ExpenseModel>, ExpenseData>();
        builder.Services.AddSingleton<IExpenseCategoryData<ExpenseCategoryModel>, ExpenseCategoryData>();
        builder.Services.AddSingleton<ILocationData<UserLocationModel>, LocationData>();
        builder.Services.AddSingleton<ILocationExpenseData<LocationExpenseModel>, LocationExpenseData>();
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
        builder.Services.AddScoped<ICalendarViewService, CalendarViewService>();
        builder.Services.AddScoped<IChartService, ChartService>();
        builder.Services.AddScoped<IChartBankService, ChartBankService>();
        builder.Services.AddScoped<IChartExpenseService, ChartExpenseService>();
        builder.Services.AddScoped<IChartTransactionService, ChartTransactionService>();
        builder.Services.AddScoped<IDateTimeService, DateTimeService>();
        builder.Services.AddScoped<IDropdownDateRangeService, DropdownDateRangeService>();
        builder.Services.AddScoped<IDropdownDateMonthYearService, DropdownDateMonthYearService>();
        builder.Services.AddScoped<IDropdownFilterService, DropdownFilterService>();
        builder.Services.AddScoped<IOffCanvasService, OffCanvasService>();
        builder.Services.AddScoped<IBankService<BankModel>, BankService>();
        builder.Services.AddScoped<ICompanyService<CompanyModel>, CompanyService>();
        builder.Services.AddScoped<IExpenseService<ExpenseModel>, ExpenseService>();
        builder.Services.AddScoped<IExpenseCategoryService<ExpenseCategoryModel>, ExpenseCategoryService>();
        builder.Services.AddScoped<IFinnhubService, FinnhubService>();
        builder.Services.AddScoped<IGoogleService, GoogleService>();
        builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
        builder.Services.AddScoped<ILocationService<UserLocationModel>, LocationService>();
        builder.Services.AddScoped<ILocationExpenseService<LocationExpenseModel>, LocationExpenseService>();
        builder.Services.AddScoped<IRapidApiService, RapidApiService>();
        builder.Services.AddScoped<ITimesheetService<TimesheetModel>, TimesheetService>();
        builder.Services.AddScoped<ITransactionService<TransactionModel>, TransactionService>();
        builder.Services.AddScoped<ITransactionCategoryService<TransactionCategoryModel>, TransactionCategoryService>();
        builder.Services.AddScoped<ITripService<TripModel>, TripService>();
        builder.Services.AddScoped<IVehicleService<VehicleModel>, VehicleService>();
        builder.Services.AddScoped<IWebApiService, WebApiService>();

        //State Container
        builder.Services.AddScoped<TimesheetStateService>();
    }

    public static void AddHostedServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<BackgroundWeatherService>();
    }
}
