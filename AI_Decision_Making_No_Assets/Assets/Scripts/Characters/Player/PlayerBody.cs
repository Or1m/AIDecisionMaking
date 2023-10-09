using Managers;
using System;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerBody : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private float maxHealth;
        [SerializeField]
        private float health;

        private bool isDead;
        private PlayerMovement movement;

        public float MaxHealth => maxHealth;
        public float Health => health;

        public event Action<float> OnHealthChanged;
        public event Action OnDied;

        
        private void Start()
        {
            var gameManager = MainManager.Instance.GameManager;
            if (!gameManager)
                return;

            gameManager.RegisterPlayer(this);
            gameManager.OnGameFinished += DisableMovement;
        }

        public void Initialize(PlayerMovement playerMovement)
        {
            movement = playerMovement;
        }

        public void DealDamage(float damage)
        {
            health -= damage;
            OnHealthChanged?.Invoke(health);

            if (health <= 0 && !isDead)
                Die();
        }

        private void Die()
        {
            isDead = true;
            DisableMovement();

            OnDied?.Invoke();
        }
        private void DisableMovement()
        {
            movement.enabled = false;
        }

        private void OnDestroy()
        {
            var gameManager = MainManager.Instance.GameManager;
            if (!gameManager)
                return;

            gameManager.UnregisterPlayer(this);
            gameManager.OnGameFinished -= DisableMovement;
        }
    }
}
