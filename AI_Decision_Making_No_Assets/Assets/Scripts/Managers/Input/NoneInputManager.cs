using UnityEngine;

namespace Managers
{
    public class NoneInputManager : MonoBehaviour, IInputManager
    {
        public float Horizontal             => 0;
        public float Vertical               => 0;
        
        public bool IsCrounching            => false;
        public bool WasJumpingThisFrame     => false;
        
        public bool WasCancelledLastFrame   => false;

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
