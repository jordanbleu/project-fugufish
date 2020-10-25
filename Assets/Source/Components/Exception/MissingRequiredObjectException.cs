namespace Assets.Source.Components
{
    public class MissingRequiredObjectException : System.Exception
    {
        public MissingRequiredObjectException(string name) : base(GenerateMessage(name)) { }

        private static string GenerateMessage(string name)
        {
            return $"Unable to find expected object '{name}' in the hierarchy";
        }

        public MissingRequiredObjectException()
        {
        }

        public MissingRequiredObjectException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
