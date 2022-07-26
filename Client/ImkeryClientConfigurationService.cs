using Imkery.API.Client;

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

    public string GetTokenEndpoint()
    {
        return _configuration["TokenEndPoint"];
    }
}