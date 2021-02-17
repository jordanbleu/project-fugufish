using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Util
{
    /// <summary>
    /// Contains more custom physics 2d functions besides the normal unity physics2d object
    /// </summary>
    public static class MorePhysics2D
    {
        /// <summary>
        /// Performs a normal raycast but filters out the object's own collider (and children)
        /// </summary>
        /// <returns>The first intercepting object that is not the gameobject specified</returns>
        public static RaycastHit2D RaycastOther(GameObject me, Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth)
        {
            var raycastHits = Physics2D.RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);
            return raycastHits.FirstOrDefault(hit => hit.transform != me.transform);
        }

        /// <summary>
        /// Performs a normal raycast but filters out the object's own collider (and children)
        /// </summary>
        /// <returns>The first intercepting object that is not the gameobject specified</returns>
        public static RaycastHit2D RaycastOther(GameObject me, Ray ray, float distance, int layerMask, float minDepth, float maxDepth) =>
            RaycastOther(me, ray.origin, ray.direction, distance, layerMask, minDepth, maxDepth);
    }
}
