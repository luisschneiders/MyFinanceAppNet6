namespace MainApp.Components.Breadcrumb;

public class BreadcrumbLink : IBreadcrumbLink
{
    public string AppPageURL { get; set; }
    public string AppPageTitle { get; set; }
    public bool IsActive { get; set; }

    public BreadcrumbLink()
    {
        AppPageURL = string.Empty;
        AppPageTitle = string.Empty;
        IsActive = false;
    }
}
