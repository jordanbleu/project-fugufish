using UnityEngine;

namespace Assets.Source.Components.Dummy
{
    public class DummyComponent : ComponentBase
    {

        public void SayHello() {
            Debug.Log("Hello, the thing works");
        }

        public void SayGoodbye() {
            Debug.Log("Goodbye, that thing works");
        }

        public void Say(string message) {
            Debug.Log(message);
        }

        public void KillMySelf(string message) {
            Debug.Log(message);
            Destroy(gameObject);
        }
    }
}
