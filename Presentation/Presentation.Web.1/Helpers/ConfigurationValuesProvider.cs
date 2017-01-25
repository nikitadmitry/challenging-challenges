using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Presentation.Web.Helpers
{
    public static class ConfigurationValuesProvider
    {
        private static IConfiguration configuration;

        public static void SetConfiguration(IConfiguration configurationProvider)
        {
            configuration = configurationProvider;
        }

        public static T Get<T>(string name)
        {
            string value = configuration[name];
            
            if (value == null)
            {
                throw new Exception($"Could not find setting '{name}',");
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}