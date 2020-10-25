using Assets.Source.Components.Physics.Base;
using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerPhysicsComponent : PhysicsComponentBase
    {

        [SerializeField]
        private float moveSpeed = 6f;

        [SerializeField]
        [Tooltip("How high player can jump.  You might need to tweak the object's mass as well")]
        private float jumpHeight = 400f; 

        private bool jump = false;


        public override void ComponentUpdate()
        {
            if (Input.IsKeyPressed(InputConstants.K_JUMP)) 
            {
                AddForce(new Vector2(0, jumpHeight));
            }

            base.ComponentUpdate();
        }

        public override void ComponentFixedUpdate()
        {


            base.ComponentFixedUpdate();
        }

        public override float CalculateHorizontalMovement()
        {
            var moveRight = Input.GetAxisValue(InputConstants.K_MOVE_RIGHT);
            var moveLeft = Input.GetAxisValue(InputConstants.K_MOVE_LEFT);
            return (moveRight - moveLeft) * moveSpeed;
        }

        

    }
}
