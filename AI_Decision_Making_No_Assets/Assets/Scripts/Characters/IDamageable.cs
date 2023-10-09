using System;

namespace Characters
{
    public interface IDamageable
    {
        public float MaxHealth { get; }
        public float Health { get; }

        public event Action<float> OnHealthChanged;
        public event Action OnDied;

        public void DealDamage(float damage);
    }
}
