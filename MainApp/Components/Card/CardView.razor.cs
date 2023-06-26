using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Card;

public partial class CardView : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Link { get; set; }

    [Parameter]
    public string Styles { get; set; }

    private string _radius { get; set; } = Radius.Default;

    public CardView()
    {
        Link = string.Empty;
        Styles = string.Empty;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _radius = await _appSettingsService.GetCardShape();
            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }
}
