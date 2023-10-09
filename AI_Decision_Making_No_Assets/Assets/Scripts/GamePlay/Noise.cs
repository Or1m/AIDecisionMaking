using UnityEngine;

namespace GamePlay
{
    public readonly struct Noise
    {
        private readonly Vector3 position;
        public Vector3 Position => position;

        private readonly float range;
        public float Range => range;


        public Noise(Vector3 position, float range)
        {
            this.position = position;
            this.range = range;
        }
    }
}
