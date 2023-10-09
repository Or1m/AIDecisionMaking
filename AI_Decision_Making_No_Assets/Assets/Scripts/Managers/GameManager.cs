using Characters;
using Characters.Enums;
using GamePlay;
using GamePlay.Pickups;
using GamePlay.Spawn;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private const float uiDelay = 2f;
        private readonly WaitForSeconds uiWait = new WaitForSeconds(uiDelay);

        [SerializeField]
        private EnemySpawner spawner;

        [SerializeField]
        private int minCoins;
        [SerializeField]
        private int minBottles;

        [Header("UI Screens")]
        [SerializeField]
        private GameObject youDiedUI;
        [SerializeField]
        private GameObject youWonUI;
        [SerializeField]
        private GameObject pauseUI;

        [Header("UI Elements")]
        [SerializeField]
        private HealthBar healthBar;
        [SerializeField]
        private TextMeshProUGUI coinCounter;
        [SerializeField]
        private TextMeshProUGUI bottleCounter;

        private int pickedBottles;
        public int PickedBottles 
        { 
            get { return pickedBottles; }
            private set
            {
                pickedBottles = value; 
                bottleCounter.text = pickedBottles.ToString() + $"/{minBottles}"; 
            } 
        }

        private int pickedCoins;
        public int PickedCoins
        {
            get { return pickedCoins; }
            private set
            {
                pickedCoins = value;
                coinCounter.text = pickedCoins.ToString() + $"/{minCoins}";
            }
        }

        public event Action OnGameFinished;
        public event Action OnGameEnded;
        public event Action<bool> OnPauseMenuVisibilityChanged;


        public void OnAwake() 
        {
            // Intentionally empty
        }
        public void OnStart() 
        {
            if (!spawner)
                return;

            var controllMethod = MainManager.Instance.ConfigManager.NPCControll;

            if (controllMethod == ENPCControllMethod.DecisionTrees)
                spawner.SpawnRegularEnemies();
            else if (controllMethod == ENPCControllMethod.ReinforcementLearning)
                spawner.SpawnMLEnemies();
            else
                throw new Exception("Invalid NPC controll method");
        }

        private void Update()
        {
            if (!MainManager.Instance.InputManager.WasCancelledLastFrame)
                return;

            if (pauseUI == null)
                return;

            bool newActiveState = !pauseUI.activeInHierarchy;
            pauseUI.SetActive(newActiveState);
            Cursor.visible = newActiveState;

            OnPauseMenuVisibilityChanged?.Invoke(!newActiveState);
        }

        public void RegisterPickup(Pickup p)
        {
            p.OnPickedUp += PickupPicked;
        }
        public void RegisterFinish(IFinish finish)
        {
            finish.OnTriggered += GameFinished;
        }
        public void RegisterPlayer(IDamageable player)
        {
            player.OnDied += GameEnded;

            if (!healthBar)
                return;

            player.OnHealthChanged += healthBar.SetValue;
            healthBar.SetMaxValue(player.MaxHealth);
        }

        public void UnregisterPickup(Pickup p)
        {
            p.OnPickedUp -= PickupPicked;
        }
        public void UnregisterFinish(IFinish finish)
        {
            finish.OnTriggered -= GameFinished;
        }
        public void UnregisterPlayer(IDamageable player)
        {
            player.OnDied -= GameEnded;

            if (healthBar)
                player.OnHealthChanged -= healthBar.SetValue;
        }

        public void OpenMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private void PickupPicked(Pickup p)
        {
            p.OnPickedUp -= PickupPicked;
            
            if (p is Bottle)
                PickedBottles++;
            else
                PickedCoins++;
        }

        private bool PrerequisitesMet()
        {
            return (pickedBottles >= minBottles) && (pickedCoins >= minCoins);
        }
        private void GameFinished()
        {
            if (!PrerequisitesMet())
                return;

            OnGameFinished?.Invoke();

            if (youWonUI != null)
                youWonUI.SetActive(true);

            StartCoroutine(RestartSceneCoroutine());
        }
        private void GameEnded()
        {
            OnGameEnded?.Invoke();

            if (youDiedUI != null)
                youDiedUI.SetActive(true);

            StartCoroutine(RestartSceneCoroutine());
        }
        
        private IEnumerator RestartSceneCoroutine()
        {
            yield return uiWait;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
