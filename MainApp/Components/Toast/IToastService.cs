namespace MainApp.Components.Toast;

public interface IToastService
{
    public event Action<string, Theme> OnShow;
    public event Action OnHide;
    public void ShowToast(string message, Theme theme);
}
