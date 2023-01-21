using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Footer;

public partial class FooterViewNotAuthorized : ComponentBase
{
    private DateTime _currentYear { get; }

    public FooterViewNotAuthorized()
	{
        _currentYear = DateTime.Now;
    }
}
