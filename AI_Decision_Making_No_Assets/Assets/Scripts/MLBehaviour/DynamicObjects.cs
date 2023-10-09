using UnityEngine;

namespace MLBehaviour
{
    public class DynamicObjects : MonoBehaviour
    {
        private readonly float[] possibleRotations = new float[] { 0, 90, -90 };
        
        [ContextMenu("RandomRotate")]
        public void RandomRotate()
        {
            var idx = Random.Range(0, possibleRotations.Length);
            transform.eulerAngles = new Vector3(0, possibleRotations[idx], 0);
        }
    }
}
