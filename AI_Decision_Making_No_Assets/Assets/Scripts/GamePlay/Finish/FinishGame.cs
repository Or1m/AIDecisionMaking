using Managers;
using System;
using UnityEngine;
using Utils;

namespace GamePlay
{
    public class FinishGame : MonoBehaviour, IFinish
    {
        public event Action OnTriggered;


        private void Start()
        {
            MainManager.Instance.GameManager.RegisterFinish(this);    
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Constants.PlayerTag))
                return;

            OnTriggered?.Invoke();
        }

        private void OnDestroy()
        {
            MainManager.Instance.GameManager.UnregisterFinish(this);
        }
    }
}
