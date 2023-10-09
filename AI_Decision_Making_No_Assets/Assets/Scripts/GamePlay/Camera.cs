using Cinemachine;
using Managers;
using UnityEngine;

namespace GamePlay
{
    public class Camera : MonoBehaviour
    {
        [SerializeField]
        private CinemachineBrain brain;

        [SerializeField]
        private UnityEngine.Camera cameraComponent;

        [SerializeField]
        private Transform pickupTarget;
        public Transform PickupTarget { get => pickupTarget; }


        private static Camera instance;
        public static Camera Instance { get => instance; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }

            instance = this;
        }

        public Vector3 ViewportToWorldPoint(Vector3 v)
        {
            return cameraComponent.ViewportToWorldPoint(v);
        }

        private void Start()
        {
            var gameManager = MainManager.Instance.GameManager;

            gameManager.OnGameEnded += DisableControls;
            gameManager.OnGameFinished += DisableControls;
            gameManager.OnPauseMenuVisibilityChanged += ChangeCameraControls;
        }

        private void ChangeCameraControls(bool enabled)
        {
            if (brain != null)
                brain.enabled = enabled;
        }
        private void EnableControls()
        {
            if (brain != null)
                brain.enabled = true;
        }
        private void DisableControls()
        {
            if (brain != null)
                brain.enabled = false;
        }

        private void OnDestroy()
        {
            var gameManager = MainManager.Instance.GameManager;

            gameManager.OnGameEnded -= DisableControls;
            gameManager.OnGameFinished -= DisableControls;
        }
    }
}
