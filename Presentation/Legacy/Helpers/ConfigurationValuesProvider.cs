using System;
using System.Configuration;
using System.Globalization;

namespace Challenging_Challenges.Helpers
{
    public static class ConfigurationValuesProvider   
    {
        public static T Get<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception($"Could not find setting '{name}',");
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}