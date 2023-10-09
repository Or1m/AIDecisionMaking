using UnityEngine;

namespace Managers
{
    public class OldInputManager : MonoBehaviour, IInputManager
    {
        public bool IsCrounching        => Input.GetButton("Crouch");
        public bool WasJumpingThisFrame => Input.GetButtonDown("Jump");
        public float Vertical           => Input.GetAxisRaw("Vertical");
        public float Horizontal         => Input.GetAxisRaw("Horizontal");

        public bool WasCancelledLastFrame => Input.GetButtonDown("Cancel");

        public void OnAwake() 
        {
            // Intentionally empty
        }
        public void OnStart() 
        {
            // Intentionally empty
        }
    }
}
