using System;
using MainApp.Components.Toast;
using MainApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MongoDB.Driver.Core.Connections;

namespace MainApp.Components.OffCanvas;

public partial class OffCanvasMenu : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    private IJSObjectReference _objectReference { get; set; } = default!;

    public OffCanvasMenu()
	{
    }
}
