using System.ComponentModel;

namespace MyFinanceAppLibrary.Enum;

public enum TransactionActionType
{
    [Description("Credit")]
    C,

    [Description("Debit")]
    D,

    [Description("Transfer")]
    T
}
