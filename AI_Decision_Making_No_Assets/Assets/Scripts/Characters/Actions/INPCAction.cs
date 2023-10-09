using Characters.Enums;

namespace Characters.Actions
{
    public interface INPCAction
    {
        public ENPCAction EnumValue { get; }
        //public bool IsPending { get; }

        public void OnStart(NPCBody body);
        public void OnUpdate(NPCBody body);
        public void OnTransit(NPCBody body);
    }
}
