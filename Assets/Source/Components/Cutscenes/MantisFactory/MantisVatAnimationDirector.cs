using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Cutscenes.MantisFactory
{
    /// <summary>
    /// These methods get called by the parent animator.  This is basically a one-use class lol
    /// </summary>
    public class MantisVatAnimationDirector : ComponentBase
    {

        [SerializeField]
        private GameObject mantisVat;

        [SerializeField]
        private GameObject dropAnimation;

        public void ActivateDropAnimation()
        {
            mantisVat.SetActive(false);
            dropAnimation.SetActive(true);
        }

    }
}
