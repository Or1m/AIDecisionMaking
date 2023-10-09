using System;

namespace GamePlay
{
    public interface IFinish
    {
        public event Action OnTriggered;
    }
}
