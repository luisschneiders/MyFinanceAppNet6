using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class InvestmentReturnCalculator : ComponentBase
{

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private InvestmentReturnModel _investmentReturnModel { get; set; } = new();

    private InvestmentReturn[] _investmentReturnTypes { get; set; } = default!;
    public InvestmentReturnCalculator()
    {
        _investmentReturnTypes = (InvestmentReturn[])Enum.GetValues(typeof(InvestmentReturn));
    }

}
