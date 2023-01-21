namespace MainApp.Components.Breadcrumb;

public interface IBreadcrumbLink
{
    public string AppPageURL { get; set; }
    public string AppPageTitle { get; set; }
    public bool IsActive { get; set; }
}
