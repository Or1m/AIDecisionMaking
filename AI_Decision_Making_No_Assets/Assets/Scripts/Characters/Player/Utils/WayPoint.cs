using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Utils
{
    [System.Serializable]
    public struct WayPoint
    {
        public List<Transform> wayPointVariants;

        public Vector3 GetVariant()
        {
            int idx = Random.Range(0, wayPointVariants.Count);
            return wayPointVariants[idx].position;
        }
    }
}
