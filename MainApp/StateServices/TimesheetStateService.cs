namespace MainApp.StateServices;

public class TimesheetStateService : IStateContainer<TimesheetModelStateContainerDTO>
{
    public TimesheetModelStateContainerDTO Value { get; set; } = default!;

    public event Action? OnStateChange;

    public void SetValue(TimesheetModelStateContainerDTO value)
    {
        Value = value;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChange?.Invoke();
}
