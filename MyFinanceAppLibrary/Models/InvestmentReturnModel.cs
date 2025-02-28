namespace MyFinanceAppLibrary.Models;

public class InvestmentReturnModel
{
    public int IRType { get; set; }
    public decimal Period { get; set; }
    public decimal PersonalContribution { get; set; }
    public decimal FinalAmount { get; set; }

    public decimal Percentage
    {
        get
        {
            if (IRType == (int)InvestmentReturn.Annualized)
            {
                return CalculateAnnualized(PersonalContribution, FinalAmount, Period) * 100;
            }
            else if (IRType == (int)InvestmentReturn.Simple)
            {
                return CalculateSimple(PersonalContribution, FinalAmount) * 100;
            }
            else
            {
                return 0;
            }
        }
    }

    private static decimal CalculateSimple(decimal personalContribution, decimal finalAmount)
    {
        if (personalContribution != 0 && finalAmount != 0)
        {
            return (finalAmount - personalContribution) / personalContribution;
        }
        else
        {
            return 0;
        }
    }

    private static decimal CalculateAnnualized(decimal personalContribution, decimal finalAmount, decimal period)
    {
        if (personalContribution != 0 && finalAmount != 0 && period != 0)
        {
            double cagr = Math.Pow((double)(finalAmount / personalContribution), 1.0 / (double)period) - 1.0;

            return (decimal)cagr;
        }
        else
        {
            return 0;
        }
    }
}
