using Characters.Senses;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public abstract class NPCBody : MonoBehaviour, IDamageable
    {
        [SerializeField]
        protected EyeSight eyeSight;
        public EyeSight EyeSight { get => eyeSight; }

        [SerializeField]
        protected Hearing hearing;
        public Hearing Hearing { get => hearing; }

        [SerializeField]
        protected NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } }

        protected Vector3 initialPosition;
        public Vector3 InitialPosition { get { return initialPosition; } }

        private float health;
        public float Health { get { return health; } }
        public bool SeesEnemy { get { return eyeSight.IsActivated; } }
        public bool HearEnemy { get { return hearing.IsActivated; } }
        public bool EnemyInRange { get { return eyeSight.DistanceToTarget <= Range; } }

        public abstract float MaxHealth { get; }
        public abstract float Range { get; }
        public abstract float Speed { get; }
        public abstract float RotationSpeed { get; }

        public abstract float AttackCooldown { get; }
        public abstract float AttackDamage { get; }

        public event Action<float> OnHealthChanged;
        public event Action OnDied;


        private void Awake()
        {
            health = MaxHealth;
            initialPosition = transform.position;
        }
        public void DealDamage(float damage)
        {
            health -= damage;
            OnHealthChanged?.Invoke(health);

            if (health <= 0)
                Die();
        }

        private void Die()
        {
            OnDied?.Invoke();
        }
    }
}
