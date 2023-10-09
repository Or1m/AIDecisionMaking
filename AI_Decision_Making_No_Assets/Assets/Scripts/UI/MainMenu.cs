using Characters.Enums;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI npcControllText;
        [SerializeField]
        private TextMeshProUGUI treeAlgorithmText;
        
        
        private void Start()
        {
            MainManager.Instance.ConfigManager.OnDataChanged += OnConfigChanged;
            OnConfigChanged();
        }
        private void Update()
        {
            if (MainManager.Instance.InputManager.WasCancelledLastFrame)
                QuitGame();
        }
        private void OnDestroy()
        {
            var mainManager = MainManager.Instance;
            if (!mainManager)
                return;

            var configManager = mainManager.ConfigManager;
            if (!configManager)
                return;

            configManager.OnDataChanged -= OnConfigChanged;
        }

        public void LoadMainLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private void OnConfigChanged()
        {
            var configManager = MainManager.Instance.ConfigManager;
            var npcControll = configManager.NPCControll;

            npcControllText.text = npcControll.ToString();
            treeAlgorithmText.text = configManager.TreeAlgorithm.ToString();

            var treeGUIParent = treeAlgorithmText.gameObject.transform.parent.gameObject;
            treeGUIParent.SetActive(npcControll == ENPCControllMethod.DecisionTrees);
        }
    }
}
