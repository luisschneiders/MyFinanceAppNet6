using System;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Message;

public partial class MessageView : ComponentBase, IMessageView
{
    [Parameter]
    public string Icon { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Message { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public MessageView()
    {
        Icon = string.Empty;
        Title = string.Empty;
        Message = string.Empty;
    }
}
