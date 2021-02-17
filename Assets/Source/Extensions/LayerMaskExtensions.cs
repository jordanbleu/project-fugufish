using UnityEngine;

namespace Assets.Source
{
    public static class LayerMaskExtensions
    {
        public static bool IncludesLayer(this LayerMask layerMask, int layer) => layerMask == (layerMask | (1 << layer));

        public static bool IsEverything(this LayerMask layermask)  => layermask == ~0;

        public static bool IsNothing(this LayerMask layerMask) => layerMask == ~-1;
                
    }
}
