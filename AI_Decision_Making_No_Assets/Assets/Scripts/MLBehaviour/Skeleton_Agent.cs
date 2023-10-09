using Characters;
using Characters.Player.Utils;
using Characters.Skeleton;
using GamePlay;
using Unity.AI.Navigation;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBehaviour
{
    public class Skeleton_Agent : Agent
    {
        private const float walkRadius = 14.5f;
        private const float playerStartLocalZ = 18f;

        [SerializeField]
        private Material winMaterial;
        [SerializeField]
        private Material looseMaterial;

        [SerializeField]
        private SkeletonBody body;
        [SerializeField]
        private Transform playerTransform;
        [SerializeField]
        private Rigidbody rb;

        private DynamicObjects dynamicObjects;
        private NavMeshSurface navMeshSurface;
        private Obstacles obstacles;
        private MeshRenderer groundRenderer;
        private ScriptedPath scriptedPath;
        private float duration;
        private Vector3 lastSoundPosition;
        private RayPerceptionSensorComponent3D rayPerceptionSensor;

        public Vector3 initialPosition;

        public override void Initialize()
        {
            initialPosition = transform.position;

            navMeshSurface = FindObjectOfType<NavMeshSurface>();
            if (navMeshSurface == null)
                Debug.LogError("NavMeshSurface is null");

            var scenario = transform.parent;
            if (scenario == null)
            {
                Debug.LogError("Scenario is null");
                return;
            }

            obstacles = scenario.GetComponentInChildren<Obstacles>();
            if (obstacles == null)
                Debug.LogError("Obstacles is null");

            dynamicObjects = scenario.GetComponentInChildren<DynamicObjects>();
            if (dynamicObjects == null)
                Debug.LogError("DynamicObjects is null");

            scriptedPath = scenario.GetComponentInChildren<ScriptedPath>();
            if (scriptedPath == null)
                Debug.LogError("ScriptedPath is null");

            var finish = scenario.GetComponentInChildren<IFinish>();
            if (finish != null)
                finish.OnTriggered += FailEpisode;

            if (!TryGetComponent(out rayPerceptionSensor))
                Debug.LogError("RayPerceptionSensorComponent3D not found");
            
            var ground = scenario.GetComponentInChildren<Ground>();
            if (ground == null)
            {
                Debug.LogError("Ground is null");
                return;
            }

            if (!ground.gameObject.TryGetComponent(out groundRenderer))
                Debug.LogError("MeshRenderer on ground not found");
        }
        public override void OnEpisodeBegin()
        {
            transform.localPosition = Vector3.zero;

            if (dynamicObjects == null)
                return;

            dynamicObjects.RandomRotate();
            playerTransform.localPosition = new Vector3(0f, 0f, playerStartLocalZ);

            if (scriptedPath != null)
                scriptedPath.ResetWayPoints();

            PlaceObstacles();
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


            if (!IsInBounds())
                return;

            var playerDistance = Vector3.Distance(transform.position, playerTransform.position);
            if (playerDistance <= body.Range && IsInFOV(playerTransform.position))
            {
                OnEpisodeSucceeded();
                //Attack();
            }

            //var soundDistance = Vector3.Distance(transform.position, lastSoundPosition);
            //Debug.Log(soundDistance);
            //if (lastSoundPosition != Vector3.zero && soundDistance <= body.Range / 2)
            //{
            //    //OnEpisodeSucceeded(0.1f);
            //    AddReward(10f);
            //}

            //if (distance < lastDistance)
            //{
            //    AddReward(10f / MaxStep);
            //}
            //else
            //{
            //    AddReward(-1f / MaxStep);
            //}

            AddReward(-1f / MaxStep);
            //lastDistance = distance;
        }

        private bool IsInFOV(Vector3 position)
        {
            Vector3 directionToTarget = (position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            return angle <= rayPerceptionSensor.MaxRayDegrees;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("wall"))
                OnEpisodeFailed();
        }

        protected override void OnDisable()
        {
            var scenario = transform.parent.transform;
            if (scenario == null)
                return;

            var finish = scenario.GetComponentInChildren<IFinish>();
            if (finish != null)
                finish.OnTriggered -= FailEpisode;
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


        private bool IsInBounds()
        {
            var xPos = transform.localPosition.x;
            var zPos = transform.localPosition.z;

            if (xPos > walkRadius)
                return OnEpisodeFailed();
            else if (xPos < -walkRadius)
                return OnEpisodeFailed();

            if (zPos > walkRadius)
                return OnEpisodeFailed();
            else if (zPos < -walkRadius)
                return OnEpisodeFailed();

            return true;
        }

        private void FailEpisode()
        {
            OnEpisodeFailed();
        }
        private bool OnEpisodeFailed()
        {
            if (groundRenderer != null)
                groundRenderer.material = looseMaterial;

            AddReward(-10.0f);
            EndEpisode();

            return false;
        }
        private bool OnEpisodeSucceeded(float multiplier = 1.0f)
        {
            if (groundRenderer != null)
                groundRenderer.material = winMaterial;

            AddReward(100.0f * multiplier);
            EndEpisode();

            return true;
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

        private void PlaceObstacles()
        {
            if (obstacles == null)
                return;

            obstacles.PlaceRandomly();

            if (navMeshSurface == null)
                return;

            navMeshSurface.BuildNavMesh();
        }
    }
}
