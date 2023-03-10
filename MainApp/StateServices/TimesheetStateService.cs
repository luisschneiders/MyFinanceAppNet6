namespace MainApp.StateServices;

public class TimesheetStateService : IStateContainer<TimesheetStateContainerDTO>
{
    public TimesheetStateContainerDTO Value { get; set; } = default!;

    public event Action? OnStateChange;

    public void SetValue(TimesheetStateContainerDTO value)
    {
        Value = value;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChange?.Invoke();
}
