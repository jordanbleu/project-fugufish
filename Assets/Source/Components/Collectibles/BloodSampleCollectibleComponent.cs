using Assets.Source.Components.Collectibles.Base;
using Assets.Source.Components.UI;
using Assets.Source.Scene;

namespace Assets.Source.Components.Collectibles
{
    public class BloodSampleCollectibleComponent : CollectibleComponentBase
    {
        public override void ItemCollected()
        {
            GameDataTracker.CollectBloodSample();

            // find the hud and flash the collected total
            var uiObject = GetRequiredChild("BloodSampleDisplay", FindOrCreateCanvas());
            GetRequiredComponent<BloodSampleHudDisplayComponent>(uiObject).ShowHud();            

        }
    }
}
