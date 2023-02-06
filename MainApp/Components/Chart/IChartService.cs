using Microsoft.JSInterop;

namespace MainApp.Components.Chart;

public interface IChartService
{
    Task<IJSObjectReference> InvokeChartModule();
    Task<IJSObjectReference> SetupChartModule(string id, ChartConfig chartConfig);
    Task<IJSObjectReference> GetChartObjectReference();
    Task UpdateChartData(IJSObjectReference chartObjectReference, List<string> chartData);
    Task RemoveChartData(IJSObjectReference chartObjectReference);
}
