using Imkery.API.Client.Core;

internal class ImkeryClientConfigurationService : IApiConfiguration
{
    private readonly IConfiguration _configuration;

    public ImkeryClientConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetAPIEndPoint()
    {
        return _configuration["APIEndPoint"];

    }

    public string GetAuthenticatedHttpClientName()
    {
        return _configuration["AuthenticatedHttpClient"];
    }
    public string GetDefaultHttpClientName()
    {
        return _configuration["DefaultHttpClient"];
    }
    
}