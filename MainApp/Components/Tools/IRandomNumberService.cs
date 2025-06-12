namespace MainApp.Components.Tools;

public interface IRandomNumberService
{
    public Task<HashSet<int>> GetLocalStorageRandomNumber();
    public Task SetLocalStorageRandomNumber(HashSet<int> numbers);
}
