using UnityEngine;

namespace MLBehaviour
{
    public class Obstacles : MonoBehaviour
    {
        private const float fixedHeight = 1.25f;
        [SerializeField]
        private GameObject[] obstacles;

        [SerializeField]
        private MeshRenderer[] boundBoxes;

        public void PlaceRandomly()
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                var bounds = Random.value < 0.5 ? boundBoxes[0].bounds : boundBoxes[1].bounds;
                obstacles[i].transform.position = RandomPointInBounds(bounds);
            }
        }
        private  Vector3 RandomPointInBounds(Bounds bounds)
        {
            return new Vector3(Random.Range(bounds.min.x, bounds.max.x), fixedHeight, Random.Range(bounds.min.z, bounds.max.z));
        }
    }
}
