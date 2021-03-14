﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Objects.Switches
{
    /// <summary>
    /// This is a basic switch that will display one sprite when open, one sprite when closed
    /// </summary>
    public class SimpleTwoSpriteSwitch : ComponentBase
    {

        [SerializeField]
        private bool isOn;

        [SerializeField]
        private SpriteRenderer onSprite;

        [SerializeField]
        private SpriteRenderer offSprite;

        [SerializeField]
        [Tooltip("Fired when the switch state is updated")]
        private UnityEvent onSwitchStateUpdated = new UnityEvent();

        [SerializeField]
        [Tooltip("If true, the switch can only be used once (no toggling back and forth)")]
        private bool isOneWay;

        private bool wasToggled = false;

        public override void ComponentUpdate()
        {
            if (isOn)
            {
                onSprite.gameObject.SetActive(true);
                offSprite.gameObject.SetActive(false);
            }
            else {
                onSprite.gameObject.SetActive(false);
                offSprite.gameObject.SetActive(true);
            }

            base.ComponentUpdate();
        }

        public void ToggleSwitch() {
            if (!isOneWay || !wasToggled) { 
                isOn = !isOn;
                wasToggled = true;
                onSwitchStateUpdated?.Invoke();
            }
        }

    }
}
