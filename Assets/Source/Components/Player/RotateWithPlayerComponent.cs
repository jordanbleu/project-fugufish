using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class RotateWithPlayerComponent : ComponentBase
    {

        [SerializeField]
        private GameObject playerObject; 

        private PlayerComponent playerComponent; 
        public override void ComponentAwake()
        {
            playerComponent = GetRequiredComponentInParent<PlayerComponent>(playerObject);
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (playerComponent.DirectionFacing == Directions.Up)
            {
                transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, 0, transform.localRotation.w);
            }
            else 
            {
                transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, 180, transform.localRotation.w);
            }
            base.ComponentUpdate();
        }


    }
}
