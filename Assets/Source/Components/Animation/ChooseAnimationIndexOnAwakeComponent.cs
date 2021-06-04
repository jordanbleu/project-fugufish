using Assets.Editor.Attributes;
using UnityEngine;

namespace Assets.Source.Components.Animation
{
    /// <summary>
    /// Very basic component that chooses a random animation index on startup.
    /// <para>
    /// This component requires an animator controller with a single integer variable called "animation_index" that 
    /// will control which animation to display.
    /// </para>
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ChooseAnimationIndexOnAwakeComponent : ComponentBase
    {
        [SerializeField]
        [Tooltip("Max index.  If isRandom is true, value will be chosen at random from zero to this number.  if false, this number will be chosen.")]
        private int maxIndex;

        [SerializeField]
        [ReadOnly]
        private int chosenIndex = 0;


        [SerializeField]
        [Tooltip("If true, maxIndex is treated like a range from 0 to maxIndex, and the animation is chosen at random.  If false, the maxIndex will be chosen.")]
        private bool isRandom = true;

        public override void ComponentAwake()
        {
            if (isRandom)
            {
                chosenIndex = UnityEngine.Random.Range(0, maxIndex);
            }
            else {
                chosenIndex = maxIndex;
            }
            
            var animator = GetRequiredComponent<Animator>();
            animator.SetInteger("animation_index", chosenIndex);
            base.ComponentAwake();
        }



    }
}
