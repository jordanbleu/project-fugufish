using Assets.Source.Configuration.Base;

namespace Assets.Source.Configuration.Exception
{
    /// <summary>
    /// Throw this exception if something in the configuration is not valid. Exception will contain information
    /// about the configuration file that is wrong
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InvalidConfigurationException<T> : System.Exception where T : ConfigurationBase
    {
        public InvalidConfigurationException(string message) : base(FormatMessage(message)) { }

        public InvalidConfigurationException(string message, System.Exception ex) : base(FormatMessage(message), ex) { }

        private static string FormatMessage(string message)
        {
            return $"Configuration is not valid for configuration file '{typeof(T).Name}': {message}";
        }
    }
}
