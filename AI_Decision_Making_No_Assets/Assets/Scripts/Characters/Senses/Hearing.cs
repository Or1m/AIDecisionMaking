//#define HEARING_DEBUG
//using Utils;

using GamePlay;
using UnityEngine;

namespace Characters.Senses
{
    public class Hearing : MonoBehaviour, ISense<Noise>
    {
        private const double noiseStoppingDistance = 1.25;

        private bool isActivated;
        public bool IsActivated { get => isActivated; }

        private float distanceToTarget;
        public float DistanceToTarget { get => distanceToTarget; }

        private Noise target;
        public Noise Target { get => target; }

        public bool TryGetTarget(out Noise outTarget)
        {
            outTarget = target;
            return isActivated;
        }

        public void RespondToNoise(Noise noise)
        {
            target = noise;
            isActivated = true;
            distanceToTarget = Vector3.Distance(transform.localPosition, target.Position);

#if DEBUG && HEARING_DEBUG
            DebugUtils.SpawnCube(noise.Position);
#endif
        }

        private void Update()
        {
            if (!isActivated)
                return;

            var distance = Vector3.Distance(transform.localPosition, target.Position);
            if (distance > noiseStoppingDistance)
                return;

            target = new Noise();
            isActivated = false;
            distanceToTarget = 0;
        }
    }
}
