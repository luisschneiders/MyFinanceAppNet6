using System.Timers;

namespace MainApp.Components.Toast;

public class ToastService : IToastService, IDisposable
{
    public event Action<string, Theme>? OnShow;
    public event Action? OnHide;
    private System.Timers.Timer? Countdown { get; set; }

    public ToastService()
    {
    }

    public void ShowToast(string message, Theme theme)
    {
        OnShow?.Invoke(message, theme);
        StartCountDown();
    }

    private void StartCountDown()
    {
        SetCountDown();
        if (Countdown is not null && Countdown.Enabled)
        {
            Countdown.Stop();
            Countdown.Start();
        }
        else
        {
            Countdown?.Start();
        }
    }

    private void SetCountDown()
    {
        if (Countdown == null)
        {
            Countdown = new System.Timers.Timer((int)Delay.Toast);
            Countdown.Elapsed += HideToast;
            Countdown.AutoReset = false;
        }
    }

    private void HideToast(object? source, ElapsedEventArgs args)
    {
        OnHide?.Invoke();
    }

    public void Dispose()
    {
        Countdown?.Dispose();
    }

}
