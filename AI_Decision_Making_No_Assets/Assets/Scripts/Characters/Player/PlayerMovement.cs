using GamePlay;
using Managers;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private const float defaultYSpeed = -0.5f;
        
        [Header("Movement parameters")]
        [SerializeField]
        private float speed = 6f;
        [SerializeField]
        private float jumpSpeed = 6f;
        [SerializeField]
        private float turnSmoothTime = 0.1f;
        [SerializeField]
        private float noiseMultiplier = 0.08f;
        [SerializeField]
        private float stepDelay = 0.4f;

        private IInputManager inputManager;

        [Header("References")]
        [SerializeField]
        private CharacterController controller;
        [SerializeField]
        private Transform cameraTransform;

        private float ySpeed;
        private float vertical, horizontal;
        private float turnSmoothVelocity;
        private float currentStepDelay;

        private void Start()
        {
            currentStepDelay = stepDelay;
            inputManager = MainManager.Instance.InputManager;
        }
        void Update()
        {
            var currentSpeed = speed;

            if (controller.isGrounded)
            {
                UpdateGrounded(ref currentSpeed);
            }

            Vector3 direction = new Vector3(horizontal, 0, vertical);
            float magnitude = Mathf.Clamp01(direction.magnitude) * currentSpeed;
            direction.Normalize();

            float angle = SmoothAngleFromDirection(direction);
            if (direction != Vector3.zero)
                transform.localRotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            Vector3 velocity = moveDirection.normalized * magnitude;

            ySpeed += Physics.gravity.y * Time.deltaTime;
            velocity.y = ySpeed;

            controller.Move(velocity * Time.deltaTime);
            MakeNoise(velocity);
        }

        private void UpdateGrounded(ref float currentSpeed)
        {
            ySpeed = defaultYSpeed;

            horizontal = inputManager.Horizontal;
            vertical = inputManager.Vertical;

            if (inputManager.WasJumpingThisFrame)
                ySpeed = jumpSpeed;

            if (inputManager.IsCrounching)
                currentSpeed /= 2;
        }

        private void MakeNoise(Vector3 velocity)
        {
            currentStepDelay -= Time.deltaTime;
            
            var magnitude = Vector2.SqrMagnitude(new Vector2(velocity.x, velocity.z));
            if (magnitude == 0 || currentStepDelay > 0)
                return;

            currentStepDelay = stepDelay;
            MainManager.Instance.SoundManager.MakeNoise(new Noise(transform.localPosition, noiseMultiplier * magnitude));
        }

        private float SmoothAngleFromDirection(Vector3 direction)
        {
            var cameraY = (cameraTransform != null) ? cameraTransform.eulerAngles.y : 0.0f;

            float tempAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraY;
            return Mathf.SmoothDampAngle(transform.eulerAngles.y, tempAngle, ref turnSmoothVelocity, turnSmoothTime);
        }
    }
}
