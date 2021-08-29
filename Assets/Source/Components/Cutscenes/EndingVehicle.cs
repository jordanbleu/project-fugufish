using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Cutscenes
{
    public class EndingVehicle : ComponentBase
    {

        [SerializeField]
        private GameObject player;


        [SerializeField]
        private GameObject credits;

        public void DisablePlayer() => player.SetActive(false);

        public void EnableCredits()
        {
            credits.SetActive(true);
        }

    }


}
