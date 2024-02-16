namespace MyFinanceAppLibrary.Constants;

public static class EndPoint
{
    public const string Home = "/";

    public const string Admin = "/admin";
    public const string AdminExpenses = $"{Admin}/expenses";
    public const string AdminTimesheet = $"{Admin}/timesheet";
    public const string AdminTransactions = $"{Admin}/transactions";
    public const string AdminTrips = $"{Admin}/trips";

    public const string Dashboard = "/dashboard";

    public const string Identity = "/Identity";
    public const string IdentityAccount = $"{Identity}/Account";
    public const string IdentityAccountManage = $"{IdentityAccount}/Manage-New";
    public const string IdentityAccountManageEmail = $"{IdentityAccountManage}/Email";
    public const string IdentityAccountManageChangePassword = $"{IdentityAccountManage}/ChangePassword";
    public const string IdentityAccountManageTwoFactorAuthentication = $"{IdentityAccountManage}/TwoFactorAuthentication";
    public const string IdentityAccountManagePersonalData = $"{IdentityAccountManage}/PersonalData";

    public const string Setup = "/setup";
    public const string SetupBank = $"{Setup}/bank";
    public const string SetupCompany = $"{Setup}/company";
    public const string SetupExpense = $"{Setup}/expense";
    public const string SetupTransaction = $"{Setup}/transaction";
    public const string SetupVehicle = $"{Setup}/vehicle";

    public const string Settings = "/settings";
    public const string SettingsAppearance = $"{Settings}/appearance";
    public const string Tools = $"/tools";
    public const string V2FinnhubNewsAll = "v2/Finnhub/GetAllNewsRecords";
    public const string V2GoogleGeocode = "v2/Google/GetGeocode";
    public const string V2GoogleMapStatic = "v2/Google/GetMapStatic";
    public const string V2RapidApiWeatherCondition = "v2/RapidApi/GetWeatherCondition";

}
