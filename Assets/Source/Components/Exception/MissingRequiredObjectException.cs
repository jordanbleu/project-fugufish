namespace Assets.Source.Components
{
    public class MissingRequiredObjectException : System.Exception
    {
        public MissingRequiredObjectException(string gameObjectName, string name) : base(GenerateMessage(gameObjectName, name)) { }

        private static string GenerateMessage(string gameObjectName, string name)
        {
            return $"Game Object '{gameObjectName}' was unable to find expected object '{name}' in the hierarchy";
        }

        public MissingRequiredObjectException()
        {
        }

        public MissingRequiredObjectException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
