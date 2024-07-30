namespace MainApp.Services;

public interface IBrowserService
{
    public Task CloseTab();
    public Task PrintWindow(string printPage);
}
