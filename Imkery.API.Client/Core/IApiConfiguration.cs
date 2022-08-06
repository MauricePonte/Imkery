namespace Imkery.API.Client.Core
{
    public interface IApiConfiguration
    {
        string GetAPIEndPoint();
        string GetAuthenticatedHttpClientName();
    }
}