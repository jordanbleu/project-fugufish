using Assets.Editor.Attributes;
using Assets.Source.Components.Collectibles.Base;
using Assets.Source.Components.UI;
using Assets.Source.Scene;
using System;
using UnityEngine;

namespace Assets.Source.Components.Collectibles
{
    public class BloodSampleCollectibleComponent : CollectibleComponentBase
    {
        [Tooltip("This needs to be unique per instance because of hacks")]
        [SerializeField]
        private string identifier = Guid.NewGuid().ToString();

        public override void ComponentAwake()
        {
            if (GameDataTracker.CollectedBloodSamples.Contains(identifier)) {
                Destroy(gameObject);
            }
            base.ComponentAwake();
        }

        public override void ItemCollected()
        {
            GameDataTracker.CollectedBloodSamples.Add(identifier);

            GameDataTracker.CollectBloodSample();

            // find the hud and flash the collected totala
            var uiObject = GetRequiredChild("BloodSampleDisplay", FindOrCreateCanvas());
            GetRequiredComponent<BloodSampleHudDisplayComponent>(uiObject).ShowHud();            

        }
    }
}
