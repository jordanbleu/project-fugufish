using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Cutscenes.Intro
{
    /// <summary>
    /// To further detract from this already bad code, this class exists solely so that the actor animators can hook into
    /// the cutscene director
    /// </summary>
    public class IntroCutsceneHookComponent : ComponentBase
    {

        [SerializeField]
        private IntroCutsceneDirectorComponent cutsceneDirector;

        public void IncrementStage() => cutsceneDirector.IncrementStage();


    }
}
