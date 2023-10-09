using UnityEngine;

namespace Managers
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField]
        private bool cursorVisible;

        private IInputManager inputManager;
        public IInputManager InputManager { get => inputManager; }

        private ConfigManager configManager;
        public ConfigManager ConfigManager { get => configManager; }

        private GameManager gameManager;
        public GameManager GameManager { get => gameManager; }

        private SoundManager soundManager;
        public SoundManager SoundManager { get => soundManager; }


        private static MainManager instance;
        public static MainManager Instance { get => instance; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }

            instance = this;

            Cursor.visible = cursorVisible;
            InitializeSubManagers();
        }
        private void Start()
        {
            if (inputManager != null)
                inputManager.OnStart();

            if (configManager != null)
                configManager.OnStart();

            if (gameManager != null)
                gameManager.OnStart();

            if (soundManager != null)
                soundManager.OnStart();
        }

        private void InitializeSubManagers()
        {
            inputManager = GetComponentInChildren<IInputManager>();
            if (inputManager != null)
                inputManager.OnAwake();
            else
                Debug.LogWarning("InputManager not found");

            configManager = GetComponentInChildren<ConfigManager>();
            if (configManager != null)
                configManager.OnAwake();
            else
                Debug.LogWarning("ConfigManager not found");

            gameManager = GetComponentInChildren<GameManager>();
            if (gameManager != null)
                gameManager.OnAwake();
            else
                Debug.LogWarning("GameManager not found");

            soundManager = GetComponentInChildren<SoundManager>();
            if (soundManager != null)
                soundManager.OnAwake();
            else
                Debug.LogWarning("SoundManager not found");
        }
    }
}
