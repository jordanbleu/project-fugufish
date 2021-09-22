using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Animation
{
    public class RandomizeSpriteColorOnAwakeComponent : ComponentBase
    {
        private SpriteRenderer sprite;
        private Animator animator;

        public override void ComponentPreStart()
        {
            sprite = GetRequiredComponent<SpriteRenderer>();
            animator = GetRequiredComponent<Animator>();
            base.ComponentPreStart();
        }

        public override void ComponentStart()
        {
            sprite.color = new Color(UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), 1f);
            base.ComponentStart();
        }

    }
}
