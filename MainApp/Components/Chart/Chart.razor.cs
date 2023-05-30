using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MainApp.Components.Chart;
using MainApp.Settings.Enum;

namespace MainApp.Components.Chart;

public partial class Chart : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Parameter]
	public string Id { get; set; } = string.Empty;

    [Parameter]
	public ChartType Type { get; set; } = ChartType.bar;

    [Parameter]
	public ChartConfigData Data { get; set; } = new();

    [Parameter]
    public Position Position { get; set; } = Position.Top;

    [Parameter]
    public EventCallback<IJSObjectReference> OnSubmitSuccess { get; set; }

    private ChartConfig _chartConfig { get; set; } = new();

    public Chart()
	{
	}

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _chartConfig.Type = Type.ToString();
            _chartConfig.Data = Data;

            await _chartService.InvokeChartModule();
            await _chartService.SetupChartModule(Id, _chartConfig, Position);

            IJSObjectReference objectReference = await _chartService.GetChartObjectReference();

            await OnSubmitSuccess.InvokeAsync(objectReference);
        }

        await Task.CompletedTask;
    }
}
