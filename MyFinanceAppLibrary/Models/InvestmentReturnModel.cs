namespace MyFinanceAppLibrary.Models;

public class InvestmentReturnModel
{
    public int IRType { get; set; }
    public decimal Period { get; set; }
    public decimal InitialValue { get; set; }
    public decimal FinalValue { get; set; }

    public decimal Percentage
    {
        get
        {
            if (IRType == (int)InvestmentReturn.Annualized)
            {
                return CalculateAnnualized(InitialValue, FinalValue, Period) * 100;
            }
            else if (IRType == (int)InvestmentReturn.Simple)
            {
                return CalculateSimple(InitialValue, FinalValue, Period) * 100;
            }
            else
            {
                return 0;
            }
        }
    }

    private static decimal CalculateSimple(decimal initial, decimal final, decimal period)
    {
        if (initial != 0 && final != 0)
        {
            return (final - initial) / initial;
        }
        else
        {
            return 0;
        }
    }

    private static decimal CalculateAnnualized(decimal initial, decimal final, decimal period)
    {
        if (initial != 0 && final != 0 && period != 0)
        {
            double cagr = Math.Pow((double)(final / initial), 1.0 / (double)period) - 1.0;

            return (decimal)cagr;
        }
        else
        {
            return 0;
        }
    }
}
