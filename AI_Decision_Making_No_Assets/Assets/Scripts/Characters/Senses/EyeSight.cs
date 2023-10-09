using System.Collections;
using UnityEngine;

namespace Characters.Senses
{
    public class EyeSight : MonoBehaviour, ISense<GameObject>
    {
        [SerializeField]
        protected LayerMask targetMask;
        [SerializeField]
        protected LayerMask obstacleMask;

        [SerializeField]
        [Range(0f, 360f)]
        private float angle;
        public float Angle { get => angle; }

        [SerializeField]
        private float radius;
        public float Radius { get => radius; }

        private bool isActivated;
        public bool IsActivated { get => isActivated; }

        private float distanceToTarget;
        public float DistanceToTarget { get => distanceToTarget; }

        private GameObject target;
        private bool isActive;

        public GameObject Target { get => target; }


        private void Start()
        {
            isActive = true;
            StartCoroutine(EyeSightCoroutine());
        }

        public bool TryGetTarget(out GameObject outTarget)
        {
            outTarget = target;
            return isActivated;
        }

        private IEnumerator EyeSightCoroutine()
        {
            float delay = 0.2f;
            var wait = new WaitForSeconds(delay);

            while (isActive)
            {
                yield return wait;
                CheckEyeSight();
            }
        }
        private void CheckEyeSight()
        {
            this.target = null;
            isActivated = false;
            this.distanceToTarget = float.MaxValue;

            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
            if (rangeChecks.Length == 0)
                return;

            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) > angle / 2)
                return;

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)) // Obstacle in the way
                return;

            isActivated = true;
            this.target = target.gameObject;
            this.distanceToTarget = distanceToTarget;
        }

        private void OnDestroy()
        {
            isActive = false;
        }
    }
}
