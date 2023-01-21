using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Footer;

public partial class FooterViewAuthorized : ComponentBase
{
	private DateTime _currentYear { get; }

	public FooterViewAuthorized()
	{
		_currentYear = DateTime.Now;
	}
}
