namespace MainApp.StateServices;

public interface IStateContainer<T>
{
    T Value { get; set; }

    event Action? OnStateChange;

    void SetValue(T value);
}
