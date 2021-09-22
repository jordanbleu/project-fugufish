using Assets.Source.Components.Timer;
using UnityEngine;

namespace Assets.Source.Components.Collectibles.Base
{
    [RequireComponent(typeof(Collider2D), typeof(Animator), typeof(AudioSource))]
    public abstract class CollectibleComponentBase : ComponentBase
    {
        private const float MAGNET_SPEED = 0.05f;

        protected GameObject playerObject;

        private Animator animator;
        private bool wasCollected = false;
        private bool isReady = false;

        private AudioSource audioSource;

        [SerializeField]
        private AudioClip pickupSound;

        public override void ComponentPreStart()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            playerObject = GetRequiredObject("Player");
            animator = GetRequiredComponent<Animator>();
            base.ComponentPreStart();
        }

        public void CooldownEnded() => isReady = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isReady && collision.gameObject.Equals(playerObject))
            {
                audioSource.PlayOneShot(pickupSound);
                wasCollected = true;
                animator.SetTrigger("collected");
            }
        }

        public override void ComponentUpdate()
        {
            if (wasCollected) {
                
                // a very simplistic magnetization type animation where the item pulls towards the players position
                var xDiff = transform.position.x - playerObject.transform.position.x;
                var yDiff = transform.position.y - playerObject.transform.position.y;

                if (xDiff.IsWithin(MAGNET_SPEED, playerObject.transform.position.x)) {
                    xDiff = 0;
                }

                if (yDiff.IsWithin(MAGNET_SPEED, playerObject.transform.position.y)) {
                    yDiff = 0;
                }

                transform.Translate(new Vector2(MAGNET_SPEED * -Mathf.Sign(xDiff), MAGNET_SPEED * -Mathf.Sign(yDiff)));

            }
            base.ComponentUpdate();
        }

        // Triggered from animator
        public void OnCollectAnimationCompleted() {
            ItemCollected();
            Destroy(gameObject);
        }

        public abstract void ItemCollected();


    }
}
