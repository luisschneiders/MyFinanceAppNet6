namespace MainApp.Services;

public interface IAppSettingsService
{
    public Task SetShapes(string radius);
    public Task<string> GetShapes();
    public Task<string> GetButtonShape();
}
