using AspNetCore.Authentication.ApiKey;
using System.Security.Claims;

namespace WebApi.Dependencies
{
    public static class ApiKeyDependencyInjection
    {
        public static IServiceCollection AddApiKeyAuthentication(
            this IServiceCollection services,
            string apiKeyHeader,
            string apiKeyRealm,
            string apiKeySecret)
        {
            services.AddSingleton(apiKeySecret);

            services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                .AddApiKeyInHeaderOrQueryParams<FabianFrancoApiKeyProvider>(options =>
                {
                    options.Realm = apiKeyRealm;
                    options.KeyName = apiKeyHeader;
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
        private readonly string _apiKey;

        public FabianFrancoApiKeyProvider(ILogger<FabianFrancoApiKeyProvider> logger, string apiKeySecret)
        {
            _logger = logger;
            _apiKey = apiKeySecret ?? throw new ArgumentNullException(nameof(apiKeySecret));
        }

        public Task<IApiKey> ProvideAsync(string key)
        {
            if (key.Equals(_apiKey))
            {
                _logger.LogDebug("Key valido");
                return Task.FromResult<IApiKey>(new FabianFrancoApiKey(key));
            }

            _logger.LogWarning("Key no valido");
            return Task.FromResult<IApiKey>(null);
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
            Claims = Array.Empty<Claim>();
        }
    }
}