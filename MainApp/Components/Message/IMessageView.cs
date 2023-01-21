using System;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Message;

public interface IMessageView
{
    public string Icon { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }

    #nullable enable    
    public RenderFragment? ChildContent { get; set;  }
}
