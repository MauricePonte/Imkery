using Imkery.API.Client;

internal class ImkeryTokenStorage : ITokenStorage
{
    public Task<string> GetToken()
    {
        return Task.FromResult("");
    }

    public Task StoreToken(string token)
    {
        return null;
    }
}