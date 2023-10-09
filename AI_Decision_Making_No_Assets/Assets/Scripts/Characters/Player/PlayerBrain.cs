using Characters.Player.Enums;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player
{
    public class PlayerBrain : MonoBehaviour
    {
        [SerializeField]
        private PlayerBody body;
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private PlayerMovement movement;
        [SerializeField]
        private CharacterController controller;
        [SerializeField]
        private PlayerAutomaticMovement automaticMovement;


        private void Start()
        {
            body.Initialize(movement);

            var configManager = MainManager.Instance.ConfigManager;
            if (!configManager)
                return;

            if (configManager.PlayerControll == EPlayerControllMethod.Manual)
            {
                agent.enabled = false;
                movement.enabled = true;
                automaticMovement.enabled = false;

                return;
            }

            agent.enabled = true;
            movement.enabled = false;
            automaticMovement.enabled = true;

            automaticMovement.Initialize(agent);
        }
    }
}
