namespace MyFinanceAppLibrary.Models;

public class LVRModel
{
    public decimal LoanAmount { get; set; }
    public decimal PropertyValue { get; set; }
    public decimal Percentage 
    {
        get 
        {
            if (LoanAmount is not 0 & PropertyValue is not 0)
            {
                return (LoanAmount / PropertyValue)  * 100;
            }
            else
            {
                return 0;
            }
        }
    }
}
