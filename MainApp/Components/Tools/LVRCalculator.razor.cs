using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class LVRCalculator : ComponentBase
{
    private LVRModel _lvrModel { get; set; } = new();

    public LVRCalculator()
    {
    }
}
