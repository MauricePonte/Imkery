namespace Imkery.API.Client
{
    public interface IApiConfiguration
    {
        string GetTokenEndpoint();
        string GetAPIEndPoint();
    }
}