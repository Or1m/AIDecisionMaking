#define HEARING_DEBUG

using Characters.Player.Utils;
using Characters.Senses;
using GamePlay;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player
{
    public class PlayerAutomaticMovement : MonoBehaviour
    {
        private const float distanceThreshold = 0.5f;

        private ScriptedPath scriptedPath;
        private NavMeshAgent agent;
        private bool initialized;

        [SerializeField]
        private float noiseMultiplier = 0.08f;
        [SerializeField]
        private float stepDelay = 0.4f;

        private float currentStepDelay;


        private void OnEnable() 
        { 
            // Intentionally empty
        }
        private void Start()
        {
            currentStepDelay = stepDelay;

            var path = transform.parent.GetComponentInChildren<ScriptedPath>();
            if (path == null) 
                return;

            scriptedPath = path;
            scriptedPath.OnWaypointsReset += SetNewDestionation;
        }

        private void SetNewDestionation()
        {
            agent.SetDestination(scriptedPath.GetNextPoint());
        }

        private void Update()
        {
            MakeNoise(agent.velocity);
            
            if (!initialized)
                return;

            if (agent.pathPending || agent.remainingDistance >= distanceThreshold)
                return;

            SetNewDestionation();
        }

        public void Initialize(NavMeshAgent agent)
        {
            this.agent = agent;
            initialized = true;
        }

        private void MakeNoise(Vector3 velocity)
        {
            currentStepDelay -= Time.deltaTime;

            var magnitude = Vector2.SqrMagnitude(new Vector2(velocity.x, velocity.z));
            
            if (magnitude == 0 || currentStepDelay > 0)
                return;

            currentStepDelay = stepDelay;
            MakeNoise(new Noise(transform.position, noiseMultiplier * magnitude));
        }

        private void OnDestroy()
        {
            if (scriptedPath != null)
                scriptedPath.OnWaypointsReset -= SetNewDestionation;
        }

        // For training
        public void MakeNoise(Noise noise)
        {
            Collider[] cols = Physics.OverlapSphere(noise.Position, noise.Range);

            var length = cols.Length;
            for (int i = 0; i < length; i++)
            {
                if (cols[i].TryGetComponent(out Hearing hearer))
                    hearer.RespondToNoise(noise);
            }

#if UNITY_EDITOR && HEARING_DEBUG
            lastNoise = noise;
#endif
        }

#if UNITY_EDITOR && HEARING_DEBUG
        private Noise lastNoise;
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lastNoise.Position, lastNoise.Range);
            lastNoise = new Noise();
        }
#endif
    }
}
