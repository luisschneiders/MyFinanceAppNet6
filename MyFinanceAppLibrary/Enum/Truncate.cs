namespace MyFinanceAppLibrary.Enum;

public enum Truncate
{
    FirstName = 8,
    LastName = 12,
    Bank = 15,
    Company = Bank,
    ColumnTimesheet = Bank,
    ShortMonthName = 3,
    ExpenseCategory = 20,
    TransactionCategory = ExpenseCategory
}
