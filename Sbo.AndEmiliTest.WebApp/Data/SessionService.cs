using Blazored.LocalStorage;

namespace Sbo.AndEmiliTest.WebApp.Data;

public class SessionService
{
    private readonly ILocalStorageService localStorageService;
    private const string UserKey = "User";

    public SessionService(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    public async Task<string?> GetLoggedUser()
    {
        try
        {
            var user = await localStorageService.GetItemAsync<string?>(UserKey);
            return user;
        }
        catch
        {
            return null;
        }
    }

    public async Task Login(string user)
    {
        await localStorageService.SetItemAsync(UserKey, user);
    }

    public async Task Logout()
    {
        await localStorageService.SetItemAsync<string?>(UserKey, null);
    }
}