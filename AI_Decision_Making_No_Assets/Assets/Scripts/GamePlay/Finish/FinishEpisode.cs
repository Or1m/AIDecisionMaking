using System;
using UnityEngine;
using Utils;

namespace GamePlay
{
    public class FinishEpisode : MonoBehaviour, IFinish
    {
        public event Action OnTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Constants.PlayerTag))
                return;

            OnTriggered?.Invoke();
        }
    }
}
