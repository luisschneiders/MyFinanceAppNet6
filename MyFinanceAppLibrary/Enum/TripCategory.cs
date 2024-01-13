using System.ComponentModel;

namespace MyFinanceAppLibrary.Enum;

public enum TripCategory
{
    [Description("Not Specified")]
    NotSpecified = 0,

    [Description("Courier")]
    Courier = 1,

    [Description("Food Delivery")]
    FoodDelivery = 2,

    [Description("Other ABN Work")]
    OtherABNWork = 3,
    
    [Description("Private")]
    Private = 4,

    [Description("Rideshare")]
    Rideshare = 5,

    [Description("TFN Employer")]
    TFNEmployer = 6,
}
