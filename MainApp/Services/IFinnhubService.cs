namespace MainApp.Services;

public interface IFinnhubService
{
    public Task<Response<List<FinnhubNewsModel>>> GetNewsAsync();
}
