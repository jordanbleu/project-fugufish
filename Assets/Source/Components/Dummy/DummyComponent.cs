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
    }
}
