using UnityEngine;

namespace Characters.Skeleton
{
    public class SkeletonBody : NPCBody
    {
        [SerializeField]
        protected int maxHealth = 120;
        public override float MaxHealth => maxHealth;

        [SerializeField]
        protected float range = 1.5f;
        public override float Range => range;

        [SerializeField]
        protected float speed = 3.85f;
        public override float Speed => speed;

        [SerializeField]
        protected float rotationSpeed = 100;
        public override float RotationSpeed => rotationSpeed;


        [SerializeField]
        protected float attackCooldown = 0.4f;
        public override float AttackCooldown => attackCooldown;

        [SerializeField]
        protected float attackDamage = 30;
        public override float AttackDamage => attackDamage;
    }
}
