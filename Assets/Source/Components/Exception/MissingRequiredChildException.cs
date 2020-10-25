using UnityEngine;

namespace Assets.Source.Components
{
    public class MissingRequiredChildException : System.Exception
    {
        public MissingRequiredChildException(GameObject gameObject, string childName) : base(GenerateMessage(gameObject, childName)) { }

        private static string GenerateMessage(GameObject gameObject, string childName)
        {
            string gameObjectName = gameObject?.name ?? "[null]";
            string childObjectName = childName ?? "[null]";

            return $"Game Object '{gameObjectName}' does not contain a required child '{childObjectName}' " +
                $"as a direct child object.  It may be buried in the hierarchy (or it just doesn't exist).";
        }

        public MissingRequiredChildException()
        {
        }

        public MissingRequiredChildException(string message) : base(message)
        {
        }

        public MissingRequiredChildException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
