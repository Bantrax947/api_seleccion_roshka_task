using AspNetCore.Authentication.ApiKey;
using System.Security.Claims;

namespace WebApi.Dependencies
{
    public static class ApiKeyDependencyInjection
    {
        public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                .AddApiKeyInHeaderOrQueryParams<FabianFrancoApiKeyProvider>(options =>
                {
                    options.Realm = configuration.GetValue<string>("ApiKeyConfig:Realm");
                    options.KeyName = configuration.GetValue<string>("ApiKeyConfig:Header");
                    options.SuppressWWWAuthenticateHeader = false;
                });

            services.AddAuthorization();
            services.AddTransient<IApiKeyProvider, FabianFrancoApiKeyProvider>();

            return services;
        }
    }

    public class FabianFrancoApiKeyProvider : IApiKeyProvider
    {
        private readonly ILogger<FabianFrancoApiKeyProvider> _logger;
        public string ApiKey => _apiKey;
        private readonly string _apiKey;

        public FabianFrancoApiKeyProvider(ILogger<FabianFrancoApiKeyProvider> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiKey = configuration.GetValue<string>("ApiKeyConfig:Key")!;

            if (_apiKey is null)
            {
                throw new ArgumentException("No se encuentra el ApiKey en la configuracion.");
            }
        }

        public Task<IApiKey> ProvideAsync(string key)
        {
            try
            {
                if (key.Equals(_apiKey))
                {
                    _logger.LogDebug("Key valido");
                    return Task.FromResult<IApiKey>(new FabianFrancoApiKey(key));
                }

                _logger.LogWarning("Key no valido");

                return Task.FromResult<IApiKey>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mensaje: {Mensaje}", ex.Message);
                throw new ArgumentException("Error al verificar apiKey");
            }
        }
    }

    public class FabianFrancoApiKey : IApiKey
    {
        public string Key { get; }
        public string OwnerName { get; } = "Fabian Franco";
        public IReadOnlyCollection<Claim> Claims { get; }

        public FabianFrancoApiKey(string key)
        {
            Key = key;
        }
    }
}
