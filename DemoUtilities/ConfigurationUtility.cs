using Microsoft.Extensions.Configuration;

namespace DemoUtilities
{
    public static class ConfigurationUtility
    {
        /// <summary>
        /// Get configuration key value.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns>Configuratoin key value as string.</returns>
        public static string GetConfigurationKeyValue(IConfiguration configuration, string key)
        {
            var configSection = configuration.GetSection("Brandpoint");
            if (configSection != null)
            {
                return configSection[key] ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
