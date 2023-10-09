using UnityEngine;

namespace Utils
{
    public static class DebugUtils
    {
        public static void SpawnCube(Vector3 position)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localPosition = position;
            var collider = cube.GetComponent<BoxCollider>();
            collider.enabled = false;
        }
    }
}
