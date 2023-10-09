using Characters;
using Characters.Skeleton;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Utils;

namespace MLBehaviour
{
    public class MLSkeleton : Agent
    {
        private const float walkRadius = 14.5f;

        [SerializeField]
        private SkeletonBody body;
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private RayPerceptionSensorComponent3D rayPerceptionSensor;

        private float duration;
        private Vector3 lastSoundPosition;

        private Transform playerTransform;
        private Vector3 initialPosition;


        public override void Initialize()
        {
            initialPosition = transform.position;

            var playerTransform = GameObject.FindGameObjectWithTag(Constants.PlayerTag).transform;
            if (playerTransform != null)
                this.playerTransform = playerTransform;
        }
        public override void OnEpisodeBegin()
        {
            transform.localPosition = Vector3.zero;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            lastSoundPosition = initialPosition;
            bool soundPositionValid = false;

            if (body.Hearing.TryGetTarget(out var target))
            {
                lastSoundPosition = target.Position;
                soundPositionValid = true;
            }

            sensor.AddObservation(GetRelativePosition(transform, lastSoundPosition));
            sensor.AddObservation(soundPositionValid);
        }
        public override void OnActionReceived(ActionBuffers actions)
        {
            MoveAgent(actions.DiscreteActions);

            var playerDistance = Vector3.Distance(transform.position, playerTransform.position);
            if (playerDistance <= body.Range && IsInFOV(playerTransform.position))
            {
                Attack();
            }

            AddReward(-1f / MaxStep);
        }

        public void MoveAgent(ActionSegment<int> act)
        {
            var direction = Vector3.zero;
            var rotation = Vector3.zero;

            var action = act[0];

            switch (action)
            {
                case 1:
                    direction = transform.forward * 1f;
                    break;
                case 2:
                    direction = transform.forward * -1f;
                    break;
                case 3:
                    rotation = transform.up * 1f;
                    break;
                case 4:
                    rotation = transform.up * -1f;
                    break;
                case 5:
                    direction = transform.right * -0.75f;
                    break;
                case 6:
                    direction = transform.right * 0.75f;
                    break;
            }

            transform.Rotate(rotation, Time.fixedDeltaTime * body.RotationSpeed, Space.World);
            var move = body.Speed * Time.fixedDeltaTime * direction;
            transform.Translate(move, Space.World);
        }
        private bool IsInFOV(Vector3 position)
        {
            Vector3 directionToTarget = (position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            return angle <= rayPerceptionSensor.MaxRayDegrees;
        }
        private void Attack()
        {
            duration += Time.deltaTime;
            if (duration < body.AttackCooldown)
                return;

            if (playerTransform.TryGetComponent<IDamageable>(out var damageable)) 
            {
                damageable.DealDamage(body.AttackDamage);
                duration = 0;
            }
        }

        public static Vector3 GetRelativePosition(Transform origin, Vector3 position)
        {
            Vector3 distance = position - origin.position;
            Vector3 relativePosition = Vector3.zero;
            relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
            relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
            relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

            return relativePosition;
        }
    }
}
