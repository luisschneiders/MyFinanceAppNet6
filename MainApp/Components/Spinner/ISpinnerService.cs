namespace MainApp.Components.Spinner;

public interface ISpinnerService
{
    public event Action OnShow;
    public event Action OnHide;

    public void ShowSpinner();
    public void HideSpinner();
}
