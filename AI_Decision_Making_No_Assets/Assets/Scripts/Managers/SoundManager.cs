#define HEARING_DEBUG

using Characters.Senses;
using GamePlay;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public void OnAwake() { }
        public void OnStart() { }
        
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
