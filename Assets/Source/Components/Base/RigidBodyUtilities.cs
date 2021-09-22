using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Base
{
    /// <summary>
    /// Exposes neat functionalities for objects witha 2d Rigid body.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class RigidBodyUtilities : ComponentBase
    {
        private Rigidbody2D rigidBody;
        private GameObject player;

        public override void ComponentPreStart()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();

            // todo: might need to update this if we want this to work without a player existing.
            player = GetRequiredObject("Player");

            base.ComponentPreStart();
        }

        public void AddHorizontalImpact(float vel) => rigidBody.AddForce(new Vector2(vel, 0));
        
        public void AddVerticalImpact(float vel) => rigidBody.AddForce(new Vector2(0, vel));

        public void AddImpactAwayFromPlayer(float vel) {
            if (UnityUtils.Exists(player)) {
                if (player.transform.position.x > transform.position.x)
                {
                    rigidBody.AddForce(new Vector2(-vel, 0), ForceMode2D.Impulse);
                }
                else {
                    rigidBody.AddForce(new Vector2(vel, 0), ForceMode2D.Impulse);
                }
            }
        }

    }
}
