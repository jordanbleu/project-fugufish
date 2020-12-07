using Assets.Source.Components.Brain.Base;
using UnityEngine;

namespace Assets.Source.Components.Brain.Behavior
{
    public class WaitInPositionBehavior : AIBehaviorBase
    {
        [SerializeField]
        [Tooltip("Drag the actor here (where the state machine and AnimaotrController and stuff is).")]
        private GameObject actor;

        [SerializeField]
        [Tooltip("If true, NPC will wait in the exact specified position.  Otherwise they'll just stay whereever they were last.")]
        private bool waitInExactPosition = true;

        [SerializeField]
        [Tooltip("The position to wait idly in.  This is ignored if 'waitInExactPosition' is unchecked.")]
        private float waitPosition;

        [SerializeField]
        [Tooltip("How close the actor needs to get to the position.  Smaller number means closer but can cause glitches.")]
        private float precision = 0.5f;

        [SerializeField]
        private float moveSpeed = 3f;
        
        private Animator animator;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>(actor);
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            
            base.ComponentUpdate();
        }

        public override Vector2 HorizontalMovment() {
            if (waitInExactPosition)
            {
                if (!transform.position.x.IsWithin(precision, waitPosition)) {
                    if (waitPosition > transform.position.x)
                    {
                        return new Vector2(moveSpeed, Brain.CurrentVelocity.y);
                    }
                    else {
                        return new Vector2(-moveSpeed, Brain.CurrentVelocity.y);
                    }                
                }
            }
            return new Vector2(0f, Brain.CurrentVelocity.y);
        }

        private void OnDrawGizmosSelected()
        {
            if (waitInExactPosition)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, new Vector3(waitPosition, transform.position.y));
            }            
        }
    }
}
