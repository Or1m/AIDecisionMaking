namespace Managers
{
    public interface IInputManager
    {
        float Horizontal { get; }
        float Vertical { get; }

        bool IsCrounching { get; }
        bool WasJumpingThisFrame { get; }

        public bool WasCancelledLastFrame { get; }

        public void OnAwake();
        void OnStart();
    }
}
