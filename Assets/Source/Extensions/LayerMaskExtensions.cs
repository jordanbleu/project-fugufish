using UnityEngine;

namespace Assets.Source
{
    public static class LayerMaskExtensions
    {
        public static bool IncludesLayer(this LayerMask layerMask, int layer) => layerMask == (layerMask | (1 << layer));
        
    }
}
