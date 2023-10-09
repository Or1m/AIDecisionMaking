using Managers;
using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace GamePlay.Pickups
{
    public class Pickup : MonoBehaviour
    {
        private const float timeToMove = 0.2f;
        
        public event Action<Pickup> OnPickedUp;

        private readonly WaitForEndOfFrame waitForFrameToEnd = new WaitForEndOfFrame();

        private MeshRenderer mesh;


        private void Start()
        {
            MainManager.Instance.GameManager.RegisterPickup(this);
            mesh = GetComponentInChildren<MeshRenderer>();
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Constants.PlayerTag))
                return;

            OnPickedUp?.Invoke(this);
            StartCoroutine(LerpPosition(transform.position, Camera.Instance.PickupTarget.position, timeToMove, () => { gameObject.SetActive(false); }));
        }

        private void OnDestroy()
        {
            MainManager.Instance.GameManager.UnregisterPickup(this);
        }

        IEnumerator LerpPosition(Vector3 start, Vector3 end, float timeToMove, Action callback)
        {
            float time = 0;

            while (time < 1)
            {
                mesh.transform.position = Vector3.Lerp(start, end, time);
                time += Time.deltaTime / timeToMove;

                yield return waitForFrameToEnd;
            }

            mesh.transform.position = end;
            callback();
        }
    }
}
