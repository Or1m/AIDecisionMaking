using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Utils
{
    public class ScriptedPath : MonoBehaviour
    {
        [SerializeField]
        private List<WayPoint> path = new List<WayPoint>();

        private int currentWayPoint;

        public event Action OnWaypointsReset;

        private void Awake()
        {
            ResetWayPoints();
        }


        public Vector3 GetNextPoint()
        {
            return path[currentWayPoint++ % path.Count].GetVariant();
        }

        public void ResetWayPoints()
        {
            currentWayPoint = 0;
            OnWaypointsReset?.Invoke();
        }
    }
}
