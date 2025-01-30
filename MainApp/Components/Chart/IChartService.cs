using Microsoft.JSInterop;

namespace MainApp.Components.Chart;

public interface IChartService
{
    Task<IJSObjectReference> InvokeModule();
    Task<IJSObjectReference> SetupModule(string id, ChartConfig chartConfig, Position position);
    Task<IJSObjectReference> GetObjectReference();
    Task UpdateData(IJSObjectReference chartObjectReference, ChartConfigData chartData);
    Task RemoveData(IJSObjectReference chartObjectReference);
}
