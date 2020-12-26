using Assets.Source.Components.Timer;
using System.Collections;
using UnityEngine;

namespace Assets.Source.Components.MemoryManagement
{
    /// <summary>
    /// Simply kicks off a timer once the "BeginDespawn" method is called.
    /// </summary>
    public class DespawnComponent : ComponentBase
    {
        private IntervalTimerComponent timer;

        [SerializeField]
        [Tooltip("Despawn time in seconds")]
        private int time = 10;

        public void BeginDespawn() => StartCoroutine(nameof(WaitThenDespawn));
        
        private IEnumerator WaitThenDespawn()
        {
            yield return new WaitForSeconds(time);
            Despawn();
        }

        // todo:  Add some sort of particle effect to hide this cleverly
        private void Despawn() => Destroy(gameObject);
    }
}
