using System.Text;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Breadcrumb;

	public partial class Breadcrumb : ComponentBase
	{

    [Parameter]
    public bool IsVisible { get; set; } = true;

    [Parameter]
    public string AppPageLink { get; set; }

    private string[] _appPageLink { get; set; } = default!;

    private BreadcrumbLink _breadcrumbLink { get; set; }
    private List<BreadcrumbLink> _breadcrumbLinks { get; set; }

    public Breadcrumb()
	{
        AppPageLink = string.Empty;
        _breadcrumbLink = new();
        _breadcrumbLinks = new();
    }

    protected override void OnInitialized()
    {
        _appPageLink = AppPageLink.Split("/");

        BuildBreadcrumbLinks();

    }

    private void BuildBreadcrumbLinks()
    {
        int lastIndex = _appPageLink.Length - 1;

        StringBuilder sb = new StringBuilder();

        foreach (var appPageLink in _appPageLink.Select((x, i) => new { Value = x, Index = i }))
        {
            _breadcrumbLink = new();

            _breadcrumbLink.AppPageTitle = _appPageLink[appPageLink.Index];

            if ((appPageLink.Index + 1) == 1)
            {

                sb.Append(appPageLink.Value);
                _breadcrumbLink.IsActive = true;

            } else if ((appPageLink.Index + 1) > 1)
            {
                if (appPageLink.Index == lastIndex)
                {

                    _breadcrumbLink.IsActive = false;

                }
                else
                {

                    sb.Append("/" + appPageLink.Value);
                    _breadcrumbLink.IsActive = true;

                }
            }

            _breadcrumbLink.AppPageURL = sb.ToString();

            _breadcrumbLinks.Add(_breadcrumbLink);

        }
    }
}
