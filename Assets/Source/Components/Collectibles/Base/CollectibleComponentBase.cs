using UnityEngine;

namespace Assets.Source.Components.Collectibles.Base
{
    [RequireComponent(typeof(Collider2D), typeof(Animator))]
    public abstract class CollectibleComponentBase : ComponentBase
    {
        private const float MAGNET_SPEED = 0.05f;

        [SerializeField]
        [Tooltip("The collectible will move towards this object when collected")]
        protected GameObject playerObject;

        private Animator animator;
        private bool wasCollected = false;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            wasCollected = true;
            animator.SetTrigger("collected");            
        }

        public override void ComponentUpdate()
        {
            if (wasCollected) {
                
                // a very simplistic magnetization type animation where the item pulls towards the players position
                var xDiff = transform.position.x - playerObject.transform.position.x;
                var yDiff = transform.position.y - playerObject.transform.position.y;
                transform.Translate(new Vector2(MAGNET_SPEED * -Mathf.Sign(xDiff), MAGNET_SPEED * -Mathf.Sign(yDiff)));

            }
            base.ComponentUpdate();
        }

        // Triggered from 
        public void OnCollectAnimationCompleted() {
            ItemCollected();
            Destroy(gameObject);
        }

        public abstract void ItemCollected();


    }
}
