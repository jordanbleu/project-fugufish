namespace Assets.Source.Components
{
    public class MissingResourceException : System.Exception
    {
        public MissingResourceException(string path) : base(GenerateMessage(path)) { }

        private static string GenerateMessage(string path)
        {
            return $"Missing resource for path: '{path}'.";
        }

        public MissingResourceException()
        {
        }

        public MissingResourceException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
