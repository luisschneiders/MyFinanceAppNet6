namespace MainApp.Services;

public interface IFinnhubService
{
    Task<Response<List<FinnhubNewsModel>>> GetNewsAsync();
}
